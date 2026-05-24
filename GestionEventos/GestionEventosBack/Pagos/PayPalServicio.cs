using Microsoft.Extensions.Configuration;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GestionEventosBack.Pagos
{
    public class PayPalServicio
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public PayPalServicio(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        // Crea el cliente de PayPal con las credenciales del appsettings
        private async Task<string> ObtenerToken()
        {
            var clientId = _config["PayPal:ClientId"];
            var clientSecret = _config["PayPal:ClientSecret"];

            // Codifica las credenciales en Base64
            var credenciales = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credenciales);

            var contenido = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            var respuesta = await _httpClient.PostAsync(
                "https://api.sandbox.paypal.com/v1/oauth2/token", contenido);

            var json = await respuesta.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<JsonElement>(json);

            return data.GetProperty("access_token").GetString()!;

        }

        // Crea una orden de pago en PayPal
        // Recibe el monto y las URLs de retorno
        public async Task<(string orderId, string approvalUrl)> CrearOrden(decimal monto, string urlExito, string urlCancelacion)
        {
            var token = await ObtenerToken();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var orden = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                    new
                    {
                        amount = new
                        {
                            currency_code = "USD",
                            value = monto.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
                        }
                    }
                },
                application_context = new
                {
                    brand_name = "Gestión de Eventos",
                    user_action = "PAY_NOW",
                    return_url = urlExito,
                    cancel_url = urlCancelacion
                }
            };

            var contenido = new StringContent(
                JsonSerializer.Serialize(orden),
                Encoding.UTF8,
                "application/json"
                );
            var respuesta = await _httpClient.PostAsync(
                "https://api.sandbox.paypal.com/v2/checkout/orders", contenido);

            var json = await respuesta.Content.ReadAsStringAsync();

            //Console.WriteLine($"Respuesta PayPal: {json}");
            var data = JsonSerializer.Deserialize<JsonElement>(json);

            var orderId = data.GetProperty("id").GetString()!;

            // Busca la URL de aprobación en los links
            var approvalUrl = "";
            foreach (var link in data.GetProperty("links").EnumerateArray())
            {
                if (link.GetProperty("rel").GetString() == "approve")
                {
                    approvalUrl = link.GetProperty("href").GetString()!;
                    break;
                }
            }

            return (orderId, approvalUrl);
        }

        // Captura el pago después de que el usuario aprueba en PayPal
        public async Task<bool> CapturarPago(string orderId)
        {
            var token = await ObtenerToken();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var respuesta = await _httpClient.PostAsync(
                $"https://api.sandbox.paypal.com/v2/checkout/orders/{orderId}/capture",
                new StringContent("{}", Encoding.UTF8, "application/json"));

            var json = await respuesta.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<JsonElement>(json);

            return data.GetProperty("status").GetString() == "COMPLETED";
        }
    }
}
