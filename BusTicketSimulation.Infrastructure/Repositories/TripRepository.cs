using BusTicketSimulation.Core.Interfaces;
using BusTicketSimulation.Core.Entities;
using BusTicketSimulation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BusTicketSimulation.Infrastructure.Repositories;

public class TripRepository : ITripRepository
{
    private readonly AppDbContext _context;

    public TripRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Trip?> GetByIdAsync(Guid id)
    {
        return await _context.Trips
            .Include(t => t.Bus)    //sefer yüklenirken o sefere ait otobüs bilgilerini de beraberinde yükle (Inner Join mantığı)
            .Include(t => t.SoldSeats)  //Sadece bu sefere ait koltukları ilişki üzerinden otomatik filtreleyerek getirir!
            .FirstOrDefaultAsync(t => t.Id == id);
    }
    public async Task<IEnumerable<Trip>> GetAllAsync()
    {
        return await _context.Trips
            .Include(t => t.Bus)
            .Include(t => t.SoldSeats)
            .ToListAsync();
    }
    public async Task<IEnumerable<Trip>> GetTripsByRouteAsync(string from, string to)
    {
        return await _context.Trips
            .Include(t => t.Bus)
            .Include(t => t.SoldSeats)  //Arama sonuçlarında listelenen her seferin kendi koltuklarını içine yükler!
            .Where(t => t.From.ToLower() == from.ToLower() && t.To.ToLower() == to.ToLower())
            .ToListAsync();
    }
    public async Task AddAsync(Trip trip)
    {
        await _context.Trips.AddAsync(trip);
    }
    public void Update(Trip trip)
    {
        _context.Trips.Update(trip);
    }
    public void Delete(Trip trip)
    {
        _context.Trips.Remove(trip);
    }
    public async Task AddSoldSeatAsync(BusTicketSimulation.Core.Entities.SoldSeat soldSeat)
    {
        // Doğrudan DbContext üzerinden SoldSeats tablosuna bağımsız INSERT atıyoruz
        await _context.Set<BusTicketSimulation.Core.Entities.SoldSeat>().AddAsync(soldSeat);
    }
    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}