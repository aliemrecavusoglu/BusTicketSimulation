using BusTicketSimulation.Core.Interfaces;
using BusTicketSimulation.Infrastructure.Repositories;
using BusTicketSimulation.WebAPI;
using BusTicketSimulation.WebAPI.Middlewares;
using System.Runtime.ConstrainedExecution;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BusTicketSimulation.Infrastructure.Data.AppDbContext>();

//Interface ile veritabaný sýnýfýný birbirine baðlar, her HTTP isteðinde yeni bir nesne üretir
builder.Services.AddScoped<IBusRepository, BusRepository>();
builder.Services.AddScoped<ITripRepository, TripRepository>();

//AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
