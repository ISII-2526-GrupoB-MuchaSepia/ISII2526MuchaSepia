using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ComprarDTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace AppForSEII2526.UT.ComprarController_Test
{
    // Clase de pruebas  para las compras
    public class Get_Comprar_test : AppForSEII25264SqliteUT
    {
        // Constructor de la clase de test.
        // Se ejecuta ANTES de cada test y deja preparada la base de datos de pruebas.
        public Get_Comprar_test()
        {
            // Creamos una lista de modelos de coche
            var modelo = new List<Modelo>() {
            new Modelo("Honda Compacto"),
            new Modelo("Ford Pickup"),
        };

            // Creamos una lista de coches con todos sus datos
            var coches = new List<Coche>
        {
            new Coche(
                "Compacto",
                "Azul",
                "Compacto urbano eficiente",
                "1.2L",
                "Gasolina",
                "Honda",
                15000d,
                10,
                5,
                45,
                "15",
                modelo[0],                               // Asociamos el primer modelo
                Coche.TipoMantenimiento.Aceite
            ),
            new Coche(
                "Pickup",
                "Blanco",
                "Pickup robusta para trabajo pesado",
                "3.5L",
                "Diésel",
                "Ford",
                42000d,
                5,
                3,
                90,
                "20",
                modelo[1],                               // Asociamos el segundo modelo
                Coche.TipoMantenimiento.Transmision
            )
        };

            // Creamos un usuario de la aplicación
            ApplicationUser user = new ApplicationUser(
                "1",                      // Id
                "Bartolo",               // Nombre
                "Balatro Balatrez",      // Apellidos
                "balabbez",              // Nombre de usuario
                "bartolo.bbez@uclm.es"   // Email
            );

            // Creamos una compra asociada al usuario
            var comprar = new Comprar(
                "Bartolo",                          // Nombre cliente
                "Balatro Balatrez",                          // Apellidos cliente
                user,                               // Usuario de la app
                "Concesionario Central de Toledo",       // Concesionario de entrega
                DateTime.Today,                     // Fecha compra
                new List<ComprarItem>(),            // Lista de líneas de compra (vacía al inicio)
                Comprar.MetodoPagoTipos.Visa      // Método de pago
            );

            // Creamos una línea de compra (ComprarItem) con el segundo coche
            var comprarItem = new ComprarItem(
                coches[1],   // Coche (Ford Pickup)
                comprar,     // Compra a la que pertenece
                1           // Cantidad (1 unidad)
            );
            // Añadimos la línea de compra a la colección de items de la compra
            comprar.ComprarItems.Add(comprarItem);

            foreach (var item in comprar.ComprarItems) 
            {
                comprar.PrecioCompra += item.Cantidad * item.Coche.PrecioCompra;

            }


          
            // Añadimos todos los objetos para que se guarden en la BD de pruebas
            _context.Add(user);
            _context.AddRange(modelo);
            _context.AddRange(coches);
            _context.Add(comprar);
            _context.Add(comprarItem);

            // Guardamos los cambios en la BD en memoria
            _context.SaveChanges();
        }

        //la compra NO existe 
        [Fact]                                                   
        [Trait("LevelTesting", "Unit Testing")]                  
        [Trait("Database", "WithoutFixture")]
        public async Task GetCompra_NotFound_test()
        {
            // Arrange: creamos un logger falso con Moq
            var mock = new Mock<ILogger<ComprarController>>();
            ILogger<ComprarController> logger = mock.Object;

            // Creamos el controlador que vamos a probar, usando el DbContext de pruebas
            var controller = new ComprarController(_context, logger);

            // Act: llamamos al endpoint de detalles de compra con un id inexistente (0)
            var result = await controller.GetDetallesCompra(0);

            // Assert: comprobamos que el resultado es NotFound (HTTP 404)
            Assert.IsType<NotFoundResult>(result);
        }

        //la compra SÍ exist 
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetCompra_Found_test()
        {
            // Arrange: igual que antes, creamos el logger falso y el controlador
            var mock = new Mock<ILogger<ComprarController>>();
            ILogger<ComprarController> logger = mock.Object;
            var controller = new ComprarController(_context, logger);

            // Creamos el DTO que representa la compra que ESPERAMOS recibir del controlador
            var compraEsperada = new DetallesCompraDTO(
                "Bartolo",                            // Nombre
                "Balatro Balatrez",                             // Apellido
                DateTime.Today,                       // Fecha de compra
                "Concesionario Central de Toledo",         // Concesionario / dirección de entrega
                0,                                    // PrecioTotal (se inicializa a 0, luego se calcula más abajo)
                new List<ComprarItemDTO>()            // Lista de items de compra
            );

            // Añadimos un item de compra al DTO esperado
            compraEsperada.ComprarItemDTOs.Add(
                new ComprarItemDTO(
                    2,                 // Id del coche 
                    "Ford Pickup",    // Nombre del coche
                    42000,            // Precio de compra
                    1,                 // Cantidad
                    "Blanco",           // Color
                    "Pickup robusta para trabajo pesado"
                )
            );
            foreach (var item in compraEsperada.ComprarItemDTOs)
            {
                compraEsperada.PrecioCompra += item.Cantidad * item.PrecioCompra;

            }
            // Act: llamamos al controlador para obtener los detalles de la compra con id 1
            var result = await controller.GetDetallesCompra(1);

            // Assert:
            // 1) Comprobamos que el resultado es un OkObjectResult (HTTP 200)
            var okResult = Assert.IsType<OkObjectResult>(result);

            // 2) Comprobamos que el contenido del OkObjectResult es un DetallesCompraDTO
            var compraDTOActual = Assert.IsType<DetallesCompraDTO>(okResult.Value);

            // 3) Comparamos el DTO esperado con el devuelto por el controlador.
            //    Esto usa el método Equals sobreescrito en DetallesCompraDTO.
            //var eq = compraEsperada.Equals(compraDTOActual);

            Assert.Equal(compraEsperada, compraDTOActual);
        }
    }
}