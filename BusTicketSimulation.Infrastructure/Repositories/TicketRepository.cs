using BusTicketSimulation.Core.Entities;
using BusTicketSimulation.Core.Interfaces;
using BusTicketSimulation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTicketSimulation.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _context;

        public TicketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SoldSeat>> GetTicketsByUserIdAsync(Guid userId)
        {
            //Kullanıcının biletlerini çekerken sefer (Trip) ve otobüs (Bus) bilgilerini de ınclude ediyoruz
            return await _context.SoldSeats
                .Include(s => s.Trip)
                .ThenInclude(t => t.Bus)
                .Where(s => s.UserId == userId) 
                .OrderByDescending(s => s.Id)
                .ToListAsync();
        }
        public async Task AddSoldSeatAsync(SoldSeat soldSeat)
        {
            await _context.SoldSeats.AddAsync(soldSeat);
        }
        public async Task<SoldSeat?> GetByIdAsync(Guid id)
        {
            return await _context.SoldSeats.FindAsync(id);
        }
        public void DeleteSoldSeat(SoldSeat soldSeat)
        {
            _context.SoldSeats.Remove(soldSeat);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
