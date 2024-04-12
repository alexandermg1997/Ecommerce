using Ecommerce.Repositorio.DBContext;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Repositorio.Contrato;
using Ecommerce.Repositorio.Implementacion;
using Ecommerce.Servicio.Contrato;
using Ecommerce.Servicio.Implementacion;
using Ecommerce.Utilidades;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Para inyectar el _dbcontext en las clases (Conectar con la base de datos)
builder.Services.AddDbContext<DbecommerceContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL"));
});

// Habilitando los servicios de la capa Ecommerce.Repositorio para que puedan ser utilizado
builder.Services.AddTransient(typeof(IGenericoRepositorio<>), typeof(GenericoRepositorio<>));
builder.Services.AddScoped<IVentaRepositorio, VentaRepositorio>();

// Para agregar automapper(Conectando los modelos con los DTO)
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Habilitando los servicios de la capa Ecommerce.Servicio para que puedan ser utilizado
builder.Services.AddScoped<IUsuarioServicio, UsuarioServicio>();
builder.Services.AddScoped<ICategoriaServicio, CategoriaServicio>();
builder.Services.AddScoped<IProductoServicio, ProductoServicio>();
builder.Services.AddScoped<IVentaServicio, VentaServicio>();
builder.Services.AddScoped<IDashboardServicio, DashboardServicio>();

// Creando una nueva política de comunicación 
builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NuevaPolitica");

app.UseAuthorization();

app.MapControllers();

app.Run();
