using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.AlquilerDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.AlquilerController_Test
{
    public class Crear_Alquiler_test : AppForSEII25264SqliteUT
    {

        private const string NombreUsuario = "lucas.mdn";
        private const string Nombre = "Lucas";
        private const string Apellido = "Maldonado";
        private const string ConcesionarioEntrega = "Calle Mayor 12, Toledo";

        // Nuevos modelos
        private const string modelo1 = "Toyota Supra";
        private const string modelo2 = "Honda Civic Type R";


        public Crear_Alquiler_test()
        {

            var modelos = new List<Modelo>
        {
            new Modelo(modelo1),
            new Modelo(modelo2)
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
)
        };

            //USUARIO
            ApplicationUser usuario = new ApplicationUser(
     "1",
     Nombre,
     Apellido,
     NombreUsuario,
    ConcesionarioEntrega
 );

            //ALQUILER
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
                1, alquiler, 1);

            alquiler.AlquilerItems.Add(alquilerItem);


            _context.Add(usuario);
            _context.AddRange(modelos);
            _context.AddRange(coches);
            _context.Add(alquiler);
            _context.SaveChanges();


        }

        public static IEnumerable<object[]> TestCasesFor_CreateAlquiler()
        {
            var alquilerSinItem = new AlquilerParaCrearDTO(
    NombreUsuario,
    Nombre,
    Apellido,
    ConcesionarioEntrega,
    metodoPago: MetodoPagoTipos.Visa,
    inicioAlquiler: DateTime.Today.AddDays(2),
    finAlquiler: DateTime.Today.AddDays(7),
    alquilerItems: new List<AlquilerItemDTO>(),
    fechaAlquiler: DateTime.Today.AddDays(3),
    total: 0
);


            var itemsAlquiler = new List<AlquilerItemDTO>()
    {
        new AlquilerItemDTO(
           
            cantidad: 1,
            precioAlquiler: 0,
            modelo: modelo2,
            fabricante: "Honda"
        )
    };

            var inicioHoy = new AlquilerParaCrearDTO(
                NombreUsuario,
    Nombre,
    Apellido,
    ConcesionarioEntrega,
    metodoPago: MetodoPagoTipos.Visa,
    inicioAlquiler: DateTime.Today,
    finAlquiler: DateTime.Today.AddDays(5),
    alquilerItems: itemsAlquiler,
    fechaAlquiler: DateTime.Today.AddDays(1),
    total: 0

   );
            var finAntesQueInicio = new AlquilerParaCrearDTO(
    NombreUsuario,
    Nombre,
    Apellido,
    ConcesionarioEntrega,
    metodoPago: MetodoPagoTipos.Visa,
    inicioAlquiler: DateTime.Today.AddDays(5),
    finAlquiler: DateTime.Today.AddDays(2),
    alquilerItems: itemsAlquiler,
    fechaAlquiler: DateTime.Today.AddDays(1),
    total: 0
   );
            var usuarioNoExiste = new AlquilerParaCrearDTO(
    "margarita@21",
    Nombre,
    Apellido,
    ConcesionarioEntrega,
    metodoPago: MetodoPagoTipos.Visa,
    inicioAlquiler: DateTime.Today.AddDays(2),
    finAlquiler: DateTime.Today.AddDays(3),
    alquilerItems: itemsAlquiler,
    fechaAlquiler: DateTime.Today.AddDays(4),
    total: 0
);
            var cocheNoDisponible = new AlquilerParaCrearDTO(
     NombreUsuario,
     Nombre,
     Apellido,
     ConcesionarioEntrega,
     metodoPago: MetodoPagoTipos.Visa,
     inicioAlquiler: DateTime.Today.AddDays(2),
     finAlquiler: DateTime.Today.AddDays(5),
     alquilerItems: new List<AlquilerItemDTO>()
     {
        new AlquilerItemDTO(
            cantidad: 1,
            precioAlquiler: 0,
            modelo: modelo1,
            fabricante: "Toyota"
        )
     },
     fechaAlquiler: DateTime.Today.AddDays(3),
     total: 0
 );


            var NoCalle = new AlquilerParaCrearDTO(
     NombreUsuario,
     Nombre,
     Apellido,
    "C/Rosario",
     metodoPago: MetodoPagoTipos.Visa,
     inicioAlquiler: DateTime.Today.AddDays(2),
     finAlquiler: DateTime.Today.AddDays(5),
     alquilerItems: new List<AlquilerItemDTO>()
     {
        new AlquilerItemDTO(
            cantidad: 1,
            precioAlquiler: 0,
            modelo: modelo2,
            fabricante: "Honda"
        )
     },
     fechaAlquiler: DateTime.Today.AddDays(3),
     total: 0
 );



            //Cada caso corresponde EXACTAMENTE con una validación de tu CreateAlquiler:

            var allTests = new List<object[]>
            {
                 new object[] { alquilerSinItem, "Debe seleccionar al menos un coche para alquilar." },//0 coches seleccionados
                 new object[] { inicioHoy, "¡Error! La fecha de inicio debe ser posterior a hoy." }, //inicio = hoy
                 new object[] { finAntesQueInicio, "¡Error! La fecha de finalización debe ser posterior a la de inicio." },//fin < inicio
                 new object[] { usuarioNoExiste, "El usuario indicado no existe." },//usuario incorrecto
                 new object[] { cocheNoDisponible, "no está disponible para las fechas" },//unidad no disponible
                 new object[] {NoCalle, "la dirección debe contener la palabra calle" }
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
            Console.WriteLine(errorActual);


            Assert.Contains(errorEsperado, errorActual);


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
       
    NombreUsuario,
    Nombre,
    Apellido,
    ConcesionarioEntrega,
    metodoPago: MetodoPagoTipos.Visa,
    inicioAlquiler: inicio,
    finAlquiler: fin,
    alquilerItems: new List<AlquilerItemDTO>()
    {
        new AlquilerItemDTO(
            cantidad: 1,
            precioAlquiler: 0,
            modelo: modelo2,
            fabricante: "Honda"
        )
    },
    fechaAlquiler: inicio,
    total: 0
);


            

            var expectedDetalle = new DetalleAlquilerDTO(
               2,
                fechaAlquiler: inicio,
                Nombre,
                Apellido,
                ConcesionarioEntrega,
                inicioAlquiler: inicio,
                finAlquiler: fin,
                metodoPago: MetodoPagoTipos.Visa,
                alquilerItems: new List<AlquilerItemDTO>()
                {
            new AlquilerItemDTO(
             
                cantidad: 1,
                precioAlquiler: 55,
                modelo: modelo2,
                fabricante: "Honda"
            )
                },
                total: 55
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
