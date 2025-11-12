using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.AlquilerDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.AlquilerController_Test
{
    public class PostAlquiler_test : AppForSEII25264SqliteUT
    {

        private const string NombreUsuario = "maria@correo.com";
        private const string Nombre = "María";
        private const string Apellidos = "López García";
        private const string ConcesionarioEntrega = "Concesionario Central de Toledo";

        // Nuevos modelos
        private const string modelo1 = "Nissan Qashqai";
        private const string modelo2 = "Hyundai Tucson";


        public PostAlquiler_test()
        {

            var modelos = new List<Modelo>
        {
            new Modelo(modelo1),
            new Modelo(modelo2)
        };

            var coches = new List<Coche>
        {
            new Coche(
                claseCoche: "SUV",
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

            var usuario = new ApplicationUser(
           id: "10",
           nombre: Nombre,
           apellido: Apellidos,
           nombreUsuario: NombreUsuario,
           direccion: "Calle Mayor 12, Toledo"
       )
            {
                UserName = NombreUsuario,
                EmailConfirmed = true
            };

            var alquiler = new Alquiler(
            nombre: Nombre,
            apellido: Apellidos,
            concesionarioEntrega: ConcesionarioEntrega,
            fechaAlquiler: DateTime.Today,
            metodoPago: MetodoPagoTipos.PayPal,
            inicioAlquiler: DateTime.Today.AddDays(1),
            finAlquiler: DateTime.Today.AddDays(7),
            alquilerItems: new List<AlquilerItem>(),
            applicationUser: usuario
        );

            var item = new AlquilerItem(coches[0], alquiler, cantidad: 1);
            alquiler.AlquilerItems.Add(item);

            _context.Add(usuario);
            _context.AddRange(modelos);
            _context.AddRange(coches);
            _context.Add(alquiler);
            _context.SaveChanges();


        }

        public static IEnumerable<object[]> TestCasesFor_CreateAlquiler()
        {
            var alquilerSinItems = new AlquilerParaCrearDTO(
        nombre: "Laura",
        apellido: "Gómez Hernández",
        concesionarioEntrega: "Sucursal Central Madrid",
        metodoPago: MetodoPagoTipos.GooglePay,
        inicioAlquiler: DateTime.Today.AddDays(3),
        finAlquiler: DateTime.Today.AddDays(6),
        alquilerItems: new List<AlquilerItemDTO>() // vacío
    );

            var itemsEjemplo = new List<AlquilerItemDTO>()
    {
        new AlquilerItemDTO(
            cocheId: 10,
            cantidad: 1,
            precioAlquiler: 0,
            modelo: "CualquierModelo",
            fabricante: "MarcaX"
        )
    };

            var inicioHoy = new AlquilerParaCrearDTO(
       nombre: "Laura",
       apellido: "Gómez Hernández",
       concesionarioEntrega: "Sucursal Central Madrid",
       metodoPago: MetodoPagoTipos.PayPal,
       inicioAlquiler: DateTime.Today, //
       finAlquiler: DateTime.Today.AddDays(5),
       alquilerItems: itemsEjemplo
   );
            var finAntesQueInicio = new AlquilerParaCrearDTO(
       nombre: "Laura",
       apellido: "Gómez Hernández",
       concesionarioEntrega: "Sucursal Norte",
       metodoPago: MetodoPagoTipos.GooglePay,
       inicioAlquiler: DateTime.Today.AddDays(7),
       finAlquiler: DateTime.Today.AddDays(3), // 
       alquilerItems: itemsEjemplo
   );
            var usuarioNoExiste = new AlquilerParaCrearDTO(
    nombre: "NombreInventado",
    apellido: "NoRegistrado",
    concesionarioEntrega: "Sucursal Este",
    metodoPago: MetodoPagoTipos.GooglePay,
    inicioAlquiler: DateTime.Today.AddDays(4),
    finAlquiler: DateTime.Today.AddDays(9),
    alquilerItems: itemsEjemplo
);
            var cocheNoDisponible = new AlquilerParaCrearDTO(
       nombre: "Laura",
       apellido: "Gómez Hernández",
       concesionarioEntrega: "Sucursal Sur",
       metodoPago: MetodoPagoTipos.GooglePay,
       inicioAlquiler: DateTime.Today.AddDays(2),
       finAlquiler: DateTime.Today.AddDays(6),
       alquilerItems: new List<AlquilerItemDTO>()
       {
            new AlquilerItemDTO(
                cocheId: 99,      
                cantidad: 1,
                precioAlquiler: 0,
                modelo: "NoExiste",
                fabricante: "NoExiste"
            )
       }
   );
            var allTests = new List<object[]>
            {
                 new object[] { alquilerSinItems, "Debe seleccionar al menos un coche para alquilar." },
        new object[] { inicioHoy, "¡Error! La fecha de inicio debe ser posterior a hoy." },
        new object[] { finAntesQueInicio, "¡Error! La fecha de finalización debe ser posterior a la de inicio." },
        new object[] { usuarioNoExiste, "El usuario indicado no existe." },
        new object[] { cocheNoDisponible, "no está disponible para las fechas" }
            };

            return allTests;


        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]

        [MemberData(nameof(TestCasesFor_CreateAlquiler))]
        public async Task CreateAlquiler_Error_Test(AlquilerParaCrearDTO alquilerDTO, string errorEsperado)
        {
            // Arrange
            var mock = new Mock<ILogger<AlquilerController>>();
            ILogger<AlquilerController> logger = mock.Object;

            var controller = new AlquilerController(_context, logger);

            // Act
            var result = await controller.CreateAlquiler(alquilerDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            Assert.StartsWith(errorEsperado, errorActual);


        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateAlquiler_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<AlquilerController>>();
            ILogger<AlquilerController> logger = mock.Object;

            var controller = new AlquilerController(_context, logger);

            DateTime inicio = DateTime.Today.AddDays(6);
            DateTime fin = DateTime.Today.AddDays(7);



            var alquilerDTO = new AlquilerParaCrearDTO(
        nombre: "Lucía",
        apellido: "García Torres",
        concesionarioEntrega: "Sucursal Central",
        metodoPago: MetodoPagoTipos.GooglePay,
        inicioAlquiler: inicio,
        finAlquiler: fin,
        alquilerItems: new List<AlquilerItemDTO>()
        {
            new AlquilerItemDTO(
                cocheId: 1,
                cantidad: 1,
                precioAlquiler: 0,
                modelo: "",
                fabricante: ""
            )
        }
    );


            var precioEsperado = 55 * (fin - inicio).Days;

            var expectedDetalle = new DetalleAlquilerDTO(
                id: 0, // no importa, el que devuelva EF
                fechaAlquiler: DateTime.Now,
                nombre: "Lucía",
                apellido: "García Torres",
                concesionarioEntrega: "Sucursal Central",
                inicioAlquiler: inicio,
                finAlquiler: fin,
                metodoPago: MetodoPagoTipos.GooglePay,
                alquilerItems: new List<AlquilerItemDTO>()
                {
            new AlquilerItemDTO(
                cocheId: 1,
                cantidad: 1,
                precioAlquiler: 55,
                modelo: "Volkswagen Golf",
                fabricante: "Volkswagen"
            )
                },
                total: precioEsperado
            );

            //ACT
            var result = await controller.CreateAlquiler(alquilerDTO);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var alquilerReal = Assert.IsType<DetalleAlquilerDTO>(createdResult.Value);


            Assert.Equal(expectedDetalle, alquilerReal);
        }
    }
}
