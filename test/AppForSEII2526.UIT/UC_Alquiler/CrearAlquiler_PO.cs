using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppForSEII2526.UIT.CU_Rental
{
    internal class CrearAlquiler_PO : PageObject
    {
        private By Nombre = By.Id("AlquilerNombre");
        private By Apellido = By.Id("AlquilerApellido");
        private By ConcesionarioEntrega = By.Id("AlquilerConcesionarioEntrega");
        private By MetodoPago = By.Id("AlquilerMetodoPago");
        private By PrecioTotal = By.Id("PrecioTotal");

        private IWebElement _nombre() => _driver.FindElement(Nombre);
        private IWebElement _apellido() => _driver.FindElement(Apellido);
        private IWebElement _concesionarioEntrega() => _driver.FindElement(ConcesionarioEntrega);
        private IWebElement _metodoPago() => _driver.FindElement(MetodoPago);
        public CrearAlquiler_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {

        }

        public void RellenarCantidadAlquiler(string modelo,string cantidad)
        {
            Thread.Sleep(500);
            WaitForBeingClickable(By.Id("cantidad_" + modelo));
            _driver.FindElement(By.Id("cantidad_" + modelo)).Clear();
            _driver.FindElement(By.Id("cantidad_" + modelo)).SendKeys(cantidad);
        }

        public void RellenarDatosObligatorios(string nombre, string apellido, string metodoPago, string concesionarioEntrega)
        {
            WaitForBeingClickable(Nombre);
            _nombre().SendKeys(nombre);
            _apellido().SendKeys(apellido);
            _concesionarioEntrega().SendKeys(concesionarioEntrega);

            SelectElement selectPaymentMethod = new SelectElement(_metodoPago());
            selectPaymentMethod.SelectByText(metodoPago);
        }

     

        public void PulsarAlquilarCoches()
        {
            _driver.FindElement(By.Id("AlquilerSubmit")).Click();
        }
        public void PulsaModificarCoches()
        {
            _driver.FindElement(By.Id("ModificarCoches")).Click();
        }

        public bool CombrobarArticulosAlquiler(List<string[]> expectedAlquilerItems)
        {
            return CheckBodyTable(expectedAlquilerItems, By.Id("TablaArticulosAlquiler"));
        }

        public bool ComprobarArticulosAlquiler(List<string[]> expectedAlquilerItems, string cantidad, string modelo)
        {
            return CheckBodyTable(expectedAlquilerItems, By.Id("TablaArticulosAlquiler")) && _driver.FindElement(By.Id("cantidad_" + modelo)).GetAttribute("value") == cantidad;
        }

        public bool ComprobarPrecioTotal(string expectedPrecioTotal)
        {
            WaitForBeingVisible(PrecioTotal);
            string precioActual = _driver.FindElement(PrecioTotal).Text;
            return precioActual.Contains
                (precioActual);
        }


        public bool ComprobarErrores(string expectedError)
        {
            Thread.Sleep(500);
            return _driver.PageSource.Contains(expectedError);
        }


    }
}