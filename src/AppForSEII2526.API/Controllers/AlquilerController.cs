using AppForSEII2526.API.DTOs.AlquilerDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlquilerController : ControllerBase
    {

        private readonly ApplicationDbContext _context; // PARA ACCEDER A LA BASE DE DATOS
        private readonly ILogger<AlquilerController> _logger; // PARA REGISTRAR LOGS


        public AlquilerController(ApplicationDbContext context, ILogger<AlquilerController> logger)
        {
            _context = context;
            _logger = logger;
        }


        //7. El sistema muestra un recibo con la información del alquiler indicando el nombre,
        //apellidos y dirección del usuario, coches alquilados(indicando su modelo, fabricante,
        //precio y cantidad), método de pago utilizado, fecha de inicio y fecha de finalización del
        //alquiler, fecha en la que se realizó el alquiler y precio total del alquiler.


        //DEVUELVE EL RECIBO/DETALLE DE UN ALQUILER POR SU ID

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DetalleAlquilerDTO), (int)HttpStatusCode.OK)] //200 OK CON UN DETALLEALQUILERDTO SI LO ENCUENTRA
        [ProducesResponseType((int)HttpStatusCode.NotFound)] //404 NOT FOUND SI NO LO ENCUENTRA
        public async Task<ActionResult> GetAlquiler(int id)
        //endpoint GET asincrono que dado un id, devuelve un ActionResult( ejemplo: Ok(), NotFound(), BadRequest(), etc)
        {
            if (_context.Alquileres == null)
            //si el Dbset de Alquiler no esta configurado en el ApplicationDbContext, registra un error y devuelve 404
            {
                _logger.LogError("Error: La tabla de alquileres no existe.");
                return NotFound();
            }
            var alquiler = await _context.Alquileres
                .Where(a => a.Id == id)//filtra por el id del alquiler
                .Include(a=> a.ApplicationUser)
                .Include(a => a.AlquilerItems)
                .ThenInclude(ai => ai.Coche)
                .ThenInclude(coche => coche.Modelo)
                .Select(a => new DetalleAlquilerDTO(
                    a.Id,
                    a.FechaAlquiler,
                    a.Nombre,
                    a.Apellido,
                    a.ConcesionarioEntrega,
                    a.InicioAlquiler,
                    a.FinAlquiler,
                    a.MetodoPago,
                    a.AlquilerItems.Select(ai => new AlquilerItemDTO(
                        ai.Coche.Id,
                        ai.Cantidad,
                        ai.Coche.PrecioAlquiler,
                        ai.Coche.Modelo.Name,
                        ai.Coche.Fabricante
                        )).ToList(),
                    a.Total
                    ))

                  .FirstOrDefaultAsync(); //devuelve null si no existe el alquiler con ese id

            if (alquiler == null)
            {
                _logger.LogError($"Error: Alquiler con id :  {id} no existe"); 
                return NotFound();
            }

            return Ok(alquiler); //devuelve 200 OK con el detalle del alquiler

        }

       
    }
}
