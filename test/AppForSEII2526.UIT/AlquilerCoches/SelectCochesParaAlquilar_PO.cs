using OpenQA.Selenium.BiDi.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.AlquilerCoches
{
    public class SelectCochesParaAlquilar_PO : PageObject
    {
        By inputModelo = By.Id("cocheModelo");
        By inputPrecio=By.Id("cochePrecio");
        By buttonBuscarCoches = By.Id("buscarCoches");
        By tablaCoches = By.Id("tablaCochesDisponibles");
        public SelectCochesParaAlquilar_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        //precio se maneja como string ya que selenium envia texto al input html y estos reciben valores como texto
        public void BuscarCoches(string modelo,string precio)
        {
            WaitForBeingClickable(inputModelo);
            _driver.FindElement(inputModelo).SendKeys(modelo);

            if (precio == "") precio = "All";
            SelectElement selectElement = new SelectElement(_driver.FindElement(inputPrecio));
            selectElement.SelectByText(precio);
            _driver.FindElement(buttonBuscarCoches).Click();
        }

        public bool ComprobarCochesMostrados(List<string[]> expectedCoches)
        {
            return CheckBodyTable(expectedCoches, tablaCoches);
        }
    }
}
