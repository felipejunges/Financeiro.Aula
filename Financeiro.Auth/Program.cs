using Financeiro.Auth.Context;
using Financeiro.Auth.Entities;
using Financeiro.Auth.Interfaces.Repositories;
using Financeiro.Auth.Interfaces.Services;
using Financeiro.Auth.Repositories;
using Financeiro.Auth.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;

Assembly domainAssembly = AppDomain.CurrentDomain.Load("Financeiro.Auth");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthDb>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(domainAssembly);
});

builder.Services
    .AddScoped<IAcessoRepository, AcessoRepository>()
    .AddScoped<IUsuarioRepository, UsuarioRepository>()
    .AddScoped<ITokenService, TokenService>()
    .AddScoped<IAcessoService, AcessoService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AuthDb>();
    db.Database.EnsureCreated();

    if (!db.Usuarios.Any())
    {
        db.Usuarios.Add(new Usuario(0, "Felipe", "felipejunges@yahoo.com.br", "felipe123", "Admin"));

        db.SaveChanges();
    }
}

app.Run();
