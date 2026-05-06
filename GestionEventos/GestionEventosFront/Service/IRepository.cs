namespace GestionEventosFront.Service
{
    public interface IRepository
    {
        // GET - para obtener una lista de datos
        // Ejemplo: obtener todos los eventos
        // T es genérico — puede ser List<Evento>, List<Usuario>, etc.
        Task<T> GetAsync<T>(string url);

        // GET por ID - para obtener un dato específico
        // Ejemplo: obtener el evento con ID 1
        Task<T> GetByIdAsync<T>(string url, int id);

        // POST - para crear un nuevo dato
        // Ejemplo: crear un nuevo evento
        Task<object> PostAsync<T>(string url, T model);

        // PUT - para actualizar un dato existente
        // Ejemplo: actualizar el evento con ID 1
        Task<object> PutAsync<T>(string url, T model);

        // DELETE - para eliminar un dato
        // Ejemplo: eliminar el evento con ID 1
        Task<object> DeleteAsync(string url); 
    }
}
