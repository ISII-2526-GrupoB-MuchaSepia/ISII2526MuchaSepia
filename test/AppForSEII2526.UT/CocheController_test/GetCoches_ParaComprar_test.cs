using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.CocheDTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.CocheController_test
{
    // Clase de pruebas del CocheController.
    // Hereda de AppForSEII25264SqliteUT, que probablemente configura un DbContext SQLite en memoria para los tests.
    public class GetCoches_ParaComprar_test : AppForSEII25264SqliteUT
    {
        // Constructor de la clase de test.
        // Se ejecuta ANTES de cada test y prepara datos en la BDD de pruebas.
        public GetCoches_ParaComprar_test()
        {
            // Creamos una lista de modelos (entidad Modelo).
            var modelos = new List<Modelo>()
            {
                new Modelo ("Citroen Berlingo"),
                new Modelo ("Fiat Stilo"),
                new Modelo ("Fiat Talento"),
                new Modelo ("Mazda cx-5")
            };

            // Creamos una lista de coches (entidad Coche) asociándolos a los modelos anteriores.
            var coches = new List<Coche>
            {
                // Coche 1
                new Coche("Derivado",              // ClaseCoche
                          "Blanco",               // Color
                          "Una leyenda.",         // Descripción
                          "1.6L",                 // Motor
                          "Diesel",               // TipoCombustible
                          "Citroen",              // Fabricante
                          12000d,                 // PrecioCompra
                          2,                      // CantidadCompra
                          3,                      // CantidadVenta (por ejemplo)
                          40f,                    // Consumo
                          "16",                   // Llanta
                          modelos[0],             // Modelo asociado (Citroen Berlingo)
                          Coche.TipoMantenimiento.Aceite),

                // Coche 2
                new Coche("Ranchera",
                          "Blanco",
                          "Literalmente un coche funebre.",
                          "1.9L",
                          "Diesel",
                          "Fiat",
                          11000d,
                          1,
                          4,
                          60f,
                          "18",
                          modelos[1],             // Fiat Stilo
                          Coche.TipoMantenimiento.Refrigeracion),

                // Coche 3
                new Coche("Furgoneta",
                          "Rojo",
                          "Parece una limusina.",
                          "1.6L",
                          "Diesel",
                          "Fiat",
                          18000d,
                          4,
                          2,
                          190f,
                          "16",
                          modelos[2],             // Fiat Talento
                          Coche.TipoMantenimiento.Transmision),

                // Coche 4
                new Coche("SUV",
                          "Rojo",
                          "Coche bien lujoso.",
                          "2.0L",
                          "Gasolina",
                          "Mazda",
                          27000d,
                          6,
                          3,
                          110f,
                          "19",
                          modelos[3],             // Mazda cx-5
                          Coche.TipoMantenimiento.Suspension)
            };

            // Creamos un usuario de la aplicación (ApplicationUser).
            ApplicationUser user = new ApplicationUser(
                "1",
                "BARTOLO",
                "Balatro Balatrez",
                "balatro.bbez ",
                "balatro.bbez@ucl.es"
            );

            // Creamos una compra (Comprar) asociada al usuario.
            var compra = new Comprar(
                1,                               // Id compra
                "BARTOLO",                       // Nombre cliente
                "Balatro Balatrez",              // Apellidos cliente
                user,                            // Usuario asociado
                "Concesionario Albacete",        // Lugar de compra
                DateTime.Today,                  // Fecha
                new List<ComprarItem>(),         // Lista inicial vacía de items
                Comprar.MetodoPagoTipos.Visa     // Método de pago
            );

            // Creamos un item de compra asociando el coche 2 a esa compra.
            var comprarItems = new ComprarItem(coches[1], compra, 2);
            // Añadimos el item a la colección de la compra.
            compra.ComprarItems.Add(comprarItems);

            // Añadimos todas las entidades al DbContext para que se guarden en la BDD de pruebas.
            _context.Add(user);           // usuario
            _context.AddRange(modelos);   // modelos
            _context.AddRange(coches);    // coches
            _context.Add(compra);         // compra
            _context.Add(comprarItems);   // línea de compra

            // Persistimos los cambios en la base de datos SQLite en memoria.
            _context.SaveChanges();
        }

        // Método estático que genera los casos de prueba para el [Theory].
        // Devuelve una colección de object[] donde cada elemento representa:
        //  - parámetros de entrada del método
        //  - y la lista de coches esperada.
        public static IEnumerable<object[]> TestCasesFor_GetCarsForPurchase_OK()
        {
            // Creamos los DTOs que representan el resultado esperado del método del controlador.
            var cocheDTOs = new List<CocheParaCompraDTO>() {
                new CocheParaCompraDTO(1,"Citroen Berlingo",12000d,"Blanco","Diesel","Citroen"),
                new CocheParaCompraDTO(2,"Fiat Stilo",11000d,"Blanco","Diesel","Fiat"),
                new CocheParaCompraDTO(3,"Fiat Talento",18000d,"Rojo","Diesel","Fiat"),
                new CocheParaCompraDTO(4,"Mazda cx-5",27000d,"Rojo","Gasolina","Mazda"),
            };

            // Caso 1: sin filtros → esperamos todos los coches.
            var cocheDTOsTest1 = new List<CocheParaCompraDTO>()
            {
                cocheDTOs[0], cocheDTOs[1], cocheDTOs[2], cocheDTOs[3]
            };

            // Caso 2: filtramos solo por color "Blanco" → los coches blancos (ID 1 y 2).
            var cocheDTOsTest2 = new List<CocheParaCompraDTO>()
            {
                cocheDTOs[0], cocheDTOs[1]
            };

            // Caso 3: filtramos solo por fabricante "Fiat" → los coches Fiat (ID 2 y 3).
            var cocheDTOsTest3 = new List<CocheParaCompraDTO>()
            {
                cocheDTOs[1], cocheDTOs[2]
            };

            // Caso 4: filtramos por color "Blanco" y fabricante "Citroen" → solo el Citroen blanco (ID 1).
            var cocheDTOsTest4 = new List<CocheParaCompraDTO>()
            {
                cocheDTOs[0]
            };

            // Empaquetamos todos los casos de prueba:
            //  - primer elemento: parámetro fcolor (color)
            //  - segundo: parámetro modelo (que en el controlador se usa como fabricante)
            //  - tercero: lista esperada de CocheParaCompraDTO
            var allTests = new List<object[]>
            {
                new object[] { null,    null,      cocheDTOsTest1 }, // sin filtros
                new object[] { "Blanco", null,     cocheDTOsTest2 }, // filtra por color
                new object[] { null,    "Fiat",    cocheDTOsTest3 }, // filtra por fabricante
                new object[] { "Blanco","Citroen", cocheDTOsTest4 }, // filtra por color y fabricante
            };

            return allTests;
        }

        // Test parametrizado con xUnit [Theory].
        // Va a ejecutar este método 4 veces, una por cada caso de TestCasesFor_GetCarsForPurchase_OK.
        [Theory]
        [MemberData(nameof(TestCasesFor_GetCarsForPurchase_OK))]  // de aquí saca los parámetros
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCarsForPurchase_OK_test(string? fcolor, string? modelo, IList<CocheParaCompraDTO> expectedCars)
        {
            // Arrange (preparación)

            // Creamos un mock del logger para inyectarlo al controlador.
            var mock = new Mock<ILogger<CocheController>>();
            ILogger<CocheController> logger = mock.Object;

            // Creamos una instancia del CocheController usando el DbContext de pruebas (_context)
            // que viene de la clase base AppForSEII25264SqliteUT.
            var controller = new CocheController(_context, logger);

            // Act (ejecución)
            // Llamamos al método del controlador que queremos probar,
            // pasando los parámetros fcolor y modelo que vienen del caso de prueba.
            var result = await controller.GetCochesParaComprar(fcolor, modelo);

            // Assert (comprobación)

            // 1. Comprobamos que la acción devuelve un resultado de tipo OkObjectResult (HTTP 200 OK).
            var okResult = Assert.IsType<OkObjectResult>(result);

            // 2. Obtenemos el Value del OkObjectResult y comprobamos que es una lista de CocheParaCompraDTO.
            var carDTOsActual = Assert.IsType<List<CocheParaCompraDTO>>(okResult.Value);

            // 3. Comparamos la lista real (carDTOsActual) con la lista esperada (expectedCars).
            //    La igualdad usa el Equals sobreescrito en CocheParaCompraDTO.
            Assert.Equal(expectedCars, carDTOsActual);
        }
    }
}
