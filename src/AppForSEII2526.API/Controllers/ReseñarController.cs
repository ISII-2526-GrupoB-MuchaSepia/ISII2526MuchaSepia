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
using System.Collections.Generic;

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

        // METODO GET
        [HttpGet("GetDetails")]
        [ProducesResponseType(typeof(DetallesReseñarDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetDetails(int id)
        {
            var reseña = await _context.Reseñas
                .Where(r => r.Id == id)
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
                _logger.LogError($"Error: Reseña con id {id} no existe.");
                return NotFound();
            }

            return Ok(reseña);
        }


        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(typeof(DetallesReseñarDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Create(CreacionesReseñarDTO reseñaForCreate)
        {
            if (reseñaForCreate.ReseñarItems == null || !reseñaForCreate.ReseñarItems.Any())
            {
                ModelState.AddModelError("", "Debes añadir al menos una reseña de coche.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            if (reseñaForCreate.TipoConductor != "Titular" && reseñaForCreate.TipoConductor != "Adicional")
            {
                ModelState.AddModelError("", "El tipo de conductor debe ser 'Titular' o 'Adicional'.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == reseñaForCreate.Usuario);
            if (user == null)
            {
                ModelState.AddModelError("Usuario", "Error! Usuario no registrado.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            foreach (var itemDto in reseñaForCreate.ReseñarItems)
            {
                var coche = await _context.Coches.FindAsync(itemDto.CocheId);
                if (coche == null)
                {
                    ModelState.AddModelError("", $"Error! El coche {itemDto.CocheNombre} no existe.");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }
            }

            // EJERCICIOS EXAMEN
            foreach (var examen in reseñaForCreate.ReseñarItems)
            {
                if (!String.IsNullOrEmpty(examen.Descripcion) && !examen.Descripcion.StartsWith("Reseña para"))
                {
                    ModelState.AddModelError("", "¡Error! La reseña debe empezar por Reseña para");
                    // return BadRequest(new ValidationProblemDetails(ModelState)); // <--- ESTO LO TENÍAS COMENTADO, BIEN.
                }
            }

         
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
        

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

            return StatusCode(StatusCodes.Status201Created, detallesDTO);
        }
    }
}