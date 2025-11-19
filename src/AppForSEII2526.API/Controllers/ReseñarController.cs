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
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReseñarController> _logger;

        public ReseñarController(ApplicationDbContext context, ILogger<ReseñarController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Reseñar/GetDetails?id=#
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)] // Retorna DetallesReseñarDTO o lista
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetDetails(
            int? id = null,
            string? usuario = null,
            string? pais = null,
            string? tipoConductor = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null)
        {
            if (fechaInicio.HasValue && fechaFin.HasValue && fechaInicio > fechaFin)
            {
                ModelState.AddModelError("", "La fecha de inicio debe ser anterior a la fecha de fin.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            if (id.HasValue)
            {
                // Obtener sólo la reseña concreta por id
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
                    _logger.LogError($"Error: Reseña with id {id.Value} does not exist");
                    return NotFound();
                }
                return Ok(reseña);
            }
            else
            {
                // Obtener lista de reseñas con filtros
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


        // POST: api/Reseñar/Create
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(typeof(DetallesReseñarDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Create(CreacionesReseñarDTO reseñaForCreate)
        {
            // Validar lista de reseñas vacía
            if (reseñaForCreate.ReseñarItems == null || !reseñaForCreate.ReseñarItems.Any())
            {
                ModelState.AddModelError("", "Debes añadir al menos una reseña de coche.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Validar tipo de conductor estricto
            if (reseñaForCreate.TipoConductor != "Titular" && reseñaForCreate.TipoConductor != "Adicional")
            {
                ModelState.AddModelError("", "El tipo de conductor debe ser 'Titular' o 'Adicional'.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Validar usuario registrado
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == reseñaForCreate.Usuario);
            if (user == null)
            {
                ModelState.AddModelError("Usuario", "Error! Usuario no registrado");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Validar existencia de cada coche
            foreach (var itemDto in reseñaForCreate.ReseñarItems)
            {
                var coche = await _context.Coches.FindAsync(itemDto.CocheId);
                if (coche == null)
                {
                    ModelState.AddModelError("", $"Error! El coche {itemDto.CocheNombre} no existe.");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }
            }

            // Si todo es correcto, crear la reseña
            var reseña = new Reseñar
            {
                Usuario = reseñaForCreate.Usuario,
                Pais = reseñaForCreate.Pais,
                TipoConductor = reseñaForCreate.TipoConductor,
                Creado = reseñaForCreate.Creado,
                ApplicationUser = user,
                ReseñarItems = new System.Collections.Generic.List<ReseñarItem>()
            };

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

            _context.Reseñas.Add(reseña);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Conflict("Error al guardar la reseña: " + ex.Message);
            }

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

            return CreatedAtAction(nameof(GetDetails), new { id = reseña.Id }, detallesDTO);
        }
    }
}
