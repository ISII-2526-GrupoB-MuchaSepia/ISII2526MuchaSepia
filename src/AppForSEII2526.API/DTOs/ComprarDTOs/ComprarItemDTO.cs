namespace AppForSEII2526.API.DTOs.ComprarDTOs
{
    public class ComprarItemDTO
    {
        public ComprarItemDTO(int cocheID, string modelo, decimal precioCompra, int cantidad)
        {
            CocheID = cocheID;
            PrecioCompra = precioCompra;
            Modelo = modelo;
            Cantidad = cantidad;
        }

        public int CocheID { get; set; }

        public decimal PrecioCompra { get; set; }

        public int Cantidad { get; set; }

        public string Modelo { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ComprarItemDTO dTO &&
                   CocheID == dTO.CocheID &&
                   PrecioCompra == dTO.PrecioCompra &&
                   Cantidad == dTO.Cantidad &&
                   Modelo == dTO.Modelo;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CocheID, PrecioCompra, Cantidad, Modelo);
        }
    }
}
