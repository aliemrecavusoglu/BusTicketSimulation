using BusTicketSimulation.Core.Interfaces;
using BusTicketSimulation.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BusTicketSimulation.WebAPI;
using BusTicketSimulation.WebAPI.Middlewares;
using System.Runtime.ConstrainedExecution;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BusTicketSimulation.Infrastructure.Data.AppDbContext>();

//Interface ile veritabaný sýnýfýný birbirine baðlar, her HTTP isteðinde yeni bir nesne üretir
builder.Services.AddScoped<IBusRepository, BusRepository>();
builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


//JWT Authentication Servisi Kaydý
var secretKey = builder.Configuration["Jwt:SecretKey"] ?? "BusTicketSimulationSankiKriptoluBirBankaAnahtariGibiCokGizliCokUzunSifre1234567890!";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

//AutoMapper    
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "BusTicketSimulation.WebAPI", Version = "v1" });

    // Swagger ekranýna JWT yetkilendirme (Authorize) butonunu ekliyoruz
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

//Vue.js' den gelen isteklere izin veriyoruz
builder.Services.AddCors(options =>
{
    options.AddPolicy("VueCorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("VueCorsPolicy");   //Cors izin politikasýný aktif eder

app.UseAuthentication(); 

app.UseAuthorization();  

app.UseAuthorization();

app.MapControllers();

app.UseDeveloperExceptionPage(); // ?? Hatanýn ne olduðunu tarayýcýya/konsola detaylý bassýn

app.Run();
