using BusTicketSimulation.Core.Interfaces;
using BusTicketSimulation.Core.Entities;
using BusTicketSimulation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTicketSimulation.Infrastructure.Repositories
{
    public class BusRepository : IBusRepository
    {
        private readonly AppDbContext _context;

        //Dependency Injection (Bağımlılık Enjeksiyonu) ile AppDbContext nesnesini alıyoruz
        public BusRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Bus?> GetByIdAsync(Guid id)
        {
            return await _context.Buses.FindAsync(id);
        }
        public async Task<IEnumerable<Bus>> GetAllAsync()
        {
            return await _context.Buses.ToListAsync();
        }
        public async Task AddAsync(Bus bus)
        {
            await _context.Buses.AddAsync(bus);
        }
        public void Delete(Bus bus)
        {
            _context.Buses.Remove(bus);
        }
        public void Update(Bus bus)
        {
            _context.Buses.Update(bus);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
