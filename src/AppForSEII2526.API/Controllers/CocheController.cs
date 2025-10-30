using AppForSEII2526.Shared.CocheDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AppForSEII2526.API.Data;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CocheController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CocheController> _logger;

        public CocheController(ApplicationDbContext context, ILogger<CocheController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<CocheParaReseñarDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCochesParaReseñar(string? claseCoche, string? color)
        {
            var query = _context.Coches
                .Include(c => c.Modelo)
                .Where(c =>
                    (claseCoche == null || c.ClaseCoche.Contains(claseCoche)) &&
                    (color == null || c.Color.Contains(color))
                )
                .OrderBy(c => c.ClaseCoche);

            var coches = await query
                .Select(c => new CocheParaReseñarDTO(
                    c.Id,
                    c.ClaseCoche,
                    c.Color,
                    c.Descripcion,
                    c.Modelo != null ? c.Modelo.Name : ""
                ))
                .ToListAsync();

            return Ok(coches);
        }
    }
}
