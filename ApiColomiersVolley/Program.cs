using ApiColomiersVolley;
using ApiColomiersVolley.Middlewares;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InitLocator(builder.Configuration);
builder.Services.AddControllers()

    .AddJsonOptions(jsonOptions =>
    {
        jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
        jsonOptions.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
if (builder.Configuration.GetSection("FeatureActivation")?.GetValue<bool?>("enableSwagger") == true)
{
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "ApiColomiersVolley", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                Reference = new OpenApiReference
                    {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",

                    In = ParameterLocation.Header,

                },
                new List<string>()
            }
        });
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath, true);
        c.CustomSchemaIds(x => x.FullName);

    });
}
//builder.Services.AddHealthChecks().AddSqlServer(
//    connectionString: builder.Configuration.GetConnectionString("EasyFichiersContext"),
//    healthQuery: "SELECT 1",
//    name: "Sql Serveur",
//    failureStatus: HealthStatus.Unhealthy);

var app = builder.Build();
app.UseStaticFiles();
app.UseCorsMiddleware();
if (builder.Configuration.GetSection("FeatureActivation")?.GetValue<bool?>("enableSwagger") == true)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiColomiersVolley v1");
        c.RoutePrefix = "doc";
        c.DisplayRequestDuration();
    });
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
public partial class ApiProgram { }
