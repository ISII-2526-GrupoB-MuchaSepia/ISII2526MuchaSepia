using AppForSEII2526.UIT.CU_Rental;
using AppForSEII2526.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppForSEII2526.UIT.UC_Alquiler
{
    public class UC_AlquilerCoches_UIT : UC_UIT
    {
        private SelectCochesParaAlquiler_PO selectCochesParaAlquiler_PO;
        private DetalleAlquiler_PO detalleAlquiler_PO;
        private CrearAlquiler_PO crearAlquiler_PO;

        private const string Nombre = "Lucas";
        private const string Apellido = "Maldonado";
        private const string MetodoPago = "Visa";
        private const string ConcesionarioEntrega = "Calle Rosario";
        private const string Cantidad = "1";

        private const int cocheId3 = 3;
        private const string cocheModelo3 = "Mercedes Clase C";
        private const string precioAlquiler3 = "120";
        private const string cocheColor3 = "Negro";
        private const string cocheFabricante3 = "Mercedes";
        private const string cocheCombustible3 = "Diesel";





        private const int cocheId1 = 1;
        private const string cocheModelo1 = "Seat León";
        private const string precioAlquiler1 = "50";
        private const string cocheColor1 = "Rojo";
        private const string cocheFabricante1 = "SEAT";
        private const string cocheCombustible1 = "Gasolina";

        private const int cocheId2 = 2;
        private const string cocheModelo2 = "Ford Focus";
        private const string precioAlquiler2 = "60";
        private const string cocheColor2 = "Rojo";
        private const string cocheFabricante2 = "Ford";
        private const string cocheCombustible2 = "Gasolina";



        public UC_AlquilerCoches_UIT(ITestOutputHelper output) : base(output)
        {
            Initial_step_opening_the_web_page();
           // PasosInicialesParaAlquilarCoches();
            selectCochesParaAlquiler_PO = new SelectCochesParaAlquiler_PO(_driver, _output);
            detalleAlquiler_PO = new DetalleAlquiler_PO(_driver, _output);
            crearAlquiler_PO = new CrearAlquiler_PO(_driver, _output);
        }

        private void Precondicion_login() //Cumple exactamente la precondición del caso de uso
        {
            Perform_login("elena@uclm.es", "Password1234%");
            Thread.Sleep(1000);
        }

        private void PasosInicialesParaAlquilarCoches()
        {
           // Precondicion_login();

            Thread.Sleep(1000);

            var byCrear = By.Id("CrearAlquiler");
            selectCochesParaAlquiler_PO.WaitForBeingClickable(byCrear);
            var elemento = _driver.FindElement(byCrear);

            IJavaScriptExecutor executor = (IJavaScriptExecutor)_driver;
            executor.ExecuteScript("arguments[0].click();", elemento);

            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                wait.Until(d => d.Url.Contains("alquileer"));
            }
            catch (WebDriverTimeoutException)
            {
                _driver.Navigate().GoToUrl(
                    new Uri(_driver.Url).GetLeftPart(UriPartial.Authority)
                    + "/alquileer/selectcochesparaalquiler"
                );
            }
        }


        //Cada InlineData representa un caso de prueba distinto.
        //Theory: el método de prueba se ejecuta varias veces con diferentes conjuntos de datos.

        [Theory]
        [InlineData(
    cocheModelo1, //el resultado esperado es encontrar aqui Honda Civic
    cocheFabricante1,
    cocheCombustible1,
    cocheColor1,
    precioAlquiler1,
    "Seat León", //filtra por modelo
    null
)]
        [InlineData(
    cocheModelo1,
    cocheFabricante1,
    cocheCombustible1,
    cocheColor1,
    precioAlquiler1,
    "",
    51f //filtra por precio
)]

        [InlineData(
    cocheModelo2,
    cocheFabricante2,
    cocheCombustible2,
    cocheColor2,
    precioAlquiler2,
    "Ford Focus",
    60f //filtra por precio
)]

        /*Flujo alternativo 1.- al paso 2
2.1 El sistema ofrece al cliente la opción de filtrar los artículos por modelo y precio.
2.2 El cliente fija los filtros que le interesan.
2.3 El sistema muestra solo los artículos que cumplen los criterios de los filtros
establecidos.

*/
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_AF1_filteringByModeloAndPrecio(string modelo, string fabricante, string combustible, string color, string precioAlquiler, string filtroModelo, float? filtroPrecioAlquiler)
        {
            //Arrange
            PasosInicialesParaAlquilarCoches();

            var expectedCoches = new List<string[]> //muestra los coches disponibles
            {
                new string[]
                {
                    modelo, color,precioAlquiler,fabricante, combustible
                }, };



            //ACT

            selectCochesParaAlquiler_PO.BuscarCoches(filtroModelo, filtroPrecioAlquiler);

            Thread.Sleep(1000);
            //Assert
            Assert.True(selectCochesParaAlquiler_PO.CheckListaDeCoches(expectedCoches));

        }


        /*Flujo Alternativo 2 - al Paso 4
Si el sistema detecta que no se ha seleccionado ningún coche, la opción para continuar
el proceso no estará activa.*/

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_AlquilerNoDispomible()
        {
            PasosInicialesParaAlquilarCoches();

            //Arrange


            //Act
            selectCochesParaAlquiler_PO.AñadirCocheAlCarroAlquiler(cocheModelo1);
            selectCochesParaAlquiler_PO.RemoveCocheDelCarroAlquiler(cocheModelo1);

            //Assert
            Assert.True(selectCochesParaAlquiler_PO.AlquilerNoDisponible());
        }


        /*Flujo alternativo 0 -> al paso 2:
Si no hay coches disponibles para alquilar en el periodo designado se le notificará al
usuario.*/
        
        
        
        [Theory] //admite parametros con InlineData
        //FACT no admite parametros
        [InlineData("noexisteunmodelo", null)] //modelo no existe
        [InlineData("", -1f)]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CochesNoDisponibles(string modelo,float? precio)
        {
            PasosInicialesParaAlquilarCoches();

            var mensaje= "No se han encontrado coches con los criterios de búsqueda.";
            selectCochesParaAlquiler_PO.BuscarCoches(modelo, precio);
            Thread.Sleep(1000);
            Assert.True(selectCochesParaAlquiler_PO.ComprobarMensajeErrorCocheNoDisponible(mensaje));



        }



/*

        [Theory]
        [InlineData(Nombre, Apellido, ConcesionarioEntrega, MetodoPago, cocheModelo3, Cantidad)]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_CU1_2_BasicFlow(
     string nombre,
     string apellido,
     string concesionarioEntrega,
     string metodoPago,
     string modelo,
     string cantidad)
        {
            

            PasosInicialesParaAlquilarCoches();

            var expected = new List<string[]>
    {
        new string[]
        {
            cocheModelo3,
            cocheFabricante3,
            precioAlquiler3 + " €",
            cantidad
        }
    };
            Thread.Sleep(2000);

            // Act
            selectCochesParaAlquiler_PO.AñadirCocheAlCarroAlquiler(modelo);
            selectCochesParaAlquiler_PO.AlquilarCoches();

            crearAlquiler_PO.RellenarDatosObligatorios(nombre, apellido, metodoPago, concesionarioEntrega);
            crearAlquiler_PO.RellenarCantidadAlquiler(modelo, cantidad);
            crearAlquiler_PO.PulsarAlquilarCoches();
            crearAlquiler_PO.PressOkModalDialog();
            Thread.Sleep(1000);
            // Assert
            Assert.True(
                detalleAlquiler_PO.CheckAlquilerDetalle(
                    nombre,
                    apellido,
                    concesionarioEntrega,
                    metodoPago,
                   
                    precioAlquiler3 + " €"
                ),
                "Error: rental detail is not as expected"
            );
            Thread.Sleep(1000);

            Assert.True(
                detalleAlquiler_PO.CheckListaDeCoches(expected),
                "Error: rental items are not as expected"
            );
        }


*/




        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void EliminarCochesCarrito() { 

            PasosInicialesParaAlquilarCoches();
            selectCochesParaAlquiler_PO.AñadirCocheAlCarroAlquiler(cocheModelo1);
            selectCochesParaAlquiler_PO.AñadirCocheAlCarroAlquiler(cocheModelo2);
            selectCochesParaAlquiler_PO.AlquilarCoches();

            crearAlquiler_PO.RellenarCantidadAlquiler(cocheModelo1, Cantidad);
            crearAlquiler_PO.RellenarCantidadAlquiler(cocheModelo2, Cantidad);
            crearAlquiler_PO.PulsaModificarCoches();

            selectCochesParaAlquiler_PO.RemoveCocheDelCarroAlquiler(cocheModelo1);
            selectCochesParaAlquiler_PO.AlquilarCoches();

            var PrecioEsperado = "60";
            Assert.True(crearAlquiler_PO.ComprobarPrecioTotal(PrecioEsperado));

        }

        [Theory]
        [InlineData("", Apellido, ConcesionarioEntrega, MetodoPago, "The Nombre field is required.")]
        [InlineData(Nombre, "", ConcesionarioEntrega, MetodoPago, "The Apellido field is required.")]
        [InlineData(Nombre, Apellido, "", MetodoPago, "The ConcesionarioEntrega field is required.")]

        [Trait("LevelTesting", "Funcional Testing")]
        public void FaltanDatosObligatorios(string nombre, string apellido, string concesionarioEntrega, string metodoPago, string expectedError)
        {
            //Arrange

           // var fecha = DateTime.Today.AddDays(1);
            //var inicio = fecha;
            //var fin = inicio.AddDays(1);
            PasosInicialesParaAlquilarCoches();

            selectCochesParaAlquiler_PO.BuscarCoches("", null);

            selectCochesParaAlquiler_PO.AñadirCocheAlCarroAlquiler(cocheModelo1);

            selectCochesParaAlquiler_PO.AlquilarCoches();

            //Act
            crearAlquiler_PO.RellenarDatosObligatorios(nombre, apellido, metodoPago,concesionarioEntrega);
           
            crearAlquiler_PO.PulsarAlquilarCoches();

            //Assert
            Assert.True(crearAlquiler_PO.ComprobarErrores(expectedError));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void ModificarAlquilerItems()
        {
            //Arrange
            PasosInicialesParaAlquilarCoches();
            
            
            
            var expectedAlquilerItems = new List<string[]> { new string[] { cocheModelo1, cocheFabricante1, precioAlquiler1 }, };

            //Act
            selectCochesParaAlquiler_PO.AñadirCocheAlCarroAlquiler(cocheModelo1);
            selectCochesParaAlquiler_PO.AñadirCocheAlCarroAlquiler(cocheModelo2);
            selectCochesParaAlquiler_PO.AlquilarCoches();

            crearAlquiler_PO.RellenarDatosObligatorios(Nombre,Apellido,MetodoPago,ConcesionarioEntrega );
            crearAlquiler_PO.RellenarCantidadAlquiler(cocheModelo1,Cantidad);
            crearAlquiler_PO.RellenarCantidadAlquiler(cocheModelo2, Cantidad);

            crearAlquiler_PO.PulsaModificarCoches();
            selectCochesParaAlquiler_PO.RemoveCocheDelCarroAlquiler(cocheModelo2 );
            selectCochesParaAlquiler_PO.AlquilarCoches();

            

        
            Assert.True(crearAlquiler_PO.ComprobarArticulosAlquiler(expectedAlquilerItems, Cantidad,cocheModelo1));
        }


        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void examen()
        {


            PasosInicialesParaAlquilarCoches();

            var expected = new List<string[]>
    {
        new string[]
        {


             cocheModelo3,
            cocheFabricante3,
            precioAlquiler3 + " €",
            Cantidad
            
        }
    };

            Thread.Sleep(1000);

            selectCochesParaAlquiler_PO.BuscarCoches(cocheModelo1, null);
            Thread.Sleep(1000);
            selectCochesParaAlquiler_PO.AñadirCocheAlCarroAlquiler(cocheModelo1);

            Thread.Sleep(1000);

            selectCochesParaAlquiler_PO.BuscarCoches("", 120);
            Thread.Sleep(1000);
            selectCochesParaAlquiler_PO.AñadirCocheAlCarroAlquiler(cocheModelo3);
            Thread.Sleep(1000);
            selectCochesParaAlquiler_PO.AlquilarCoches();
            Thread.Sleep(1000);
            crearAlquiler_PO.PulsaModificarCoches();
            Thread.Sleep(1000);
            selectCochesParaAlquiler_PO.RemoveCocheDelCarroAlquiler(cocheModelo1);
            Thread.Sleep(1000);
            selectCochesParaAlquiler_PO.AlquilarCoches();

            crearAlquiler_PO.RellenarDatosObligatorios(Nombre, Apellido, MetodoPago, ConcesionarioEntrega);
            Thread.Sleep(1000);
           
            crearAlquiler_PO.RellenarCantidadAlquiler(cocheModelo3, Cantidad);
            Thread.Sleep(1000);
            crearAlquiler_PO.PulsarAlquilarCoches();
            Thread.Sleep(1000);
            crearAlquiler_PO.PressOkModalDialog();
            Thread.Sleep(1000);




            Assert.True(
               detalleAlquiler_PO.CheckListaDeCoches(expected),
               "Error: rental items are not as expected"
           );
        }


        }
}

