using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReseñarDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AppForSEII2526.UT.ReseñarController_Test
{
    // CLASE DE PRUEBAS UNITARIAS PARA EL MÉTODO GET DETAILS DEL CONTROLADOR RESEÑAR
    public class Get_Reseñar_test : AppForSEII25264SqliteUT
    {
        // CONSTRUCTOR QUE INICIALIZA LA BASE DE DATOS EN MEMORIA CON DATOS DE PRUEBA
        public Get_Reseñar_test()
        {
            var modelo = new Modelo("Mercedes Clase C");
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
                nombre: "Lucas",
                apellido: "Maldonado",
                nombreUsuario: "lucas@correo.com",
                direccion: "Calle Falsa 123"
            )
            {
                UserName = "lucas@correo.com",
                EmailConfirmed = true
            };

            var reseñar = new Reseñar(
                id: 1,
                usuario: "Lucas",
                pais: "España",
                tipoConductor: "Titular",
                creado: DateTime.Today,
                applicationUser: usuario
            );

            var reseñarItem = new ReseñarItem(
                coche: coche,
                calificacion: 5,
                reseñar: reseñar,
                descripcion: "Excelente coche"
            );

            // Añade el item a la reseña
            reseñar.ReseñarItems.Add(reseñarItem);

            // Agrega todos los datos a la base de datos en memoria y guarda cambios
            _context.AddRange(modelo, coche, usuario, reseñar, reseñarItem);
            _context.SaveChanges();
        }

        // PRUEBA PARA CASO EN QUE NO EXISTE RESEÑA CON ID SOLICITADO, SE ESPERA NOTFOUND (404)
        [Fact]
        public async Task Get_Detalle_Reseñar_NotFound_Test()
        {
            var mock = new Moq.Mock<ILogger<ReseñarController>>();
            var controller = new ReseñarController(_context, mock.Object);

            var result = await controller.GetDetails(id: 99);

            Assert.IsType<NotFoundResult>(result);
        }

        // PRUEBA PARA CASO EXITOSO QUE EXISTE RESEÑA, SE ESPERA OK CON EL DTO CORRECTO
        [Fact]
        public async Task Get_Detalle_Reseñar_Found_Test()
        {
            var mock = new Moq.Mock<ILogger<ReseñarController>>();
            var controller = new ReseñarController(_context, mock.Object);

            // DTO esperado con los datos para comparar después
            var esperado = new DetallesReseñarDTO(
                id: 1,
                creado: DateTime.Today,
                usuario: "Lucas",
                pais: "España",
                tipoConductor: "Titular",
                applicationUser: null, // No relevante para comparación en este test
                reseñarItems: new List<ReseñarItemDTO>
                {
                    new ReseñarItemDTO(
                        reseñarId: 1,
                        cocheId: 1,
                        cocheNombre: "Berlina",
                        calificacion: 5,
                        descripcion: "Excelente coche"
                    )
                }
            );

            var result = await controller.GetDetails(id: 1);

            // Se comprueba que la respuesta sea ok con un DTO del tipo esperado
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<DetallesReseñarDTO>(okResult.Value);

            // Verifica los campos principales
            Assert.Equal(esperado.Id, actual.Id);
            Assert.Equal(esperado.Usuario, actual.Usuario);
            Assert.Equal(esperado.Pais, actual.Pais);
            Assert.Equal(esperado.TipoConductor, actual.TipoConductor);
            Assert.Equal(esperado.ReseñarItems.Count, actual.ReseñarItems.Count);

            // Compara en detalle el item de la lista de reseña
            var esperadoItem = esperado.ReseñarItems.First();
            var actualItem = actual.ReseñarItems.First();

            Assert.Equal(esperadoItem.CocheId, actualItem.CocheId);
            Assert.Equal(esperadoItem.CocheNombre, actualItem.CocheNombre);
            Assert.Equal(esperadoItem.Calificacion, actualItem.Calificacion);
            Assert.Equal(esperadoItem.Descripcion, actualItem.Descripcion);
        }
    }
}
