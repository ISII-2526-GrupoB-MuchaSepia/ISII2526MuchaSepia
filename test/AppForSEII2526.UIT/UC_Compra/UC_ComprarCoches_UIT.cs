using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AppForSEII2526.UIT.CU_Purchase;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Compra
{
    public class UC_ComprarCoches_UIT : UC_UIT
    {
        private SelectCochesParaCompra_PO selectCochesParaCompra_PO;
        private CrearCompra_PO crearCompra_PO;
        private DetallesCompra_PO detallesCompra_PO;

        private const string nombre = "Elena";
        private const string apellido = "Ruiz";
        private const string concesionarioEntrega = "C/Albacete 1";
        private const string cantidad = "1";

        private const string metodopago1 = "Google Pay";
        private const string metodopago2 = "Visa";

        private const int cocheId1 = 1;
        private const string modelo1 = "Mercedes Clase C";
        private const string color1 = "Negro";
        private const string tipoCombustible1 = "Diesel";
        private const string fabricante1 = "Mercedes";
        private const string descripcion1 = "Sedán premium de tamaño medio";
        private const string preciocompra1 = "25000";

        private const int cocheId2 = 7;
        private const string modelo2 = "Seat León";
        private const string color2 = "Rojo";
        private const string tipoCombustible2 = "Gasolina";
        private const string fabricante2 = "SEAT";
        private const string descripcion2 = "Compacto español eficiente y moderno";
        private const string preciocompra2 = "15000";

        
        public UC_ComprarCoches_UIT(ITestOutputHelper output) : base(output)
        {
            Initial_step_opening_the_web_page();
            selectCochesParaCompra_PO = new SelectCochesParaCompra_PO(_driver, _output);
            crearCompra_PO = new CrearCompra_PO(_driver, _output);
            detallesCompra_PO = new DetallesCompra_PO(_driver, _output);
        }
        private void Precondition_perform_login()
        {
            Perform_login("elena@uclm.es", "Password1234%");
        }
        
        private void InitialStepsForPurchaseCars()
        {
            //Precondition_perform_login();
            
            selectCochesParaCompra_PO.WaitForBeingVisibleIgnoringExeptionTypes(By.Id("CrearCompra"));
            _driver.FindElement(By.Id("CrearCompra")).Click();
            Thread.Sleep(2000);
        }
        
        [Theory]
        [InlineData(modelo1, nombre, apellido, concesionarioEntrega, metodopago1, cantidad)]
        [InlineData(modelo1, nombre, apellido, concesionarioEntrega, metodopago2, cantidad)]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_CU1_2_FlujoBasico(string modelo, string nombre, string apellido, string concesinarioEntrega, string metodoPago, string cantidad)
        {
            //Arrange
            InitialStepsForPurchaseCars();
            var expectedCompratems = new List<string[]> { new string[] { modelo1, preciocompra1, color1, cantidad }, };
            Thread.Sleep(2000);
            //Act
            selectCochesParaCompra_PO.AddCocheToCompraCart(modelo);
            selectCochesParaCompra_PO.ComprarCoche();
            Thread.Sleep(1000);
            crearCompra_PO.FillInPurchaseInfo(nombre, apellido, concesinarioEntrega, metodoPago);
            crearCompra_PO.FillInPurchaseQuantity(cantidad, modelo);
            crearCompra_PO.PressPurchaseYourCars();
            //crearCompra_PO.PressOkModalDialog();
            Thread.Sleep(1000);
            //Assert
            Assert.True(detallesCompra_PO.CheckPurchaseDetail(nombre + " " + apellido,
               concesinarioEntrega, DateTime.Now, preciocompra1),
               "Error: El detalle de la compra no es el esperado");
            Thread.Sleep(1000);
            Assert.True(detallesCompra_PO.CheckListOfPurchase(expectedCompratems),
                "Error: purchase items are not as expected");
        }
        
        
        /*
         * Flujo alternativo 0 - al paso 2
           Si el sistema detecta que no hay coches disponibles, se notificará al usuario.
         */
       
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_FA0_CU1_3_CochesNoDisponibles()
        {
            //Arrange
            InitialStepsForPurchaseCars();
            var expectedMessage = "No hay coches disponibles para comprar.";
            Thread.Sleep(1000);
            //Act
            selectCochesParaCompra_PO.BuscarCoche(".", ".");
            Thread.Sleep(1000);
            //Assert
            Assert.True(selectCochesParaCompra_PO.CheckMessageErrorNotAvailableCars(expectedMessage));
        }
        

        /*
         * Flujo alternativo 1.- al paso 2
           2.1 El sistema ofrece al cliente la opción de filtrar los artículos por color y modelo.
           2.2 El cliente fija los filtros que le interesan.
           2.3 El sistema muestra solo los artículos que cumplen los criterios de los filtros
        */
        
        [Theory]
        [InlineData(modelo1, color1, tipoCombustible1, fabricante1, preciocompra1, "Negro", "")]
        [InlineData(modelo2, color2, tipoCombustible2, fabricante2, preciocompra2, "", "Seat León")]
        [InlineData(modelo2, color2, tipoCombustible2, fabricante2, preciocompra2, "Rojo", "Seat León")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_FA1_CU1_4_5_6_Filtrado(string modelo, string color, string tipoCombustible, string fabricante, string precioCompra, string filtroColor, string filtroModelo)
        {
            //Arrange
            InitialStepsForPurchaseCars();
            var expectedCars = new List<string[]> { new string[] { modelo, color, precioCompra,fabricante, tipoCombustible }, };
            Thread.Sleep(1000);
            //Act
            selectCochesParaCompra_PO.BuscarCoche(filtroColor, filtroModelo);

            Thread.Sleep(1000);
            //Assert
            Assert.True(selectCochesParaCompra_PO.CheckListOfCoches(expectedCars));
        }
        
        [Theory]
        [InlineData(modelo1, modelo2, color1, color2, tipoCombustible1, tipoCombustible2, fabricante1, fabricante2, preciocompra1, preciocompra2, "", "")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_FA1_CU1_7_Fitrado(string modelo1, string modelo2, string color1, string color2, string tipoCombustible1, string tipoCombustible2, string fabricante1, string fabricante2, string preciocompra1, string preciocompra2, string filtroColor, string filtroModelo)
        {
            //Arrange
            InitialStepsForPurchaseCars();
            var expectedCoches = new List<string[]>
                {
                    new string[] { modelo1, color1, preciocompra1, fabricante1, tipoCombustible1 },
                    new string[] { "Volkswagen Golf", "Azul", "15000", "Volkswagen", "Gasolina" },
                    new string[] { "Ford Focus", "Rojo", "12000", "Ford", "Gasolina" },
                    new string[] { "Kia Sportage", "Blanco", "23000", "Kia", "Diesel" },
                    new string[] { "Peugeot 308", "Gris", "14000", "Peugeot", "Gasolina" },
                    new string[] { "Honda Civic", "Verde", "16000", "Honda", "Gasolina" },
                    new string[] { modelo2, color2, preciocompra2, fabricante2, tipoCombustible2 }
                };


            //Act
            selectCochesParaCompra_PO.BuscarCoche(filtroColor, filtroModelo);

            //Assert
            Assert.True(selectCochesParaCompra_PO.CheckListOfCoches(expectedCoches));
        }
        
        /*
         * Flujo Alternativo 2 - al Paso 4
           Si el sistema detecta que no se ha seleccionado ningún coche, la opción para continuar
           el proceso no estará activa.
         */
        
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_FA2_CU1_8_CompraNoDisponible()
        {
            //Arrange
            InitialStepsForPurchaseCars();
            Thread.Sleep(1000);
            //Act
            selectCochesParaCompra_PO.AddCocheToCompraCart(modelo1);
            selectCochesParaCompra_PO.RemoveCocheFromCompraCart(modelo1);
            Thread.Sleep(1000);
            //Assert
            Assert.True(selectCochesParaCompra_PO.CompraNotAvailable());
        }
        
        /*
          Flujo Alternativo 3 - al Paso 5
          El cliente elige modificar el carrito para eliminar alguno de los coches seleccionados.
          Automáticamente, el sistema actualiza el precio total del contenido del carrito de
          compra.
        */
        
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        
        public void UC1_FA3_CU1_9_EliminarCochesSelecionados()
        {
            //Arrange
            InitialStepsForPurchaseCars();
            var expectedTotalPrice = "25000";
            Thread.Sleep(1000);
            //Act
            selectCochesParaCompra_PO.AddCocheToCompraCart(modelo1);
            selectCochesParaCompra_PO.AddCocheToCompraCart(modelo2);
            selectCochesParaCompra_PO.ComprarCoche();
            Thread.Sleep(1000);
            crearCompra_PO.FillInPurchaseQuantity(cantidad, modelo1);
            crearCompra_PO.FillInPurchaseQuantity(cantidad, modelo2);
            crearCompra_PO.PressModifyCars();
            Thread.Sleep(1000);
            selectCochesParaCompra_PO.RemoveCocheFromCompraCart(modelo2);
            selectCochesParaCompra_PO.ComprarCoche();

            Thread.Sleep(1000);
            //Assert
            Assert.True(crearCompra_PO.CheckTotalPrice(expectedTotalPrice));
        }
        
        /*
         * Flujo Alternativo 4 – al Paso 6
            Si el sistema detecta que algún dato obligatorio no se ha rellenado, notificará al usuario
            y volverá al paso 5.
         */
        
        [Theory]
        [InlineData(modelo1, "", apellido, concesionarioEntrega, metodopago1, cantidad, "The field Nombre must be a string with a minimum length of 2 and a maximum length of 20.")]
        [InlineData(modelo1, nombre, "", concesionarioEntrega, metodopago1, cantidad, "The field Apellido must be a string with a minimum length of 2 and a maximum length of 50.")]
        [InlineData(modelo1, nombre, apellido, "", metodopago1, cantidad, "The field ConcesionarioEntrega must be a string with a minimum length of 1 and a maximum length of 20.")]
        [InlineData(modelo1, nombre, apellido, concesionarioEntrega, metodopago1, "", "The Cantidad field must be a number.")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_FA4_CU1_10_11_12_13_Testing_Errors_Mandatory_Data(string modelo, string nombre, string apellido, string concesionarioEntrega, string metododePago, string cantidad, string expectedMessageError)
        {
            //Arrange
            InitialStepsForPurchaseCars();

            //Act
            selectCochesParaCompra_PO.AddCocheToCompraCart(modelo);
            selectCochesParaCompra_PO.ComprarCoche();

            crearCompra_PO.FillInPurchaseInfo(nombre, apellido, concesionarioEntrega, metododePago);
            crearCompra_PO.FillInPurchaseQuantity(cantidad, modelo);

            crearCompra_PO.PressPurchaseYourCars();

            //Assert
            Assert.True(crearCompra_PO.CheckValidationError(expectedMessageError), $"Expected error: {expectedMessageError}");
        }
        

        /*  Flujo Alternativo 5 - al Paso 7
            El cliente elige modificar los coches seleccionados. El sistema volverá al paso 2 sin perder
            la información que el usuario ya había introducido.
        */
        
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_FA5_CU1_15_ModificarCochesSelecionados()
        {
            //Arrange
            InitialStepsForPurchaseCars();
            var expectedPurchaseItems = new List<string[]> { new string[] { modelo1, color1, descripcion1, preciocompra1.Trim(' ', '€') }, };
            Thread.Sleep(1000);
            //Act
            selectCochesParaCompra_PO.AddCocheToCompraCart(modelo1);
            selectCochesParaCompra_PO.AddCocheToCompraCart(modelo2);
            selectCochesParaCompra_PO.ComprarCoche();
            Thread.Sleep(1000);

            crearCompra_PO.FillInPurchaseInfo(nombre, apellido, concesionarioEntrega, metodopago1);
            crearCompra_PO.FillInPurchaseQuantity(cantidad, modelo1);
            crearCompra_PO.FillInPurchaseQuantity(cantidad, modelo2);
            Thread.Sleep(1000);

            crearCompra_PO.PressModifyCars();
            selectCochesParaCompra_PO.RemoveCocheFromCompraCart(modelo2);
            selectCochesParaCompra_PO.ComprarCoche();
            Thread.Sleep(1000);
            //Assert
            Assert.True(crearCompra_PO.CheckListOfPurchaseItems(expectedPurchaseItems, cantidad, modelo1));
        }
       
    }

}
