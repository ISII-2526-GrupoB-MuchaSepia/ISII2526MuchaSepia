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


        [HttpPost] //operación de creación
        [Route("[action]")] //operación de tipo acción
        [ProducesResponseType(typeof(DetallesCompraDTO), (int)HttpStatusCode.Created)] //devuelve OK cuando consigue meter en la base de datos el código
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)] //devuelve BadRequest cuando hay un error durante la comprobación de la petición
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)] //devuelve Conflict cuando hay un error al añadir a la base de datos
        public async Task<ActionResult> Create_Purchase(CreacionComprasDTO creacionCompras)
        {
            if (creacionCompras.ComprarItems.Count == 0) //compruebo que he seleccionado algún coche para comprar.
            {
                ModelState.AddModelError("CompraItems", "Error! Debes incluir un coche almenos para comprar");
            }

            var usuario = _context.ApplicationUsers.FirstOrDefault(au => au.Nombre == creacionCompras.Nombre); //compruebo que el usuario que compra existe en la base de datos
            if (usuario == null)
            {
                ModelState.AddModelError("PurchaseApplicationUser", "Error! Usuario no registrado");
            }

            if (ModelState.ErrorCount > 0) //si tengo algún error acumulado, devuelve BadRequest
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var purchaseCars = creacionCompras.ComprarItems.Select(pi => pi.Modelo).ToList<string>();

            var coches = _context.Coches.Include(c => c.ComprarItems)
                .ThenInclude(pi => pi.Comprar)
                .Include(c => c.Modelo)
                .Where(c => purchaseCars.Contains(c.Modelo.Name))
                .ToList();
                
                

            Comprar comprar = new Comprar(creacionCompras.Nombre, creacionCompras.Apellido, usuario, creacionCompras.ConcesionarioEntrega, DateTime.Now, new List<ComprarItem>(), (Comprar.MetodoPagoTipos)creacionCompras.MetodoPago);

            comprar.PrecioCompra = 0;

            foreach (var item in creacionCompras.ComprarItems)
            {
                // Busca el coche por nombre de modelo
                var coche = coches.FirstOrDefault(c => c.Id == item.CocheID);

                if (coche == null)
                {
                    ModelState.AddModelError("ComprarItems", $"Error: ¡El coche '{item.Modelo}' no está en venta por ese concesionario!");
                }
                else if ((coche.CantidadCompra + item.Cantidad) > coche.CantidadCompra)
                {
                    ModelState.AddModelError("ComprarItems", $"Error: ¡No hay suficientes unidades disponibles del coche '{item.Modelo}'!");
                }
                else
                {

                    comprar.ComprarItems.Add(new ComprarItem(coche, comprar, (decimal)item.Cantidad));// ERRORES NO SE QUE PASA
                    item.PrecioCompra = coche.PrecioCompra;
                    comprar.PrecioCompra += (coche.PrecioCompra * item.Cantidad);
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(comprar);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Compra", $"Error: Hubo un problema al guardar la compra. Inténtalo nuevamente más tarde.");
                return Conflict("Error: " + ex.Message);
            }

            // DTO de respuesta (ajústalo si tu clase se llama distinto)
            var detallesCompra = new DetallesCompraDTO(
                comprar.ApplicationUser.Nombre,
                comprar.ApplicationUser.Apellido,
                comprar.FechaCompra,
                comprar.ConcesionarioEntrega,
                comprar.PrecioCompra,
                creacionCompras.ComprarItems
            );

            return CreatedAtAction("GetDetallesCompra", new { id = comprar.Id }, detallesCompra);

        }
    }
}
