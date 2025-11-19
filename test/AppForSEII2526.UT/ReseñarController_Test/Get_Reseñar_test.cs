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
    public class Get_Reseñar_test : AppForSEII25264SqliteUT
    {
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
            reseñar.ReseñarItems.Add(reseñarItem);

            _context.AddRange(modelo, coche, usuario, reseñar, reseñarItem);
            _context.SaveChanges();
        }

        [Fact]
        public async Task Get_Detalle_Reseñar_NotFound_Test()
        {
            var mock = new Moq.Mock<ILogger<ReseñarController>>();
            var controller = new ReseñarController(_context, mock.Object);

            var result = await controller.GetDetails(id: 99);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Get_Detalle_Reseñar_Found_Test()
        {
            var mock = new Moq.Mock<ILogger<ReseñarController>>();
            var controller = new ReseñarController(_context, mock.Object);

            // Lo que se espera que devuelva el controlador
            var esperado = new DetallesReseñarDTO(
                id: 1,
                creado: DateTime.Today,
                usuario: "Lucas",
                pais: "España",
                tipoConductor: "Titular",
                applicationUser: null, // no es relevante para la comparación
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

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<DetallesReseñarDTO>(okResult.Value);

            Assert.Equal(esperado.Id, actual.Id);
            Assert.Equal(esperado.Usuario, actual.Usuario);
            Assert.Equal(esperado.Pais, actual.Pais);
            Assert.Equal(esperado.TipoConductor, actual.TipoConductor);
            Assert.Equal(esperado.ReseñarItems.Count, actual.ReseñarItems.Count);

            // Comparación de los campos de ReseñarItemDTO
            var esperadoItem = esperado.ReseñarItems.First();
            var actualItem = actual.ReseñarItems.First();
            Assert.Equal(esperadoItem.CocheId, actualItem.CocheId);
            Assert.Equal(esperadoItem.CocheNombre, actualItem.CocheNombre);
            Assert.Equal(esperadoItem.Calificacion, actualItem.Calificacion);
            Assert.Equal(esperadoItem.Descripcion, actualItem.Descripcion);
        }
    }
}
