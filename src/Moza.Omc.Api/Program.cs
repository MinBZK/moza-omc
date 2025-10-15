using System.Text;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Moza.Omc.Api.Configuration;
using Moza.Omc.Api.Data;
using Moza.Omc.Api.Data.Repositories;
using Moza.Omc.Api.Services;
using Moza.Omc.Api.Services.Clients;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel(options => options.AddServerHeader = false);
builder.Host.UseDefaultServiceProvider(options => options.ValidateScopes = true);

var authSection = builder.Configuration.GetSection("Authentication");
var authSettings = authSection.Get<AuthenticationSettings>() ?? throw new InvalidConfigurationException("Authentication settings not found");

// Authentication with JWT Bearer, note hier moet een echte secret key komen
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    var secretKey = authSettings.SecretKey;
    var key = Encoding.ASCII.GetBytes(secretKey!);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.Configure<AuthenticationSettings>(authSection);
builder.Services.Configure<NotifyNLSettings>(builder.Configuration.GetSection("NotifyNL"));
builder.Services.AddHttpClient<NotifyNLClient>();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "OMC Service API", Version = "v1" });
    options.SupportNonNullableReferenceTypes();
    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Description = "Paste your JWT token below. 'Bearer' will be added automatically.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<OmcDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("OmcDb");

    options.UseNpgsql(connectionString, innerOptions =>
    {
        innerOptions.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
        innerOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    });
});

builder.Services.AddHttpClient<ProfielServiceClient>(client =>
{
    client.BaseAddress = new Uri("https://profiel-service.logius-moz-poc.test3.s15m.nl/");
});

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<LogiusBrievendienstClient>();
builder.Services.AddScoped<NotificatieService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<OndernemingRepository>();
builder.Services.AddScoped<NotificatieRepository>();
builder.Services.AddScoped<NotificatieEventRepository>();
builder.Services.AddControllers();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<OmcDbContext>();
db.Database.Migrate();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.Run();