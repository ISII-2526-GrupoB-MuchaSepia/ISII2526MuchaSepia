using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;
using Xunit.Sdk;

namespace AppForSEII2526.UIT.UC_Alquiler
{
    public  class SelectCochesParaAlquiler_PO :PageObject
    {
        By inputModelo = By.Id("nombremodelo");
        By inputPrecioAlquiler = By.Id("precioalquiler");
        By buttonBuscarCoches = By.Id("buscarCoches");
       

        By tablaCoches = By.Id("TablaDeCoches");

        
        By buttonAlquilarCoches = By.Id("alquilarCocheButton");


        public SelectCochesParaAlquiler_PO(IWebDriver driver,ITestOutputHelper output):base(driver, output)
        {

        }


        public void BuscarCoches(string modelo, float? precioAlquiler)
        {
            WaitForBeingClickable(inputModelo);

            _driver.FindElement(inputModelo).Clear();
            _driver.FindElement(inputModelo).SendKeys(modelo);

            _driver.FindElement(inputPrecioAlquiler).Clear();
            if (precioAlquiler.HasValue)
            {
                _driver.FindElement(inputPrecioAlquiler)
                       .SendKeys(precioAlquiler.Value.ToString());
            }

            _driver.FindElement(buttonBuscarCoches).Click();
        }



        public bool CheckListaDeCoches(List<string[]> expectedCoches)
        {
            return CheckBodyTable(expectedCoches, tablaCoches);
        }



        public void AñadirCocheAlCarroAlquiler(string cocheModelo)
        {
            WaitForBeingClickable(By.Id("cocheParaAlquilar_" + cocheModelo));

            _driver.FindElement(By.Id("cocheParaAlquilar_" + cocheModelo)).Click();
        }


        public void RemoveCocheDelCarroAlquiler(string cocheModelo)
        {
            WaitForBeingClickable(By.Id("removerAlquiler_" + cocheModelo));

            _driver.FindElement(By.Id("removerAlquiler_" + cocheModelo)).Click();
        }

        public bool AlquilerNoDisponible()
        {
            return _driver.FindElement(buttonAlquilarCoches).Displayed == false;

        }












        public void AlquilarCoches()
        {
            WaitForBeingClickable(buttonAlquilarCoches);
            _driver.FindElement(buttonAlquilarCoches).Click();

        }

        public bool ComprobarMensajeErrorCocheNoDisponible(string error)
        {
            return _driver.PageSource.Contains(error);
        }

        public bool ComprobarMensajeError(string error)
        {
            return _driver.PageSource.Contains(error);
        }

    }
}
