using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class CompraStateContainer
    {
       
        public CreacionComprasDTO Compra { get; private set; } = new CreacionComprasDTO()
        {
            ComprarItems = new List<ComprarItemDTO>()
        };

        
        public decimal PrecioTotal
        {
            get
            {
                return Convert.ToDecimal(Compra.ComprarItems.Sum(pi => pi.PrecioCompra));
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();


        public void AddCarToPurchase(CocheParaCompraDTO coche)
        {
            //before adding a car we checked whether it has been already added
            if (!Compra.ComprarItems.Any(pi => pi.Modelo == coche.Modelo))
                //we add it if it is not in the list
                Compra.ComprarItems.Add(new ComprarItemDTO()
                {
                    Modelo = coche.Modelo,
                    PrecioCompra = (float)coche.PrecioCompra,
                    Color = coche.Color,
                    Descripcion= coche.Descripcion
                }
            );
        }

        //to delete cars from the list of selected cars
        public void RemovePurchaseItemToPurchase(ComprarItemDTO item)
        {
            Compra.ComprarItems.Remove(item);

        }

        //we eliminate all the cars from the list
        public void ClearPurchasingCart()
        {
            Compra.ComprarItems.Clear();

        }

        //we have already finished the process of purchase, thus, we create a new Purchase 
        public void PurchaseProcessed()
        {
            //we have finished the purchase process so we create a new object without data
            Compra = new CreacionComprasDTO()
            {
                ComprarItems = new List<ComprarItemDTO>()
            };
        }
    }
}