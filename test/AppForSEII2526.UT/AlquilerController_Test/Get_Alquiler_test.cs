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
            var modelos = new List<Modelo>
            {
                new Modelo("Toyota Supra"),
                new Modelo("Honda Civic Type R"),
                

            };

            //COCHES

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
            ApplicationUser usuario = new ApplicationUser(id: "1",
            nombre: "Clara",
            apellido: "Lopez",
            nombreUsuario: "Clara@lopez",
            direccion: "Calle Mayor 12, Toledo");

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

            


            // Calcular el total del alquiler

            foreach (var item in alquiler.AlquilerItems)
            {
                alquiler.Total += coches[item.CocheId - 1].PrecioAlquiler *
                                 item.Cantidad *
                                 (int)(alquiler.FinAlquiler - alquiler.InicioAlquiler).TotalDays;
            }

            _context.Add(usuario);
            _context.AddRange(modelos);
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
     fechaAlquiler: DateTime.Today,
     nombre: "Clara",
     apellido: "Lopez",
     concesionarioEntrega: "Granada Central",
     inicioAlquiler: DateTime.Today.AddDays(1),
     finAlquiler: DateTime.Today.AddDays(7),
     metodoPago: MetodoPagoTipos.GooglePay,
     alquilerItems: new List<AlquilerItemDTO>(),
     total: 0
 );

            esperado.AlquilerItems.Add(new AlquilerItemDTO(
                cantidad: 1,
                precioAlquiler: 190,
                modelo: "Toyota Supra",
                fabricante: "Toyota"
            ));


            //total= precio x cantidad x dias
            foreach (var item in esperado.AlquilerItems)
            {
                esperado.Total += item.PrecioAlquiler *
                                  item.Cantidad *
                                  (esperado.FinAlquiler - esperado.InicioAlquiler).TotalDays;
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
