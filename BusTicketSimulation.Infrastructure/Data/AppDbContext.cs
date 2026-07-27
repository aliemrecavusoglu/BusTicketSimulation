using BusTicketSimulation.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusTicketSimulation.Infrastructure.Data;

public class AppDbContext : DbContext   //EF core'un ana yönetim sınıfından miras aldık
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)    //constructor metot: API projesindeki Program.cs üzerinden geçilecek
    {
    }
    public AppDbContext()   //constructor metot: EF Core Migration (göç) araçlarının boş constructor ihtiyacını karşılar
    {
    }

    //veritabanında oluşacak tablolarımız
    public DbSet<Bus> Buses { get; set; }
    public DbSet<Trip> Trips { get; set; }
    public DbSet<SoldSeat> SoldSeats { get; set; }
    public DbSet<User> Users { get; set; } 

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)   //EF core'un veritabanına bağlanırken çalıştırdığı hazır metot
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=BusTicketDb;Username=postgres;Password=Ali1234");
    }
}