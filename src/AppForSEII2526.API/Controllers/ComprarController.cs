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

        //   GET: Detalles de compra
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DetallesCompraDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetDetallesCompra(int id)
        {
            // Comprueba que la tabla Compras exista en el contexto
            if (_context.Compras == null)
            {
                _logger.LogError("Error: La tabla de Compras no existe");
                return NotFound();
            }
            // Consulta la compra con el id indicado
            var compra = await _context.Compras
             .Where(p => p.Id == id) // Filtro por id de compra
             .Include(p => p.ApplicationUser) //join table ApplicationUser  // Incluye datos del usuario que realizó la compra
             .Include(p => p.ComprarItems) //join table PurchaseItem  // Incluye las líneas de compra
             .ThenInclude(pi => pi.Coche) //then join table Car // Incluye el coche de cada línea
             .ThenInclude(c => c.Modelo) //then join table Model   // Incluye el modelo del coche
             .Select(p => new DetallesCompraDTO(p.Id,p.ApplicationUser.Nombre, p.ApplicationUser.Apellido, p.FechaCompra, p.ConcesionarioEntrega, p.PrecioCompra, p.ComprarItems
             .Select(pi => new ComprarItemDTO(pi.Coche.Id, pi.Coche.Modelo.Name, pi.Coche.PrecioCompra, pi.Cantidad, pi.Coche.Color)).ToList<ComprarItemDTO>())) // Convierte a lista de DTOs
             .FirstOrDefaultAsync();// Toma el primero o null si no hay

            // Si no se encontró ninguna compra con ese id
            if (compra == null)
            {
                _logger.LogError($"Error: Compra con id :{id} no existe");
                return NotFound();
            }


            return Ok(compra);// Devuelve la compra encontrada detalle
        }

        //   POST: Crear una compra

        [HttpPost] 
        [Route("[action]")] 
        [ProducesResponseType(typeof(DetallesCompraDTO), (int)HttpStatusCode.Created)] 
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)] 
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)] 
        public async Task<ActionResult> CrearCompra(CreacionComprasDTO creacionCompras)
        {
            // 1. Validaciones iniciales de la petición

            if (creacionCompras.ComprarItems.Count == 0) //compruebo que he almenos hay  algún coche para comprar.
            {
                ModelState.AddModelError("CompraItems", "Error! Debes incluir un coche al menos para comprar");
            }
            foreach (var it in creacionCompras.ComprarItems)
            {
                if (it.Cantidad == 2)
                {
                    ModelState.AddModelError("Descripcion y cant", "Error! Descripcion nula o  cantidad = 2");
                }
            }

            var usuario = _context.ApplicationUsers.FirstOrDefault(au => au.Nombre == creacionCompras.Nombre); //compruebo que el usuario  existe en la base de datos
            if (usuario == null)
            {
                ModelState.AddModelError("PurchaseApplicationUser", "Error! Usuario no registrado");
            }

            if (ModelState.ErrorCount > 0)  // Si hay errores de validación hasta aquí, devuelve BadRequest
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // 2. Obtiene la lista de modelos que se quieren comprar

            var purchaseCars = creacionCompras.ComprarItems.Select(pi => pi.Modelo).ToList<string>();// Se asume que 'Modelo' es el nombre del modelo del coche

            // 3. Recupera de la base de datos los coches que coinciden con esos modelos

            var coches = _context.Coches.Include(c => c.ComprarItems)  // Incluye las compras anteriores de ese coche
                .ThenInclude(pi => pi.Comprar) 
                .Include(c => c.Modelo)   // Incluye el modelo del coche
                .Where(c => purchaseCars.Contains(c.Modelo.Name)) // Filtra solo los modelos que se quieren comprar
                .ToList();


            // 4. Crea la entidad Comprar (compra principal)

            Comprar comprar = new Comprar(creacionCompras.Nombre, creacionCompras.Apellido, usuario, creacionCompras.ConcesionarioEntrega, DateTime.Today, new List<ComprarItem>(), (Comprar.MetodoPagoTipos)creacionCompras.MetodoPago);

            // Inicializa el precio total a 0
            comprar.PrecioCompra = 0;

            // 5. Recorre cada item que el usuario quiere comprar
            foreach (var item in creacionCompras.ComprarItems)
            {
                // Busca el coche por nombre de modelo
                var coche = coches.FirstOrDefault(c => c.Modelo.Name == item.Modelo);

                // Si no existe ese coche
                if (coche == null)
                {
                    ModelState.AddModelError("ComprarItems", $"Error: ¡El coche '{item.Modelo}' no está disponible para la compra!");
                }
                // Si se intenta comprar más unidades de las disponibles
                else if (coche.ComprarItems.Sum(ci => ci.Cantidad) + item.Cantidad > coche.CantidadCompra)  // Suma todas las unidades que ya se han comprado anteriormente de este coche
                                              // Le suma la cantidad que el usuario quiere comprar ahora
                                             // Compara si el total supera el stock disponible (CantidadCompra)
                {
                    ModelState.AddModelError("ComprarItems", $"Error: ¡No hay suficientes unidades disponibles del coche '{item.Modelo}'!");
                }
                else
                {
                    // Si todo ok, crea un nuevo ComprarItem y lo añade a la compra
                    comprar.ComprarItems.Add(new ComprarItem(coche, comprar, item.Cantidad));
                    item.PrecioCompra = coche.PrecioCompra; // Asigna el precio de compra del coche al item del DTO
                    comprar.PrecioCompra += (coche.PrecioCompra * item.Cantidad); // Actualiza el precio total de la compra (precio coche * cantidad)
                }
            }
            // Si se han detectado errores durante el bucle (coches no disponibles, sin stock, etc.)
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            // 6. Añade la compra al contexto para guardarla en la base de datos
            _context.Add(comprar);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {// Si hay un error al guardar, lo registra y devuelve Conflict
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Compra", $"Error: Hubo un problema al guardar la compra. Inténtalo nuevamente más tarde.");
                return Conflict("Error: " + ex.Message);
            }


            // 7. Construye el DTO de respuesta con los datos finales de la compra
            var detallesCompra = new DetallesCompraDTO(
                comprar.Id,
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