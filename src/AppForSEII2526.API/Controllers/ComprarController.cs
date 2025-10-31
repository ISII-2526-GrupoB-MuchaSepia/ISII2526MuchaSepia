using AppForSEII2526.API.DTOs.ComprarDTOs;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprarController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ComprarController> _logger;

        public ComprarController(ApplicationDbContext context, ILogger<ComprarController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DetallesCompraDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetDetallesCompra(int id)
        {
            if (_context.Compras == null)
            {
                _logger.LogError("Error: La tabla de Compras no existe");
                return NotFound();
            }

            var compra = await _context.Compras
             .Where(p => p.Id == id)
             .Include(p => p.ApplicationUser) //join table ApplicationUser
             .Include(p => p.ComprarItems) //join table PurchaseItem
             .ThenInclude(pi => pi.Coche) //then join table Car
             .ThenInclude(c => c.Modelo) //then join table Model
             .Select(p => new DetallesCompraDTO(p.ApplicationUser.Nombre, p.ApplicationUser.Apellido, p.FechaCompra, p.ConcesionarioEntrega, p.PrecioCompra, p.ComprarItems
             .Select(pi => new ComprarItemDTO(pi.Coche.Id, pi.Coche.Modelo.Name, pi.Coche.PrecioCompra, pi.Cantidad, pi.Coche.Color)).ToList<ComprarItemDTO>()))
             .FirstOrDefaultAsync();


            if (compra == null)
            {
                _logger.LogError($"Error: Compra con id :{id} no existe");
                return NotFound();
            }


            return Ok(compra);
        }

         }   
    }
