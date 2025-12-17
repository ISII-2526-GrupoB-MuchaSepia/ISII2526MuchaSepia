using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Alquiler
{
    internal class DetalleAlquiler_PO : PageObject
    {
        public DetalleAlquiler_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        public bool CheckAlquilerDetalle(
     string nombre,
     string apellido,
     string concesionarioEntrega,
     string metodopago,
     
     string precioTotal)
        {
            WaitForBeingVisible(By.Id("Nombre"));

            bool result = true;

            result &= _driver.FindElement(By.Id("Nombre")).Text.Contains(nombre);
            result &= _driver.FindElement(By.Id("Apellido")).Text.Contains(apellido);
            result &= _driver.FindElement(By.Id("ConcesionarioEntrega")).Text.Contains(concesionarioEntrega);

            var metodoPagoText = _driver.FindElement(By.Id("MetodoPago")).Text;
            result &= metodoPagoText.ToLower().Contains(metodopago.ToLower());


            var precioText = _driver.FindElement(By.Id("PrecioTotal")).Text;
            result &= precioText.Contains("€");




            return result;
        }



        public bool CheckListaDeCoches(List<string[]> expectedAlquilerItems)
        {
            return CheckBodyTable(expectedAlquilerItems, By.Id("CochesAlquilados"));
        }
    }

}