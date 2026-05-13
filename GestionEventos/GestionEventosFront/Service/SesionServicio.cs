using Microsoft.JSInterop;

namespace GestionEventosFront.Service
{
    // Este servicio maneja los datos del usuario en sesión
    // Usa sessionStorage del navegador para guardar y leer los datos
    public class SesionServicio
    {
        private readonly IJSRuntime _js;

        public SesionServicio(IJSRuntime js)
        {
            _js = js;
        }

        // Obtiene el nombre del usuario en sesión
        public async Task<string> ObtenerNombre()
        {
            var nombre = await _js.InvokeAsync<string>("sessionStorage.getItem", "usuarioNombre");
            return nombre ?? "Usuario";
        }

        public async Task<string> ObtenerEmail()
        {
            var email = await _js.InvokeAsync<string>("sessionStorage.getItem", "usuarioEmail");
            return email ?? "";
        }

        public async Task<string> ObtenerRol()
        {
            var rol = await _js.InvokeAsync<string>("sessionStorage.getItem", "usuarioRol");
            return rol ?? "";
        }

        public async Task<int> ObtenerId()
        {
            var id = await _js.InvokeAsync<string>("sessionStorage.getItem", "usuarioId");
            
            return Convert.ToInt32(id);
        }

        public async Task CerrarSesion()
        {
            await _js.InvokeVoidAsync("sessionStorage.removeItem", "usuarioNombre");
            await _js.InvokeVoidAsync("sessionStorage.removeItem", "usuarioEmail");
            await _js.InvokeVoidAsync("sessionStorage.removeItem", "usuarioRol");
            await _js.InvokeVoidAsync("sessionStorage.removeItem", "usuarioId");
        }
    }
}
