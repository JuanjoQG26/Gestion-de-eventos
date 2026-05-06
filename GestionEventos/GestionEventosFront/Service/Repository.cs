using System.Text;
using System.Text.Json;

namespace GestionEventosFront.Service
{
    // Esta clase IMPLEMENTA la interfaz IRepository
    // Es la que realmente hace las peticiones HTTP a la API
    public class Repository:IRepository
    {
        // HttpClient es el objeto que hace las peticiones HTTP
        private readonly HttpClient _httpClient;

        // Opciones para deserializar el JSON que devuelve la API
        // PropertyNameCaseInsensitive = true significa que no importa
        // si la API devuelve "nombre" o "Nombre" — los reconoce igual
        private JsonSerializerOptions _jsonDefaultOptions => new()
        {
            PropertyNameCaseInsensitive = true,
        };

        // Constructor: recibe el HttpClient por inyección de dependencias
        public Repository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ---- GET - Obtener lista ----
        // Hace una petición GET a la URL indicada y devuelve los datos
        public async Task<T> GetAsync<T>(string url)
        {
            // Hace la petición GET
            var response = await _httpClient.GetAsync(url);

            // Si hay error lanza una excepción automáticamente
            response.EnsureSuccessStatusCode();

            // Lee el contenido de la respuesta como texto
            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(content, _jsonDefaultOptions)!;
        }

        // ---- GET por ID - Obtener uno ----
        // Hace una petición GET a la URL + ID indicado
        public async Task<T> GetByIdAsync<T>(string url, int id)
        {
            // Construye la URL con el ID: ejemplo "api/eventos/1"
            var requestUrl = $"{url}/{id}";

            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode() ;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _jsonDefaultOptions)!;
        }

        // ---- POST - Crear ----
        // Hace una petición POST enviando el objeto como JSON
        public async Task<object> PostAsync<T>(string url, T model)
        {
            //convertir objeto a json
            var content = new StringContent(
                JsonSerializer.Serialize(model),
                Encoding.UTF8,
                "application/json" 
                );
            var response = await _httpClient.PostAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            /*response.EnsureSuccessStatusCode();

            //retorna la respuesta como texto
            return await response.Content.ReadAsStringAsync();*/
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error en PostAsync: {response.StatusCode} - {responseContent}");
                throw new Exception($"Error {response.StatusCode}: {responseContent}");
            }

            return responseContent;
        }

        // ---- PUT - Actualizar ----
        // Hace una petición PUT enviando el objeto actualizado como JSON
        public async Task<object> PutAsync<T>(string url, T model)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(model),
                Encoding.UTF8,
                "application/json"
                );
            var response = await _httpClient.PutAsync(url, content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        // ---- DELETE - Eliminar ----
        // Hace una petición DELETE a la URL indicada
        public async Task<object> DeleteAsync(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
