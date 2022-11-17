using Financeiro.Boleto.Domain.Interfaces.ApiServices;
using Financeiro.Boleto.Domain.Interfaces.Repositories;
using Financeiro.Boleto.Domain.Interfaces.Services;
using Financeiro.Boleto.Domain.Services.ApiServices;
using Financeiro.Boleto.Infra.Context;
using Financeiro.Boleto.Infra.Repositories;
using Financeiro.Boleto.Infra.Services.ApiServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Net.Http.Headers;
using System.Text;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BoletoDb>(options => options.UseSqlServer("Data Source=localhost;Initial Catalog=ApiBoleto;Persist Security Info=True;Encrypt=False;User ID=sa;Password=feherr")); // TODO: colocar no appsettings

builder.Services
            .AddScoped<IBoletoService, BoletoService>()
            .AddScoped<IBoletoRepository, BoletoRepository>()
            .AddScoped<IParametroBoletoRepository, ParametroBoletoRepository>();

builder.Services.AddHttpClient<IGeradorBoletoApiService, BoletoCloudApiService>(client =>
{
    var token = $"{builder.Configuration["ApiBoletoCloud:ApiKey"]}:token";

    client.BaseAddress = new Uri(builder.Configuration["ApiBoletoCloud:BaseAddress"]);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(token)));
});

builder.Services.AddControllers();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.Authority = "http://localhost:8080/realms/myrealm";
    o.Audience = "account";
    o.RequireHttpsMetadata = false;

    o.Events = new JwtBearerEvents()
    {
        OnAuthenticationFailed = c =>
        {
            c.NoResult();

            c.Response.StatusCode = 500;
            c.Response.ContentType = "text/plain";
            //if (Environment.IsDevelopment())
            //{
            //return c.Response.WriteAsync(c.Exception.ToString());
            Console.WriteLine(c.Exception.ToString());
            return Task.FromResult(string.Empty);
            //}
            //return c.Response.WriteAsync("An error occured processing your authentication.");
        }
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();