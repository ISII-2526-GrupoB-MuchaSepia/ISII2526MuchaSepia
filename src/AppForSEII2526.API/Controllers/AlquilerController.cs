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
        public async Task<ActionResult> Get_Detalle_Alquiler(int id)
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
        public async Task<ActionResult> CreateAlquiler(AlquilerParaCrearDTO alquilerParaCrear)
        {

            if (alquilerParaCrear.InicioAlquiler <= DateTime.Today)
                ModelState.AddModelError("InicioAlquiler", "¡Error! La fecha de inicio debe ser posterior a hoy.");

            if (alquilerParaCrear.InicioAlquiler >= alquilerParaCrear.FinAlquiler)
                ModelState.AddModelError("InicioAlquilerFinAlquiler", "¡Error! La fecha de finalización debe ser posterior a la de inicio.");


            if (string.IsNullOrEmpty(alquilerParaCrear.Nombre) || string.IsNullOrEmpty(alquilerParaCrear.Apellido))
                ModelState.AddModelError("Usuario", "Debe indicar nombre y apellidos.");

            if (string.IsNullOrEmpty(alquilerParaCrear.ConcesionarioEntrega))
                ModelState.AddModelError("ConcesionarioEntrega", "Debe indicar un concesionario de entrega.");

            //Flujo alternativo 2: No se han seleccionado coches para alquilar
            if (alquilerParaCrear.AlquilerItems.Count == 0)
                ModelState.AddModelError("AlquilerItems", "Debe seleccionar al menos un coche para alquilar.");



            var usuario = _context.ApplicationUsers.FirstOrDefault(au => au.Nombre == alquilerParaCrear.Nombre);
            if (usuario == null)
            {
                ModelState.AddModelError("Usuario", "El usuario indicado no existe."); //Si el nombre no corresponde a ningún usuario, se agrega otro error.
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));
            // FLUJO ALTERNATIVO 4: Si falta algún dato obligatorio o hay un error de validación, el sistema notifica al usuario y regresa al paso anterior para corregirlo.

            // 1️⃣ Obtenemos los IDs de los coches seleccionados
            var cochesSeleccionados = alquilerParaCrear.AlquilerItems
                .Select(ai => ai.CocheId)
                .ToList();

            // 2️⃣ Buscamos los coches en la base de datos e incluimos sus alquileres
            var coches = _context.Coches
                .Include(c => c.AlquilerItems)
                    .ThenInclude(ai => ai.Alquiler)

                .Where(c => cochesSeleccionados.Contains(c.Id))

                // 3️⃣ Proyectamos solo los datos necesarios
                .Select(c => new
                {
                    c.Id,
                    c.Modelo.Name,
                    c.Fabricante,
                    c.PrecioAlquiler,
                    c.CantidadAlquiler,
                    // 4️⃣ Contamos los coches alquilados dentro del rango de fechas
                    NumeroCochesAlquilados = c.AlquilerItems.Count(ai =>
                        ai.Alquiler.InicioAlquiler <= alquilerParaCrear.FinAlquiler &&
                        ai.Alquiler.FinAlquiler >= alquilerParaCrear.InicioAlquiler)
                })
                .ToList();

            Alquiler alquiler = new Alquiler(
           alquilerParaCrear.Nombre,
           alquilerParaCrear.Apellido,

           alquilerParaCrear.ConcesionarioEntrega,
           DateTime.Now, //FECHA ACTUAL
           alquilerParaCrear.MetodoPago,
           alquilerParaCrear.InicioAlquiler,
           alquilerParaCrear.FinAlquiler,
           new List<AlquilerItem>(),
           usuario)
            {
                FechaAlquiler = DateTime.Now
            };



            alquiler.Total = 0;

            // Calculamos la cantidad de días del alquiler
            var numeroDias = (alquiler.FinAlquiler - alquiler.InicioAlquiler).TotalDays;

            foreach (var item in alquilerParaCrear.AlquilerItems)
            {
                // Buscamos el coche seleccionado
                var coche = coches.FirstOrDefault(c => c.Id == item.CocheId);

                // Validamos existencia y disponibilidad
                if (coche == null || coche.NumeroCochesAlquilados >= coche.CantidadAlquiler)
                {
                    ModelState.AddModelError("AlquilerItems",
                        $"Error: El coche '{item.CocheId}' no está disponible para las fechas del " +
                        $"{alquilerParaCrear.InicioAlquiler.ToShortDateString()} al {alquilerParaCrear.FinAlquiler.ToShortDateString()}");
                }
                else
                {


                    alquiler.AlquilerItems.Add(new AlquilerItem(coche.Id, alquiler.Id, item.Cantidad));
                    item.PrecioAlquiler = coche.PrecioAlquiler;
                }
            }
            alquiler.Total = alquiler.AlquilerItems.Sum(ai => ai.Cantidad * numeroDias * coches.First(c => c.Id == ai.CocheId).PrecioAlquiler);

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
                alquiler.Id,
    alquiler.FechaAlquiler,
    alquiler.Nombre,
    alquiler.Apellido,
    alquiler.ConcesionarioEntrega,
    alquiler.InicioAlquiler,
    alquiler.FinAlquiler,
    alquiler.MetodoPago,
    alquiler.AlquilerItems.Select(ai => new AlquilerItemDTO(
        ai.Coche.Id,
        (int)ai.Cantidad,
        ai.Coche.PrecioAlquiler,
        ai.Coche.Modelo.Name,
        ai.Coche.Fabricante
    )).ToList(),
    alquiler.Total
);

            // Retornamos un 201 Created con el recibo completo
            return CreatedAtAction("GetAlquiler", new { id = alquiler.Id }, alquilerDetalle);








        }

    }
}
