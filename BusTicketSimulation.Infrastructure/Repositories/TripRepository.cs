using BusTicketSimulation.Core.Interfaces;
using BusTicketSimulation.Core.Entities;
using BusTicketSimulation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BusTicketSimulation.Infrastructure.Repositories;

public class TripRepository : ITripRepository
{
    private readonly AppDbContext dbContext;

    public TripRepository(AppDbContext context)
    {
        dbContext = context;
    }
    public async Task<Trip?> GetByIdAsync(Guid id)
    {
        return await dbContext.Trips
            .Include(t => t.Bus)    //sefer yüklenirken o sefere ait otobüs bilgilerini de beraberinde yükle (Inner Join mantığı)
            .Include(t => t.SoldSeats)  //Sadece bu sefere ait koltukları ilişki üzerinden otomatik filtreleyerek getirir!
            .FirstOrDefaultAsync(t => t.Id == id);
    }
    public async Task<IEnumerable<Trip>> GetAllAsync()
    {
        return await dbContext.Trips
            .Include(t => t.Bus)
            .Include(t => t.SoldSeats)
            .ToListAsync();
    }
    public async Task<IEnumerable<Trip>> GetTripsByRouteAsync(string from, string to)
    {
        return await dbContext.Trips
            .Include(t => t.Bus)
            .Include(t => t.SoldSeats)  //Arama sonuçlarında listelenen her seferin kendi koltuklarını içine yükler!
            .Where(t => t.From.ToLower() == from.ToLower() && t.To.ToLower() == to.ToLower())
            .ToListAsync();
    }
    public async Task AddAsync(Trip trip)
    {
        await dbContext.Trips.AddAsync(trip);
    }
    public void Update(Trip trip)
    {
        dbContext.Trips.Update(trip);
    }
    public void Delete(Trip trip)
    {
        dbContext.Trips.Remove(trip);
    }
    public async Task AddSoldSeatAsync(BusTicketSimulation.Core.Entities.SoldSeat soldSeat)
    {
        // Doğrudan DbContext üzerinden SoldSeats tablosuna bağımsız INSERT atıyoruz
        await dbContext.Set<BusTicketSimulation.Core.Entities.SoldSeat>().AddAsync(soldSeat);
    }
    public async Task<bool> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync() > 0;
    }
}