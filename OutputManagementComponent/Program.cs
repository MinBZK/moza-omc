using System.Text;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using OutputManagementComponent.data;
using OutputManagementComponent.Repositories;
using OutputManagementComponent.Services;
using OutputManagementComponent.Services.Clients;

var builder = WebApplication.CreateBuilder(args);

// Authentication with JWT Bearer, note hier moet een echte secret key komen
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secretKey = builder.Configuration.GetValue<string>("SecretKey");
        var key = Encoding.ASCII.GetBytes(secretKey);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();
builder.Services.Configure<NotifyNLSettings>(builder.Configuration.GetSection("NotifyNL"));
builder.Services.AddHttpClient<NotifyNLClient>();


// Configure Swagger with Auth
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "OMC Service API", Version = "v1" });
    options.SupportNonNullableReferenceTypes();
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
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
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddDbContext<OMCDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");

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

var db = scope.ServiceProvider.GetRequiredService<OMCDbContext>();
db.Database.Migrate();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
