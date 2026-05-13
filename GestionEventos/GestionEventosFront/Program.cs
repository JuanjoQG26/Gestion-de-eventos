using GestionEventosFront;
using GestionEventosFront.Service;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configura el HttpClient para que apunte a la API
// Todas las peticiones HTTP del front irán a esta URL base
var urlApi = "http://localhost:5259/";
builder.Services.AddScoped(sp => new HttpClient { 
    BaseAddress = new Uri(urlApi) 
});

// Registra el Repository para que pueda inyectarse en las páginas
// Cuando una página pida IRepository, recibirá Repository
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<GestionEventosFront.Service.SesionServicio>();

await builder.Build().RunAsync();
