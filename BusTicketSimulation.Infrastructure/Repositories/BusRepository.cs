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
        private readonly AppDbContext dbContext;

        //Dependency Injection (Bağımlılık Enjeksiyonu) ile AppDbContext nesnesini alıyoruz
        public BusRepository(AppDbContext context)
        {
            dbContext = context;
        }
        public async Task<Bus?> GetByIdAsync(Guid id)
        {
            return await dbContext.Buses.FindAsync(id);
        }
        public async Task<IEnumerable<Bus>> GetAllAsync()
        {
            return await dbContext.Buses.ToListAsync();
        }
        public async Task AddAsync(Bus bus)
        {
            await dbContext.Buses.AddAsync(bus);
        }
        public void Delete(Bus bus)
        {
            dbContext.Buses.Remove(bus);
        }
        public void Update(Bus bus)
        {
            dbContext.Buses.Update(bus);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync() > 0;
        }
    }
}
