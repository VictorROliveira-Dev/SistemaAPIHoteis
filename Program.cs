using Microsoft.EntityFrameworkCore;
using SistemaHoteis.Data;
using SistemaHoteis.Repositories;
using SistemaHoteis.Repositories.Interfaces;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(opt =>
{
    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(doc =>
{
    doc.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API para sistema de hotéis",
        Version = "v1",
        Description = "Sistema completo para CRUD de hotéis, incluindo registro de hóspedes e realização de check-in.",
        Contact = new OpenApiContact
        {
            Name = "Victor Oliveira",
            Email = "aleframos62@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/alef-ramos/")
        }
    });

    var xmlFile = "SistemaHoteis.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    doc.IncludeXmlComments(xmlPath);
});

builder.Services.AddEntityFrameworkSqlServer().AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
});

builder.Services.AddScoped<IHotelRepositorio, HotelRepositorio>();
builder.Services.AddScoped<IHospedeRepositorio, HospedeRepositorio>();
builder.Services.AddScoped<ICheckinRepositorio, CheckinRepositorio>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
