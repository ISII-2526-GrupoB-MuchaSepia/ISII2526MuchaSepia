using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_Purchase
{
    public class DetallesCompra_PO : PageObject
    {
        public DetallesCompra_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {

        }

        public bool CheckPurchaseDetail(string nombreApellidos, string concesionarioEntrega, DateTime fechaCompra, string precioTotal)
        {
            Thread.Sleep(500);
            WaitForBeingVisible(By.Id("PrecioTotal"));
            bool result = true;
            result = result && _driver.FindElement(By.Id("NombreApellidos")).Text.Contains(nombreApellidos);
            result = result && _driver.FindElement(By.Id("ConcesionarioEntrga")).Text.Contains(concesionarioEntrega);
            result = result && _driver.FindElement(By.Id("PrecioTotal")).Text.Contains(precioTotal);

            var fechacompraActual = DateTime.Parse(_driver.FindElement(By.Id("FechaCompra")).Text);
            result = result && ((fechacompraActual - fechaCompra) < new TimeSpan(0, 1, 0));

            return result;
        }

        public bool CheckListOfPurchase(List<string[]> expectedRentalItems)
        {
            return CheckBodyTable(expectedRentalItems, By.Id("CochesComprados"));
        }
    }
}