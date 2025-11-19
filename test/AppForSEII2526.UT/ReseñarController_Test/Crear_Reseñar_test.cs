using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReseñarDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.ReseñarController_Test
{
    // CLASE DE PRUEBAS UNITARIAS PARA CREACION DE RESEÑAS EN EL CONTROLADOR
    public class Crear_Reseñar_test : AppForSEII25264SqliteUT
    {
        // CONSTANTES CON DATOS DE PRUEBA USADOS EN LOS TESTS
        private const string UsuarioExistente = "lucas@correo.com";
        private const string NombreUsuarioExistente = "Lucas";
        private const string PaisUsuarioExistente = "España";
        private const string TipoConductorExistente = "Titular";
        private const string CocheModeloExistente = "Mercedes Clase C";

        // CONSTRUCTOR: INICIALIZA DATOS DE PRUEBA EN LA BASE DE DATOS EN MEMORIA
        public Crear_Reseñar_test()
        {
            var modelo = new Modelo(CocheModeloExistente);
            var coche = new Coche(
                claseCoche: "Berlina",
                color: "Negro",
                descripcion: "Sedán premium de tamaño medio",
                desplazamientoMotor: "180 CV",
                tipoCombustible: "Diesel",
                fabricante: "Mercedes",
                precioCompra: 25000d,
                cantidadCompra: 4,
                cantidadAlquiler: 2,
                precioAlquiler: 120,
                tamanoLlanta: "55.0",
                modelo: modelo,
                tiposdeMantenimiento: Coche.TipoMantenimiento.Aceite
            );

            var usuario = new ApplicationUser(
                id: "1",
                nombre: NombreUsuarioExistente,
                apellido: "Maldonado",
                nombreUsuario: UsuarioExistente,
                direccion: "Calle Falsa 123"
            )
            {
                UserName = UsuarioExistente,
                EmailConfirmed = true
            };

            _context.AddRange(modelo, coche, usuario);
            _context.SaveChanges();
        }

        // DATOS DE PRUEBA PARA CASOS DE ERROR AL CREAR RESEÑA
        public static IEnumerable<object[]> TestCasesFor_CreateReseñar()
        {
            // CASO: RESENA SIN ITEMS (ERROR)
            var reseñaSinItems = new CreacionesReseñarDTO(
                usuario: "lucas@correo.com",
                tipoConductor: "Titular",
                pais: "España",
                creado: DateTime.Today
            )
            {
                ReseñarItems = new List<ReseñarItemDTO>()
            };

            // CASO: TIPO DE CONDUCTOR INVÁLIDO
            var tiposInvalido = new CreacionesReseñarDTO(
                usuario: "lucas@correo.com",
                tipoConductor: "Inválido",
                pais: "España",
                creado: DateTime.Today
            )
            {
                ReseñarItems = new List<ReseñarItemDTO>()
                {
                    new ReseñarItemDTO(0, 1, "Mercedes Clase C", 5, "Muy buen coche")
                }
            };

            // CASO: USUARIO NO REGISTRADO
            var usuarioNoExiste = new CreacionesReseñarDTO(
                usuario: "noexiste@correo.com",
                tipoConductor: "Titular",
                pais: "España",
                creado: DateTime.Today
            )
            {
                ReseñarItems = new List<ReseñarItemDTO>()
                {
                    new ReseñarItemDTO(0, 1, "Mercedes Clase C", 5, "Muy buen coche")
                }
            };

            // CASO: COCHE NO EXISTENTE
            var cocheNoExiste = new CreacionesReseñarDTO(
                usuario: "lucas@correo.com",
                tipoConductor: "Titular",
                pais: "España",
                creado: DateTime.Today
            )
            {
                ReseñarItems = new List<ReseñarItemDTO>()
                {
                    new ReseñarItemDTO(0, 99, "NoExiste", 3, "Coche inexistente")
                }
            };

            // RETORNA LOS CASOS DE PRUEBA: DTO Y MENSAJE DE ERROR ESPERADO
            return new List<object[]>
            {
                new object[] { reseñaSinItems, "Debes añadir al menos una reseña de coche." },
                new object[] { tiposInvalido, "El tipo de conductor debe ser 'Titular' o 'Adicional'." },
                new object[] { usuarioNoExiste, "Error! Usuario no registrado" },
                new object[] { cocheNoExiste, "Error! El coche NoExiste no existe." }
            };
        }

        // PRUEBA PARAMETRIZADA USANDO DATOS DE ERROR, ESPERANDO BAD REQUEST CON MENSAJE ESPERADO
        [Theory]
        [MemberData(nameof(TestCasesFor_CreateReseñar))]
        public async Task CrearReseñar_Error_test(CreacionesReseñarDTO dto, string errorEsperado)
        {
            // ARRANGE: MOCK DEL LOGGER Y CREACION DEL CONTROLADOR CON CONTEXTO DE BD Y LOGGER
            var mock = new Moq.Mock<ILogger<ReseñarController>>();
            ILogger<ReseñarController> logger = mock.Object;
            var controller = new ReseñarController(_context, logger);

            // ACT: LLAMADA ASINCRÓNICA AL MÉTODO Create DEL CONTROLADOR
            var result = await controller.Create(dto);

            // ASSERT: SE ESPERA BADREQUEST, CON ValidationProblemDetails CONTENIENDO EL ERROR CORRECTO
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<Microsoft.AspNetCore.Mvc.ValidationProblemDetails>(badRequestResult.Value);
            var errorActual = problemDetails.Errors.First().Value[0];
            Assert.StartsWith(errorEsperado, errorActual);
        }

        // PRUEBA PARA CREAR UNA RESEÑA EXITOSAMENTE
        [Fact]
        public async Task CrearReseñar_Success_test()
        {
            // ARRANGE: MOCK LOGGER Y CONTROLADOR COMO EN PRUEBA DE ERROR
            var mock = new Moq.Mock<ILogger<ReseñarController>>();
            ILogger<ReseñarController> logger = mock.Object;
            var controller = new ReseñarController(_context, logger);

            // CREA ITEMS PARA LA RESEÑA
            var items = new List<ReseñarItemDTO>()
            {
                new ReseñarItemDTO(
                    reseñarId: 0,
                    cocheId: 1,
                    cocheNombre: "Mercedes Clase C",
                    calificacion: 5,
                    descripcion: "Excelente coche"
                )
            };

            // CREA DTO DE RESEÑA CON DATOS VALIDOS
            var dto = new CreacionesReseñarDTO(
                usuario: UsuarioExistente,
                tipoConductor: TipoConductorExistente,
                pais: PaisUsuarioExistente,
                creado: DateTime.Today
            )
            {
                ReseñarItems = items
            };

            // ACT: LLAMADA AL MÉTODO Create
            var result = await controller.Create(dto);

            // ASSERT: SE ESPERA CREATED (201) CON EL DTO DETALLE CON LOS DATOS COINCIDENTES
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var detalleDTO = Assert.IsType<DetallesReseñarDTO>(createdResult.Value);

            Assert.Equal(dto.Usuario, detalleDTO.Usuario);
            Assert.Equal(dto.Pais, detalleDTO.Pais);
            Assert.Equal(dto.TipoConductor, detalleDTO.TipoConductor);
            Assert.NotEmpty(detalleDTO.ReseñarItems);
            Assert.Equal(dto.ReseñarItems.Count, detalleDTO.ReseñarItems.Count);
        }
    }
}
