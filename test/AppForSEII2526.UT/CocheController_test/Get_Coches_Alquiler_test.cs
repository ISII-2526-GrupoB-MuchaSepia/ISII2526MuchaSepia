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
                new Modelo("Toyota Supra"),
                new Modelo("Honda Civic Type R"),
                new Modelo("BMW M3"),
                new Modelo("Lamborghini Aventador")
            };

            var coches = new List<Coche>
            {
                new Coche(
    claseCoche: "Deportivo",
    color: "Rojo",
    descripcion: "Coupé de alto rendimiento",
    desplazamientoMotor: "3.0L Twin-Turbo",
    tipoCombustible: "Gasolina",
    fabricante: "Toyota",
    precioCompra: 55000d,
    cantidadCompra: 4,
    cantidadAlquiler: 2,
    precioAlquiler: 190,
    tamanoLlanta: "19 pulgadas",
    modelo: modelos[0],
    tiposdeMantenimiento: Coche.TipoMantenimiento.Aceite
),

new Coche(
    claseCoche: "Compacto",
    color: "Azul",
    descripcion: "Hatchback deportivo para uso diario",
    desplazamientoMotor: "2.0L Turbo",
    tipoCombustible: "Gasolina",
    fabricante: "Honda",
    precioCompra: 23000d,
    cantidadCompra: 8,
    cantidadAlquiler: 4,
    precioAlquiler: 55,
    tamanoLlanta: "17 pulgadas",
    modelo: modelos[1],
    tiposdeMantenimiento: Coche.TipoMantenimiento.Aceite
),

new Coche(
    claseCoche: "Sedán",
    color: "Gris",
    descripcion: "Sedán premium cómodo y potente",
    desplazamientoMotor: "3.0L",
    tipoCombustible: "Gasolina",
    fabricante: "BMW",
    precioCompra: 60000d,
    cantidadCompra: 3,
    cantidadAlquiler: 1,
    precioAlquiler: 120,
    tamanoLlanta: "19 pulgadas",
    modelo: modelos[2],
    tiposdeMantenimiento: Coche.TipoMantenimiento.Aceite
),

new Coche(
    claseCoche: "Superdeportivo",
    color: "Amarillo",
    descripcion: "Supercar de altas prestaciones",
    desplazamientoMotor: "6.5L V12",
    tipoCombustible: "Gasolina",
    fabricante: "Lamborghini",
    precioCompra: 350000d,
    cantidadCompra: 1,
    cantidadAlquiler: 1,
    precioAlquiler: 900,
    tamanoLlanta: "21 pulgadas",
    modelo: modelos[3],
    tiposdeMantenimiento: Coche.TipoMantenimiento.Aceite
)

            };

            ApplicationUser usuario= new ApplicationUser(id: "1",
           nombre: "Clara",
           apellido: "Lopez",
           nombreUsuario: "Clara@lopez",
           direccion: "Calle Mayor 12, Toledo");

            var alquiler = new Alquiler(
                 
                 concesionarioEntrega: "Granada Central",
                 fechaAlquiler: DateTime.Today,
                 metodoPago: MetodoPagoTipos.GooglePay,
                 inicioAlquiler: DateTime.Today.AddDays(1),
                 finAlquiler: DateTime.Today.AddDays(7),
                 alquilerItems: new List<AlquilerItem>(),
                 applicationUser: usuario
             );

            // ALQUILER ITEM
            var alquilerItem = new AlquilerItem(
                1,alquiler,1);

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
              new CocheParaAlquilerDTO(1, "Toyota Supra", "Rojo", 190, "Gasolina", "Toyota"),
new CocheParaAlquilerDTO(2, "Honda Civic Type R", "Azul", 55, "Gasolina", "Honda"),
new CocheParaAlquilerDTO(3, "BMW M3", "Gris", 120, "Gasolina", "BMW"),
new CocheParaAlquilerDTO(4, "Lamborghini Aventador", "Amarillo", 900, "Gasolina", "Lamborghini")


            };

            var cocheDTOsTest1= new List<CocheParaAlquilerDTO>() { cocheDTOs[0], cocheDTOs[1], cocheDTOs[2] , cocheDTOs[3] }; //sin filtros
            var cocheDTOsTest2 = new List<CocheParaAlquilerDTO>() {  cocheDTOs[2] }; //filtro por modelo
            var cocheDTOsTest3 = new List<CocheParaAlquilerDTO>() { cocheDTOs[0], cocheDTOs[1], cocheDTOs[2] }; //filtro por precio alquiler
            var cocheDTOsTest4 = new List<CocheParaAlquilerDTO>() { cocheDTOs[1] };

            var test = new List<object[]>
            {
                new object[] { null, null, cocheDTOsTest1 }, //sin filtros
                new object[] { "BMW M3", null, cocheDTOsTest2 }, //filtro por modelo
                new object[] { null, 200.0, cocheDTOsTest3 }, //filtro por precio alquiler (<1000)
                new object[] { "Honda Civic Type R", 100.0, cocheDTOsTest4 }, //filtro por modelo y precio alquiler (<100)

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
