using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Compra
{
    public class CrearCompra_PO : PageObject
    {
        private By inputNombre = By.Id("Nombre");
        private By inputApellidos = By.Id("Apellidos");
        private By inputConcesionaroEntrega = By.Id("ConcesionarioEntrega");
        private By inputMetodoPago = By.Id("MetodoPago");
        private By precioTotal = By.Id("PrecioTotal");
        private By errorContainer = By.XPath("//div[@class='row alert alert-danger'][@role='alert']");

        private IWebElement nameElement() => _driver.FindElement(inputNombre);
        private IWebElement surnameElement() => _driver.FindElement(inputApellidos);
        private IWebElement deliveryElement() => _driver.FindElement(inputConcesionaroEntrega);
        private IWebElement paymentMethodElement() => _driver.FindElement(inputMetodoPago);

        public CrearCompra_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {

        }

        public void FillInPurchaseInfo(string nombre, string apellido, string concesionarioEntrega, string metododDepago)
        {
            WaitForBeingClickable(inputNombre);
            nameElement().SendKeys(nombre);
            surnameElement().SendKeys(apellido);
            deliveryElement().SendKeys(concesionarioEntrega);

            SelectElement selecmetodoPago = new SelectElement(paymentMethodElement());
            selecmetodoPago.SelectByText(metododDepago);
        }

        public void FillInPurchaseQuantity(string cantidad, string modelo)
        {
            WaitForBeingClickable(By.Id("quantity_" + modelo));
            _driver.FindElement(By.Id("quantity_" + modelo)).Clear();
            _driver.FindElement(By.Id("quantity_" + modelo)).SendKeys(cantidad);
        }

        public void PressPurchaseYourCars()
        {
            _driver.FindElement(By.Id("Submit")).Click();
        }

        public void PressModifyCars()
        {
            _driver.FindElement(By.Id("ModificarCoches")).Click();
        }

        public bool CheckListOfPurchaseItems(List<string[]> expectedPurchaseItems, string quantity, string model)
        {
            return CheckBodyTable(expectedPurchaseItems, By.Id("TablaCompraItems")) && _driver.FindElement(By.Id("quantity_" + model)).GetAttribute("value") == quantity;
        }

        public bool CheckValidationError(string expectedError)
        {
            Thread.Sleep(500);
            return _driver.PageSource.Contains(expectedError);
        }

        public bool CheckTotalPrice(string precioEsperado)
        {
            WaitForBeingVisible(precioTotal);
            string actualPriceText = _driver.FindElement(precioTotal).Text;
            return actualPriceText.Contains(precioEsperado);
        }
    }
}