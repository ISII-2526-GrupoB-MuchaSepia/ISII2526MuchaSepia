using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs.ReseñarDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReseñarController : ControllerBase
    {
        private readonly ApplicationDbContext _context; // PARA ACCEDER A LA BASE DE DATOS
        private readonly ILogger<ReseñarController> _logger; // PARA REGISTRAR LOGS

        public ReseñarController(ApplicationDbContext context, ILogger<ReseñarController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // METODO GET PARA OBTENER DETALLES DE RESEÑAS
        // PERMITE FILTRAR POR ID, USUARIO, PAIS, TIPO DE CONDUCTOR Y FECHAS
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)] // RETORNA LA RESEÑA O LISTA DE RESEÑAS CON DETALLES
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // SI LA FECHA INICIO ES MAYOR A LA FIN
        [ProducesResponseType(StatusCodes.Status404NotFound)] // SI NO SE ENCUENTRA LA RESEÑA PEDIDA
        public async Task<ActionResult> GetDetails(
            int? id = null,
            string? usuario = null,
            string? pais = null,
            string? tipoConductor = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null)
        {
            // VALIDA QUE LA FECHA INICIO SEA ANTERIOR A LA FECHA FIN
            if (fechaInicio.HasValue && fechaFin.HasValue && fechaInicio > fechaFin)
            {
                ModelState.AddModelError("", "La fecha de inicio debe ser anterior a la fecha de fin.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            if (id.HasValue)
            {
                // OBTENER SOLO UNA RESEÑA ESPECÍFICA POR ID INCLUYENDO SUS ITEMS Y COCHES
                var reseña = await _context.Reseñas
                    .Where(r => r.Id == id.Value)
                    .Include(r => r.ReseñarItems)
                        .ThenInclude(ri => ri.Coche)
                    .Select(r => new DetallesReseñarDTO(
                        r.Id,
                        r.Creado,
                        r.Usuario,
                        r.Pais,
                        r.TipoConductor,
                        r.ApplicationUser,
                        r.ReseñarItems.Select(ri => new ReseñarItemDTO(
                            ri.ReseñarId,
                            ri.CocheId,
                            ri.Coche.ClaseCoche,
                            ri.Calificacion,
                            ri.Descripcion ?? string.Empty
                        )).ToList()
                    ))
                    .FirstOrDefaultAsync();

                if (reseña == null)
                {
                    _logger.LogError($"Error: Reseña con id {id.Value} no existe.");
                    return NotFound();
                }
                return Ok(reseña);
            }
            else
            {
                // OBTENER LISTA DE RESEÑAS CON FILTROS DINÁMICOS
                var query = _context.Reseñas
                    .Include(r => r.ReseñarItems)
                        .ThenInclude(ri => ri.Coche)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(usuario))
                    query = query.Where(r => r.Usuario.Contains(usuario));

                if (!string.IsNullOrEmpty(pais))
                    query = query.Where(r => r.Pais == pais);

                if (!string.IsNullOrEmpty(tipoConductor))
                    query = query.Where(r => r.TipoConductor == tipoConductor);

                if (fechaInicio.HasValue)
                    query = query.Where(r => r.Creado >= fechaInicio.Value);

                if (fechaFin.HasValue)
                    query = query.Where(r => r.Creado <= fechaFin.Value);

                var reseñas = await query
                    .Select(r => new DetallesReseñarDTO(
                        r.Id,
                        r.Creado,
                        r.Usuario,
                        r.Pais,
                        r.TipoConductor,
                        r.ApplicationUser,
                        r.ReseñarItems.Select(ri => new ReseñarItemDTO(
                            ri.ReseñarId,
                            ri.CocheId,
                            ri.Coche.ClaseCoche,
                            ri.Calificacion,
                            ri.Descripcion ?? string.Empty
                        )).ToList()
                    ))
                    .ToListAsync();

                return Ok(reseñas);
            }
        }

        // METODO POST PARA CREAR NUEVAS RESEÑAS
        // VALIDA EL OBJETO DE ENTRADA Y LA EXISTENCIA DE USUARIO Y COCHES
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(typeof(DetallesReseñarDTO), StatusCodes.Status201Created)] // RETORNA DTO DE RESEÑA CREADA
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)] // ERRORES DE VALIDACION
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)] // CONFLICTOS AL GUARDAR EN BASE DE DATOS
        public async Task<ActionResult> Create(CreacionesReseñarDTO reseñaForCreate)
        {
            // VALIDAR QUE LA LISTA DE ITEMS NO ESTE VACÍA
            if (reseñaForCreate.ReseñarItems == null || !reseñaForCreate.ReseñarItems.Any())
            {
                ModelState.AddModelError("", "Debes añadir al menos una reseña de coche.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // VALIDAR TIPO DE CONDUCTOR ESTRICTO (SÓLO TITULAR O ADICIONAL)
            if (reseñaForCreate.TipoConductor != "Titular" && reseñaForCreate.TipoConductor != "Adicional")
            {
                ModelState.AddModelError("", "El tipo de conductor debe ser 'Titular' o 'Adicional'.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // VERIFICAR QUE EL USUARIO EXISTE EN LA BASE DE DATOS
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == reseñaForCreate.Usuario);
            if (user == null)
            {
                ModelState.AddModelError("Usuario", "Error! Usuario no registrado.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

           
           

            
            

            // VERIFICAR QUE CADA COCHE DE LOS ITEMS EXISTE
            foreach (var itemDto in reseñaForCreate.ReseñarItems)
            {
                var coche = await _context.Coches.FindAsync(itemDto.CocheId);
                if (coche == null)
                {
                    ModelState.AddModelError("", $"Error! El coche {itemDto.CocheNombre} no existe.");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }
            }

            //EJERCICIOS EXAMEN
            foreach (var examen in reseñaForCreate.ReseñarItems)
            {

                
                if (!String.IsNullOrEmpty(examen.Descripcion) && !examen.Descripcion.StartsWith("Reseña para")) 
                {
                    ModelState.AddModelError("", "¡Error! La reseña debe empezar por Reseña para");
                    //return BadRequest(new ValidationProblemDetails(ModelState));
                }
            }


            // CREAR OBJETO RESEÑA CON LOS DATOS PROPORCIONADOS
            var reseña = new Reseñar
            {
                Usuario = reseñaForCreate.Usuario,
                Pais = reseñaForCreate.Pais,
                TipoConductor = reseñaForCreate.TipoConductor,
                Creado = reseñaForCreate.Creado,
                ApplicationUser = user,
                ReseñarItems = new System.Collections.Generic.List<ReseñarItem>()
            };
           
            // AÑADIR CADA ITEM A LA RESEÑA CON LOS DATOS CORRESPONDIENTES
            foreach (var itemDto in reseñaForCreate.ReseñarItems)
            {
                var coche = await _context.Coches.FindAsync(itemDto.CocheId);

                reseña.ReseñarItems.Add(new ReseñarItem(
                    coche,
                    itemDto.Calificacion,
                    reseña,
                    itemDto.Descripcion ?? string.Empty
                ));
            }

            // AGREGAR RESEÑA AL CONTEXTO DE BASE DE DATOS
            _context.Reseñas.Add(reseña);

            try
            {
                // GUARDAR CAMBIOS EN LA BASE DE DATOS ASINCRÓNICAMENTE
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // SI FALLA AL GUARDAR, LOGUEAR ERROR Y DEVOLVER CONFLICTO
                _logger.LogError(ex.Message);
                return Conflict("Error al guardar la reseña: " + ex.Message);
            }
            

            // PROYECTAR EL DTO DE LA RESEÑA CREADA PARA RESPUESTA
            var detallesDTO = new DetallesReseñarDTO(
                reseña.Id,
                reseña.Creado,
                reseña.Usuario,
                reseña.Pais,
                reseña.TipoConductor,
                reseña.ApplicationUser,
                reseña.ReseñarItems.Select(ri => new ReseñarItemDTO(
                    ri.ReseñarId,
                    ri.CocheId,
                    ri.Coche.ClaseCoche,
                    ri.Calificacion,
                    ri.Descripcion)).ToList());
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            // 6. Añade la compra al contexto para guardarla en la base de datos
            _context.Add(reseña);

            // RETORNAR RESPUESTA 201 CREATED INCLUYENDO EL DTO Y ENLACE A METODO GET PARA DETALLES
            return CreatedAtAction(nameof(GetDetails), new { id = reseña.Id }, detallesDTO);
        }
    }
}
