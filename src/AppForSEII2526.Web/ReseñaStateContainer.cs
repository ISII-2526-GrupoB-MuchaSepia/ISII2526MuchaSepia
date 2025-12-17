using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class ReseñarStateContainer
    {
        // Creamos una instancia de la DTO de creación cuando se instancia el contenedor
        public CreacionesReseñarDTO Reseña { get; private set; } = new CreacionesReseñarDTO()
        {
            ReseñarItems = new List<ReseñarItemDTO>()
            // Inicializamos la lista vacía para evitar NullReferenceException
        };

        public event Action? OnChange;

        // Método para notificar cambios a la interfaz (aunque en este ejemplo básico no se llama explícitamente, es buena práctica tenerlo)
        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AñadirCocheParaReseñar(CocheParaReseñarDTO coche)
        {
            // Antes de añadir un coche, comprobamos si ya ha sido añadido a la lista
            // Usamos el ID del coche como identificador único
            if (!Reseña.ReseñarItems.Any(ri => ri.CocheId == coche.Id))
            {
                // Lo añadimos si no está en la lista
                Reseña.ReseñarItems.Add(new ReseñarItemDTO()
                {
                    CocheId = coche.Id,
                    // Mapeamos el nombre para mostrarlo luego en el resumen
                    CocheNombre = coche.Modelo ?? coche.ClaseCoche,
                    // Inicializamos valores por defecto para la reseña
                    Calificacion = 5,
                    Descripcion = ""
                });
            }
        }

        // Para eliminar coches de la lista de reseñas seleccionadas
        public void BorrarCocheDeReseña(ReseñarItemDTO item)
        {
            Reseña.ReseñarItems.Remove(item);
        }

        // Eliminamos todos los coches de la lista (Vaciar carrito)
        public void VaciarListaReseñas()
        {
            Reseña.ReseñarItems.Clear();
        }

        // Ya hemos terminado el proceso de reseñar (POST exitoso), así que reseteamos el estado
        public void ReseñaProcesada()
        {
            // Creamos un nuevo objeto limpio
            Reseña = new CreacionesReseñarDTO()
            {
                ReseñarItems = new List<ReseñarItemDTO>()
            };
        }
    }
}