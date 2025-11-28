namespace AppForSEII2526.API.DTOs.AlquilerDTOs
{
    public class AlquilerItemDTO
    {
        //representar cada coche dentro de un alquiler.
        //El sistema muestra los coches seleccionados, indicando su modelo, fabricante y
        // precio de alquiler y la cantidad de cada coche seleccionado

        public AlquilerItemDTO( int cantidad, double precioAlquiler, string modelo, string fabricante)
        {
            
            Cantidad = cantidad; //unidades alquiladas
            PrecioAlquiler = precioAlquiler;
            Modelo = modelo;
            Fabricante = fabricante; //fabricante del coche
        }

        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [StringLength(20, MinimumLength = 1)]
        public string Modelo { get; set; }



        [StringLength(20, MinimumLength = 1)]
        public string Fabricante { get; set; }

        public double PrecioAlquiler { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is AlquilerItemDTO dTO &&
                    
                   Cantidad == dTO.Cantidad &&
                   PrecioAlquiler == dTO.PrecioAlquiler &&
                   Modelo == dTO.Modelo &&
                   Fabricante == dTO.Fabricante;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine( Cantidad, PrecioAlquiler, Modelo, Fabricante);
        }
    }
}