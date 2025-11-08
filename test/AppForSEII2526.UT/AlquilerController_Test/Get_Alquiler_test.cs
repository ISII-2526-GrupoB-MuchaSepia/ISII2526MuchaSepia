using System;
using System.Collections.Generic;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.AlquilerDTOs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.UT.AlquilerController_Test
{
    public class Get_Alquiler_test : AppForSEII25264SqliteUT
    {
        public Get_Alquiler_test()
        {


            //MODELOS
            var modelo = new List<Modelo>
            {
                new Modelo("Toyota Sedán"),
                new Modelo("Tesla SUV"),
                

            };

            //COCHES

            var coches = new List<Coche>
            {
                new Coche(
                    claseCoche: "Sedán",
                    color: "Rojo",
                    descripcion: "Sedán económico y confiable",
                    desplazamientoMotor: "1.8L",
                    tipoCombustible: "Híbrido",
                    fabricante: "Toyota",
                    precioCompra: 18000m,
                    cantidadCompra: 12,
                    cantidadAlquiler: 4,
                    precioAlquiler: 65,
                    tamanoLlanta: "17",
                    modelo: modelo[0],
                    tiposdeMantenimiento: Coche.TipoMantenimiento.Frenos
                ),
                 new Coche(
                    claseCoche: "SUV Lujo",
                    color: "Negro",
                    descripcion: "SUV eléctrico de alta gama",
                    desplazamientoMotor: "0L",
                    tipoCombustible: "Eléctrico",
                    fabricante: "Tesla",
                    precioCompra: 85000m,
                    cantidadCompra: 3,
                    cantidadAlquiler: 2,
                    precioAlquiler: 120,
                    tamanoLlanta: "19",
                    modelo: modelo[1],
                    tiposdeMantenimiento: Coche.TipoMantenimiento.Suspension
                )
            };

            //USUARIO
            var usuario = new ApplicationUser(
               id: "10",
               nombre: "Laura",
               apellido: "García",
               nombreUsuario: "laura@correo.com",
               direccion: "Calle Valencia 123"
           );


            //ALQUILER
            var alquiler = new Alquiler(
                nombre: usuario.Nombre,
                apellido: usuario.Apellido,
                concesionarioEntrega: "Granada",
                fechaAlquiler: DateTime.Today.AddDays(1),
                metodoPago: MetodoPagoTipos.TarjetaCredito,
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

            // Calcular el total del alquiler

            foreach (var item in alquiler.AlquilerItems)
            {
                alquiler.Total += item.Coche.PrecioAlquiler *
                         item.Cantidad *
                         (alquiler.FinAlquiler - alquiler.InicioAlquiler).Days;
            }

            _context.Add(usuario);
            _context.AddRange(modelo);
            _context.AddRange(coches);
            _context.Add(alquiler);
            _context.SaveChanges();


        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]

        public async Task Get_Detalle_Alquiler_NotFound_Test()
        {

            //ARRANGE
            var mock = new Mock<ILogger<AlquilerController>>();
            var controller = new AlquilerController(_context, mock.Object);

            //ACT
            var result = await controller.Get_Detalle_Alquiler(0);

            //ASSERT
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]

        //test asíncrono que valida el caso no encontrado
        public async Task Get_Detalle_Alquiler_Found_Test()
        {
            //ARRANGE
            var mock = new Mock<ILogger<AlquilerController>>();
            var controller = new AlquilerController(_context, mock.Object);


            var esperado = new DetalleAlquilerDTO(
                id: 1,
                fechaAlquiler: DateTime.Today.AddDays(1),
                nombre: "Laura",
                apellido: "García",
                concesionarioEntrega: "Granada",
                inicioAlquiler: DateTime.Today,
                finAlquiler: DateTime.Today.AddDays(7),
                metodoPago: MetodoPagoTipos.TarjetaCredito,
                alquilerItems: new List<AlquilerItemDTO>(),
                total: 0
            );


            esperado.AlquilerItems.Add(new AlquilerItemDTO(
                cocheId: 1,
                cantidad: 3,
                precioAlquiler: 65,
                modelo: "Toyota Sedán",
                fabricante: "Toyota"
            ));

            //total= precio x cantidad x dias
            foreach (var item in esperado.AlquilerItems)
            {
                esperado.Total += item.PrecioAlquiler *
                                  item.Cantidad *
                                  (esperado.FinAlquiler - esperado.InicioAlquiler).Days;
            }

            //llamar al endpoint con id=1 ,ACT
            var result = await controller.Get_Detalle_Alquiler(1);

            //comprobar que el resultado es 200 OK,ASSERT
            var ok = Assert.IsType<OkObjectResult>(result);
            //verificas que sea un DetalleAlquilerDTO
            var actual = Assert.IsType<DetalleAlquilerDTO>(ok.Value);
            //guarda true o false en eq
            var eq = esperado.Equals(actual);

           //si todo lo guardado en DB durante el arrange coincide con expected, este assert pasa
            Assert.Equal(esperado, actual);

        }

        //esperado: es lo que se supone que debería devolver
        //actual: es lo que realmente devuelve
        //El test pasa SOLO SI ambos coinciden. comprueba si el endpoint funciona bien.
    }
}
