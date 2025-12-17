using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.UC_Compra
{
    public class SelectCochesParaCompra_PO:PageObject
    {
        By inputModelo = By.Id("inputModelo");
        By inputColor = By.Id("inputColor");
        By buttonbuscarCoches = By.Id("buttonbuscarCoches");
        By tablaCoches = By.Id("tablaCoches");
        By compraCocheButton = By.Id("compraCocheButton");
        By errorShown = By.Id("ErrorsShown");

        public SelectCochesParaCompra_PO(IWebDriver driver, ITestOutputHelper output):base(driver, output) { 
        }
        public void BuscarCoche(string color, string modelo)
        {
            //wait for the webelement to be clickable
            WaitForBeingClickable(inputColor);
            WaitForBeingClickable(inputModelo);
            _driver.FindElement(inputColor).SendKeys(color);
            _driver.FindElement(inputModelo).SendKeys(modelo);
            _driver.FindElement(buttonbuscarCoches).Click();
        }
        public bool CheckListOfCoches(List<string[]> expectedCoches)
        {
            return CheckBodyTable(expectedCoches,tablaCoches);
        }
        public void ComprarCoche()
        {
            WaitForBeingClickable(compraCocheButton);
            _driver.FindElement(compraCocheButton).Click();
        }
        public bool CheckMessageError(string errorMessage)
        {
            IWebElement actualErrorShown = _driver.FindElement(errorShown);
            _output.WriteLine($"actual Message shown:{actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }
        public bool CheckMessageErrorNotAvailableCars(string expectedError)
        {
            return _driver.PageSource.Contains(expectedError);
        }

        public void AddCocheToCompraCart(string modeloCoche)
        {
            WaitForBeingClickable(By.Id("cocheTocompra_" + modeloCoche));

            _driver.FindElement(By.Id("cocheTocompra_" + modeloCoche)).Click();
        }

        public void RemoveCocheFromCompraCart(string modeloCoche)
        {
            WaitForBeingClickable(By.Id("quitarCoche_" + modeloCoche));
            _driver.FindElement(By.Id("quitarCoche_" + modeloCoche)).Click();
        }

        public bool CompraNotAvailable()
        {
            //the button is not Displayed=hidden

            return _driver.FindElement(compraCocheButton).Displayed == false;
        }
        
    }
}
