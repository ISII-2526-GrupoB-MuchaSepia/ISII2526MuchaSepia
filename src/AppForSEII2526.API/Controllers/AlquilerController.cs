using AppForSEII2526.API.DTOs.AlquilerDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Components.Forms;
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

        [HttpGet] //Get_Alquiler_test (DETAIL)
        [Route("[action]")]
        [ProducesResponseType(typeof(DetalleAlquilerDTO), (int)HttpStatusCode.OK)] //200 OK CON UN DETALLEALQUILERDTO SI LO ENCUENTRA
        [ProducesResponseType((int)HttpStatusCode.NotFound)] //404 NOT FOUND SI NO LO ENCUENTRA
        public async Task<ActionResult> GetDetalleAlquiler(int id)
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
                .Include(a=> a.ApplicationUser) //El usuario que hizo la reserva.
                .Include(a => a.AlquilerItems) //Los items del alquiler.
                .ThenInclude(ai => ai.Coche)//Cada coche de esos items. por eso es thenInclude
                .ThenInclude(coche => coche.Modelo) //El modelo de cada coche.
                .Select(a => new DetalleAlquilerDTO(
                    
                    a.FechaAlquiler,
                    a.ApplicationUser.Nombre,
                    a.ApplicationUser.Apellido,
                    a.ConcesionarioEntrega,
                    a.InicioAlquiler,
                    a.FinAlquiler,
                    a.MetodoPago,
                    a.Total,
                    
                    a.AlquilerItems.Select(ai => new AlquilerItemDTO(
                        
                        ai.Cantidad,
                        ai.Coche.PrecioAlquiler,
                        ai.Coche.Modelo.Name,
                        ai.Coche.Fabricante
                        )).ToList<AlquilerItemDTO>()))
                   
                    

                  .FirstOrDefaultAsync(); //devuelve null si no existe el alquiler con ese id

            if (alquiler == null)
            {
                _logger.LogError($"Error: Alquiler con id :  {id} no existe"); 
                return NotFound();
            }

            return Ok(alquiler); //devuelve 200 OK con el detalle del alquiler

        }

       
       
        //5. El sistema muestra los coches seleccionados, indicando su modelo, fabricante y
        // precio de alquiler, y solicita al usuario su nombre, apellidos, dirección y método de pago
        //(Visa, Google Pay o Paypal) y la cantidad de cada coche seleccionado, siendo todos datos
        //obligatorios.


        //CREA UN NUEVO ALQUILER APLICANCO VALIDACIONES, Y DEVUELVE EL TOTAL Y DETALLE DEL ALQUILER


        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DetalleAlquilerDTO), (int)HttpStatusCode.Created)] //201 si se crea correctamente
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)] //400 si hay errores de validacion
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)] //409 si hay conflicto al guardar en la base de datos
        public async Task<ActionResult> CreateAlquiler(AlquilerParaCrearDTO alquilerParaCrear) //Crear_Alquiler_test (POST)
        {


           //VALIDACIONES INICIALES

            if (alquilerParaCrear.InicioAlquiler <= DateTime.Today)
                ModelState.AddModelError("InicioAlquiler", "¡Error! La fecha de inicio debe ser posterior a hoy.");

            if (alquilerParaCrear.InicioAlquiler >= alquilerParaCrear.FinAlquiler)
                ModelState.AddModelError("InicioAlquilerFinAlquiler", "¡Error! La fecha de finalización debe ser posterior a la de inicio.");


            if (string.IsNullOrEmpty(alquilerParaCrear.Nombre) || string.IsNullOrEmpty(alquilerParaCrear.Apellido))
                ModelState.AddModelError("Usuario", "Debe indicar nombre y apellidos.");

            if (string.IsNullOrEmpty(alquilerParaCrear.ConcesionarioEntrega))
                ModelState.AddModelError("ConcesionarioEntrega", "Debe indicar un concesionario de entrega.");

            if (!alquilerParaCrear.ConcesionarioEntrega.Contains("Calle"))
            {
                ModelState.AddModelError("ConcesionarioEntrega", "la dirección debe contener la palabra calle");
                return BadRequest(new ValidationProblemDetails(ModelState));

            }






           // Evitar crear un alquiler con un usuario inexistente en la base de datos.

            var usuario = _context.ApplicationUsers.FirstOrDefault(au => au.NombreUsuario == alquilerParaCrear.NombreUsuario);
            if (usuario == null)
            {
                ModelState.AddModelError("Usuario", "El usuario indicado no existe."); //Si el nombre no corresponde a ningún usuario, se agrega otro error.
            }

            if (alquilerParaCrear.AlquilerItems.Count == 0)
                ModelState.AddModelError("AlquilerItems", "Debe seleccionar al menos un coche para alquilar.");
            // FLUJO ALTERNATIVO 4: Si falta algún dato obligatorio o hay un error de validación, el sistema notifica al usuario y regresa al paso anterior para corregirlo.
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));
           

            var cochesSeleccionados = alquilerParaCrear.AlquilerItems
                .Select(ai => ai.Modelo)
                .ToList<string>(); //guardo los modelos de los coches que hemos seleccionado

            //   Buscamos en la BD los coches que coinciden con los modelos seleccionados
            var coches = _context.Coches
                .Include(a => a.AlquilerItems)
                    .ThenInclude(ai => ai.Coche).ThenInclude(coche=>coche.Modelo)

              .Where(a => cochesSeleccionados.Contains(a.Modelo.Name))

                //  Proyectamos solo los datos necesarios
                .Select(coche=> new
                {
                    coche.Id,
                    coche.Modelo.Name,
                  
                    coche.PrecioAlquiler,
                    coche.CantidadAlquiler,
                    // // Contamos cuántos coches de este modelo YA ESTÁN ALQUILADOS en este rango de fechas
                    NumeroCochesAlquilados = coche.AlquilerItems.Count(ai =>
                        ai.Alquiler.InicioAlquiler <= alquilerParaCrear.FinAlquiler &&
                        ai.Alquiler.FinAlquiler >= alquilerParaCrear.InicioAlquiler)
                })
                .ToList();


            //  CREACIÓN DEL ALQUILER

            Alquiler alquiler = new Alquiler(


           alquilerParaCrear.ConcesionarioEntrega,
           alquilerParaCrear.FechaAlquiler, //FECHA ACTUAL
           alquilerParaCrear.MetodoPago,
           alquilerParaCrear.InicioAlquiler,
           alquilerParaCrear.FinAlquiler,
           new List<AlquilerItem>(),
           usuario);
            



            alquiler.Total = 0;

            // Calculamos la cantidad de días del alquiler
            var numeroDias =  (int)Math.Ceiling((alquiler.FinAlquiler - alquiler.InicioAlquiler).TotalDays);
            //  VALIDACIÓN DE DISPONIBILIDAD DE CADA COCHE

            foreach (var item in alquilerParaCrear.AlquilerItems)
            {
                // Buscamos el coche seleccionado
                var coche = coches.FirstOrDefault(coche => coche.Name == item.Modelo);

                // Validamos existencia y disponibilidad
                if (coche == null || (coche.NumeroCochesAlquilados + item.Cantidad) >= coche.CantidadAlquiler)
                {
                    ModelState.AddModelError("AlquilerItems",
                        $"Error: El coche '{item.Modelo}' no está disponible para las fechas del " +
                        $"{alquilerParaCrear.InicioAlquiler.ToShortDateString()} al {alquilerParaCrear.FinAlquiler.ToShortDateString()}");
                }
                else
                {

                    // // Si el coche está disponible → se añade el AlquilerItem
                    alquiler.AlquilerItems.Add(new AlquilerItem(coche.Id, alquiler, item.Cantidad));
                    // Asignamos el precio para que el cliente lo vea
                    item.PrecioAlquiler = coche.PrecioAlquiler;
                    // Sumamos al total
                    alquiler.Total+= coche.PrecioAlquiler * item.Cantidad * numeroDias;
                }
            }
         

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                
                //NUEVO CONTROL DE ERROR POR DISPONIBILIDAD DE COCHES
            }

            _context.Add(alquiler);

            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Alquiler", "Error al guardar el alquiler en la base de datos.");
                return Conflict("Error" + ex.Message);
            }

            var alquilerDetalle = new DetalleAlquilerDTO(
    // Crear el DetalleAlquilerDTO con la información del alquiler y se devuelve al cliente con HTTP 201 Created

    alquiler.FechaAlquiler,
    alquiler.ApplicationUser.Nombre,
    alquiler.ApplicationUser.Apellido,
    alquiler.ConcesionarioEntrega,
    alquiler.InicioAlquiler,
    alquiler.FinAlquiler,
    alquiler.MetodoPago,
    alquiler.Total,
   alquilerParaCrear.AlquilerItems);

            _logger.LogInformation(" El alquiler {alquiler.Id} se ha creado correctamente");

            // Retornamos un 201 Created con el recibo completo
            return CreatedAtAction("Get_Detalle_Alquiler", new { id = alquiler.Id }, alquilerDetalle);








        }

    }
}
