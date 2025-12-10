using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ComprarDTOs;

namespace AppForSEII2526.UT.ComprarController_Test
{
    public class Crear_Comprar_test : AppForSEII25264SqliteUT
    {
        private const string UserName = "Balabbez";
        private const string Nombre = "Bartolo";
        private const string Apellidos = "Balatro Balatre";
        private const string Direccion = "balatorbbez@gmail.com";
        private const string ConcesionarioEntrega = "Concesionario Central de Toledo";


        private const string modelo1 = "Honda Compacto";
        private const string modelo2 = "Ford Pickup";

        public Crear_Comprar_test()
        {
            var models = new List<Modelo> //me creo modelos de coche de prueba
            {
                new Modelo(modelo1),
                new Modelo(modelo2)
            };

            var coches = new List<Coche> //me creo filas de una tabla coche de prueba
            {
                new Coche("Compacto", "Azul", "Compacto urbano eficiente", "2.0L", "Gasolina","Honda",15000d,10,5,45,"15", models[0],Coche.TipoMantenimiento.Aceite),
                new Coche("Pickup", "Blanco", "Pickup robusta para trabajo pesado","1.8L", "Diesel","Ford",42000d,5,3,90,"20", models[1],Coche.TipoMantenimiento.Transmision),
            };

            ApplicationUser user = new ApplicationUser(
                "1",                      // Id
                Nombre,               // Nombre
                Apellidos,      // Apellidos
                UserName,              // Nombre de usuario
                Direccion   // Email
            );

            var compra = new Comprar(1, Nombre, Apellidos, user, ConcesionarioEntrega, DateTime.Today, new List<ComprarItem>(), Comprar.MetodoPagoTipos.Visa);
            var comprarItems = new ComprarItem(coches[0], compra, 2);
            compra.ComprarItems.Add(comprarItems);

            _context.Add(user);
            _context.AddRange(models);
            _context.AddRange(coches);
            _context.Add(compra);
            _context.SaveChanges();
        }
        // Casos de prueba de entrada para Crear_Compra que deben producir errores de validación
        public static IEnumerable<object[]> TestCasesFor_CreatePurchases()
        {
            var comprarNOItem = new CreacionComprasDTO(0, Nombre, Apellidos, ConcesionarioEntrega, MetodoPagoTipos.PayPal, new List<ComprarItemDTO>());

            var comprarItems = new List<ComprarItemDTO>() { new ComprarItemDTO(1, modelo2, 0, 1, "Blanco", "Pickup robusta para trabajo pesado") };

            var comprarApplicationUser = new CreacionComprasDTO(0, "Mucha", "SEPIA", ConcesionarioEntrega, MetodoPagoTipos.PayPal, comprarItems);

            var comprarCocheNoExiste = new CreacionComprasDTO(0, Nombre, Apellidos, ConcesionarioEntrega, MetodoPagoTipos.PayPal, new List<ComprarItemDTO>() { new ComprarItemDTO(1, "Toyota Corola", 1, 0, "Rosa","coche rosa") });

            var comprarCocheNoDisponible = new CreacionComprasDTO(0, Nombre, Apellidos, ConcesionarioEntrega, MetodoPagoTipos.PayPal, new List<ComprarItemDTO>() { new ComprarItemDTO(1, modelo1, 1, 9, "Azul", "Compacto urbano eficiente") });// Pide más unidades de 'Honda Compacto' de las que hay disponibles

            var nodecrip = new CreacionComprasDTO(0, Nombre, Apellidos, ConcesionarioEntrega, MetodoPagoTipos.PayPal, new List<ComprarItemDTO>() { new ComprarItemDTO(1, modelo1, 1, 2, "Azul", "Compacto urbano eficiente") });// Pide más unidades de 'Honda Compacto' de las que hay disponibles


            var allTests = new List<object[]>
            {             // Casos de prueba para crear compra - se espera ERROR
               
                new object[] { comprarNOItem, "Error! Debes incluir un coche al menos para comprar", },
                new object[] { comprarApplicationUser, "Error! Usuario no registrado", },
                new object[] { comprarCocheNoExiste, "Error: ¡El coche 'Toyota Corola' no está disponible para la compra!", },
                new object[] { comprarCocheNoDisponible, "Error: ¡No hay suficientes unidades disponibles del coche 'Honda Compacto'!", },
                new object[] { nodecrip, "Error! Descripcion nula o  cantidad = 2", }
            };

            return allTests;

        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreatePurchases))]
        public async Task CreatePurchase_Error_test(CreacionComprasDTO compraDTO, string errorEsperado)
        {
            // Arrange
            var mock = new Mock<ILogger<ComprarController>>();
            ILogger<ComprarController> logger = mock.Object;

            var controller = new ComprarController(_context, logger);

            // Act
            var result = await controller.CrearCompra(compraDTO);

            //Assert
            // Comprobamos que la respuesta es BadRequest y obtenemos el error devuelto
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            // Comprobamos que el mensaje de error esperado y el real coinciden
            Assert.StartsWith(errorEsperado, errorActual); //compruebo si el error que sale es mi error esperado
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreatePurchase_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ComprarController>>();
            ILogger<ComprarController> logger = mock.Object;

            var controller = new ComprarController(_context, logger);

            var purchaseDTO = new CreacionComprasDTO(0, Nombre, Apellidos, ConcesionarioEntrega, MetodoPagoTipos.Visa, new List<ComprarItemDTO>() { new ComprarItemDTO(2, modelo2, 0d, 1, "Blanco", "Pickup robusta para trabajo pesado") });

            var expectedpurchaseDetailDTO = new DetallesCompraDTO(Nombre, Apellidos, DateTime.Today, ConcesionarioEntrega, 42000d, new List<ComprarItemDTO> { new ComprarItemDTO(2, modelo2, 42000d, 1, "Blanco", "Pickup robusta para trabajo pesado") });

            // Act
            var result = await controller.CrearCompra(purchaseDTO);

            //Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualPurchaseDetailDTO = Assert.IsType<DetallesCompraDTO>(createdResult.Value);

            Assert.Equal(expectedpurchaseDetailDTO, actualPurchaseDetailDTO);

        }
    }
}