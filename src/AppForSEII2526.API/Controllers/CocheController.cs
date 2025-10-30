using AppForSEII2526.API.DTOs.CocheDTOs;
using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

    namespace AppForSEII2526.API.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class CochesController : ControllerBase
        {
            private readonly ApplicationDbContext _context;
            private readonly ILogger<CochesController> _logger;

            public CochesController(ApplicationDbContext context, ILogger<CochesController> logger)
            {
                this._context = context;
                this._logger = logger;
            }
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<CocheParaCompraDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCochesParaComprar( string? modeloCoche, string? fcolor)
        {
            IList<CocheParaCompraDTO> selectCoche = await _context.Coches

                .Include(m => m.Modelo)
                .Include(m => m.ComprarItems).ThenInclude(pi => pi.Comprar)

                .Where(m => m.CantidadCompra > 0 && (modeloCoche == null || m.Modelo.Name.Equals(modeloCoche))) 
                .OrderBy(m => m.Modelo)

                .Select(m => new CocheParaCompraDTO(m.Id, m.Modelo.Name, m.PrecioCompra, m.Color,m.Fabricante,m.TipoCombustible ))
                .ToListAsync();

            return Ok(selectCoche);
        }
    }
    }
