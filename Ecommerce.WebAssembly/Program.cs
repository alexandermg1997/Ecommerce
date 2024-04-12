using Ecommerce.WebAssembly;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

// Para trabajar con el LocalStorage del navegador en Blazor 
using Blazored.LocalStorage;
// Para trabajar con el Toast de Blazor 
using Blazored.Toast;

using Ecommerce.WebAssembly.Servicios.Contrato;
using Ecommerce.WebAssembly.Servicios.Implementacion;

// Para los permisos de usuarios
using Microsoft.AspNetCore.Components.Authorization;
using Ecommerce.WebAssembly.Extensiones;

// Para trabajar con la libreria SweetAlert2 (es como SweetAlert[swal])
using CurrieTechnologies.Razor.SweetAlert2;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5106/api/") });

// Agregando el servicio de Blazored.LocalStorage
builder.Services.AddBlazoredLocalStorage();
// Agregando el servicio de Blazored.Toast
builder.Services.AddBlazoredToast();

// Habilitando los servicios de la capa Ecommerce.WebAssembly para que puedan ser utilizado
builder.Services.AddScoped<IUsuarioServicio, UsuarioServicio>();
builder.Services.AddScoped<ICategoriaServicio, CategoriaServicio>();
builder.Services.AddScoped<IProductoServicio, ProductoServicio>();
builder.Services.AddScoped<IVentaServicio, VentaServicio>();
builder.Services.AddScoped<IDashboardServicio, DashboardServicio>();
builder.Services.AddScoped<ICarritoServicio, CarritoServicio>();

// Agregando el servicio de SweetAlert2
builder.Services.AddSweetAlert2();

// Agregando el servicio de autenticacion y declarando las implementaciones
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AutenticacionExtension>();

await builder.Build().RunAsync();
