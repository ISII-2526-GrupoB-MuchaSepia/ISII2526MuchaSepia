using AppForSEII2526.API.DTOs.CocheDTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.UT.CocheController_test
{
    public class Get_Coches_Alquiler_test : AppForSEII25264SqliteUT
    {
        public Get_Coches_Alquiler_test()
        {
            var modelos = new List<Modelo>
            {
                new Modelo("Toyota Sedán"),
                new Modelo("Tesla SUV"),
            };

            var coches = new List<Coche>
            {
                new Coche(claseCoche: "SUV",
                color: "Negro",
                descripcion: "SUV compacto ideal para ciudad",
                desplazamientoMotor: "1.6L",
                tipoCombustible: "Gasolina",
                fabricante: "Nissan",
                precioCompra: 18000m,
                cantidadCompra: 6,
                cantidadAlquiler: 3,
                precioAlquiler: 65,
                tamanoLlanta: "18 pulgadas",
                modelo: modelos[0],
                tiposdeMantenimiento: Coche.TipoMantenimiento.Aceite
            ),

                new Coche(
                claseCoche: "SUV Híbrido",
                color: "Gris Plata",
                descripcion: "SUV híbrido eficiente y moderno",
                desplazamientoMotor: "2.0 Híbrido",
                tipoCombustible: "Híbrido",
                fabricante: "Hyundai",
                precioCompra: 22000m,
                cantidadCompra: 4,
                cantidadAlquiler: 2,
                precioAlquiler: 75,
                tamanoLlanta: "19 pulgadas",
                modelo: modelos[1],
                tiposdeMantenimiento: Coche.TipoMantenimiento.Frenos
            )
            };

            ApplicationUser usuario= new ApplicationUser(id: "10",
           nombre: "Clara",
           apellido: "Lopez",
           nombreUsuario: "Clara@lopez",
           direccion: "Calle Mayor 12, Toledo");

            var alquiler = new Alquiler(
                 nombre: usuario.Nombre,
                 apellido: usuario.Apellido,
                 concesionarioEntrega: "Granada",
                 fechaAlquiler: DateTime.Today.AddDays(1),
                 metodoPago: MetodoPagoTipos.GooglePay,
                 inicioAlquiler: DateTime.Today,
                 finAlquiler: DateTime.Today.AddDays(7),
                 alquilerItems: new List<AlquilerItem>(),
                 applicationUser: usuario
             );

            // ALQUILER ITEM
            var alquilerItem = new AlquilerItem(
                coche: coches[0],      // Toyota Sedán Rojo y precio 65
                alquiler: alquiler,
                cantidad: 3
            );

            alquiler.AlquilerItems.Add(alquilerItem);

            _context.Add(usuario);
            _context.AddRange(modelos);
            _context.AddRange(coches);
            _context.Add(alquiler);
            _context.SaveChanges();


        }


        public static IEnumerable<object[]> TestGetCochesAlquiler_OK()
        {
            var cocheDTOs = new List<CocheParaAlquilerDTO>()
            {
                new CocheParaAlquilerDTO(1, "Toyota Corolla", "Rojo", 45.99, "Gasolina", "Toyota"),
                new CocheParaAlquilerDTO(2, "Tesla Model 3", "Blanco", 89.50, "Eléctrico", "Tesla"),
                new CocheParaAlquilerDTO(3, "Volkswagen Golf", "Azul", 50.00, "Diésel", "Volkswagen"),
                new CocheParaAlquilerDTO(4, "Ford Fiesta", "Negro", 40.75, "Gasolina", "Ford"),
               
            };

            var cocheDTOsTest1= new List<CocheParaAlquilerDTO>() { cocheDTOs[0], cocheDTOs[1], cocheDTOs[2] , cocheDTOs[3] };
            var cocheDTOsTest2 = new List<CocheParaAlquilerDTO>() { cocheDTOs[0], cocheDTOs[3] };
            var cocheDTOsTest3 = new List<CocheParaAlquilerDTO>() {  cocheDTOs[2] };
            var cocheDTOsTest4 = new List<CocheParaAlquilerDTO>() { cocheDTOs[1] };

            var test = new List<object[]>
            {
                new object[] { null, null, cocheDTOsTest1 },
                new object[] { "Volkswagen Golf", null, cocheDTOsTest2 },
                new object[] { null, "89.50", cocheDTOsTest3 },
                new object[] { "Tesla Model 3", "89.50", cocheDTOsTest4 },

            };

            return test;
        }

        [Theory]
        [MemberData(nameof(TestGetCochesAlquiler_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCochesAlquiler_Ok_test(string? modelo, double? precioAlquiler, List<CocheParaAlquilerDTO> expected)
        {
            // Arrange
            var mock = new Mock<ILogger<CocheController>>();
            ILogger<CocheController> logger = mock.Object;
            var controller = new CocheController(_context, logger);

            // Act
            var result = await controller.GetCochesParaAlquilar(modelo, precioAlquiler);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var cochesActual = Assert.IsType<List<CocheParaAlquilerDTO>>(okResult.Value);
            Assert.Equal(expected, cochesActual);
        }

    }
    }
