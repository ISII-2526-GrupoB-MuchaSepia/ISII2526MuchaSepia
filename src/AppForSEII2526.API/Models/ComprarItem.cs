namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(CocheId), nameof(ComprarId))]
    public class ComprarItem
    {
        public ComprarItem() { }

        // Permite crear un nuevo ítem de compra asociando un coche y una compra concretos.
        // guarda el precio actual del coche y la cantidad comprada.
        public ComprarItem(Coche coche, Comprar comprar, int cantidad)
        {
            Coche = coche;
            CocheId = coche.Id;
            Comprar = comprar;
            ComprarId = comprar.Id;
            Precio = coche.PrecioCompra;
            Cantidad = cantidad;
            Descripcion = coche.Descripcion;
        }

        public Coche Coche { get; set; }
        public int CocheId { get; set; }
        public string? Descripcion { get; set; }
        public Comprar Comprar { get; set; }
        public int ComprarId { get; set; }

        [Precision(10, 2)]
        public double Precio { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 1")]
        public int Cantidad { get; set; }
    }

}