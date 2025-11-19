using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReseñarDTOs;
using AppForSEII2526.Shared.CocheDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.CocheController_test
{
    // CLASE DE PRUEBAS UNITARIAS PARA EL MÉTODO GET COCHES PARA RESEÑAR DEL CONTROLADOR COCHE
    public class Get_Coches_Reseñar_test : AppForSEII25264SqliteUT
    {
        // CONSTRUCTOR QUE INICIALIZA DATOS DE PRUEBA EN LA BASE DE DATOS EN MEMORIA
        public Get_Coches_Reseñar_test()
        {
            var modelos = new List<Modelo>
            {
                new Modelo("Mercedes Clase C"),
                new Modelo("Volkswagen Golf"),
                new Modelo("Toyota Corolla")
            };

            var coches = new List<Coche>
            {
                new Coche("Berlina", "Negro", "Sedán premium", "180 CV", "Diesel", "Mercedes", 25000d, 4, 2, 120, "17", modelos[0], Coche.TipoMantenimiento.Aceite),
                new Coche("Compacto", "Azul", "Versátil y eficiente", "110 CV", "Gasolina", "Volkswagen", 20000d, 6, 3, 80, "16", modelos[1], Coche.TipoMantenimiento.Frenos),
                new Coche("Sedán", "Blanco", "Cómodo para familias", "140 CV", "Híbrido", "Toyota", 23000d, 7, 2, 90, "17", modelos[2], Coche.TipoMantenimiento.Refrigeracion)
            };

            _context.AddRange(modelos);
            _context.AddRange(coches);
            _context.SaveChanges();
        }

        // DATOS DE PRUEBA PARAMETRIZADOS PARA DIFERENTES FILTROS Y RESULTADOS ESPERADOS
        public static IEnumerable<object[]> TestCasesFor_GetCochesParaReseñar_OK()
        {
            var cocheDTOs = new List<CocheParaReseñarDTO>
            {
                new CocheParaReseñarDTO(1, "Berlina", "Negro", "Sedán premium", "Mercedes Clase C"),
                new CocheParaReseñarDTO(2, "Compacto", "Azul", "Versátil y eficiente", "Volkswagen Golf"),
                new CocheParaReseñarDTO(3, "Sedán", "Blanco", "Cómodo para familias", "Toyota Corolla")
            };

            var test1 = new List<CocheParaReseñarDTO> { cocheDTOs[0], cocheDTOs[1], cocheDTOs[2] };
            var test2 = new List<CocheParaReseñarDTO> { cocheDTOs[1] };
            var test3 = new List<CocheParaReseñarDTO> { cocheDTOs[2] };

            var allTests = new List<object[]>
            {
                new object[] { null, null, test1 },        // SIN FILTROS: RETORNA TODOS
                new object[] { "Compacto", null, test2 },  // FILTRADO POR CLASE "Compacto"
                new object[] { null, "Blanco", test3 }     // FILTRADO POR COLOR "Blanco"
            };

            return allTests;
        }

        // PRUEBA PARAMETRIZADA QUE VERIFICA QUE EL MÉTODO DEVUELVE LOS COCHES ESPERADOS SEGÚN FILTROS
        [Theory]
        [MemberData(nameof(TestCasesFor_GetCochesParaReseñar_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCochesParaReseñar_OK_test(string? claseCoche, string? color, List<CocheParaReseñarDTO> expected)
        {
            // ARRANGE: CREAR MOCK DEL LOGGER Y CONTROLADOR CON CONTEXTO DE BD
            var mock = new Moq.Mock<ILogger<CocheController>>();
            var controller = new CocheController(_context, mock.Object);

            // ACT: LLAMAR ASINCRÓNICAMENTE AL MÉTODO CON FILTROS DE ENTRADA
            var result = await controller.GetCochesParaReseñar(claseCoche, color);

            // ASSERT: SE ESPERA RESPUESTA OK CON LISTA DE DTOs Y QUE COINCIDAN CON LOS ESPERADOS
            var okResult = Assert.IsType<OkObjectResult>(result);
            var cochesActual = Assert.IsAssignableFrom<IList<CocheParaReseñarDTO>>(okResult.Value);

            Assert.Equal(expected.Count, cochesActual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].Id, cochesActual[i].Id);
                Assert.Equal(expected[i].ClaseCoche, cochesActual[i].ClaseCoche);
                Assert.Equal(expected[i].Color, cochesActual[i].Color);
                Assert.Equal(expected[i].Descripcion, cochesActual[i].Descripcion);
                Assert.Equal(expected[i].Modelo, cochesActual[i].Modelo);
            }
        }
    }
}
