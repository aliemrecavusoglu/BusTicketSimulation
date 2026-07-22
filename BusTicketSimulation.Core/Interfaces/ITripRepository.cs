using BusTicketSimulation.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTicketSimulation.Core.Interfaces
{
    public interface ITripRepository
    {
        Task<Trip?> GetByIdAsync(Guid id);
        Task<IEnumerable<Trip>> GetAllAsync();  //IEnumerable: verileri liste formatında döndürür
        Task<IEnumerable<Trip>> GetTripsByRouteAsync(string from, string to);
        Task AddAsync(Trip trip);
        void Update(Trip trip);
        void Delete(Trip trip);
        Task<bool> SaveChangesAsync();

        //Sefer esnasında yeni bir bilet/koltuk satışı yapıldığında bu satışı kaydeder
        Task AddSoldSeatAsync(BusTicketSimulation.Core.Entities.SoldSeat soldSeat);
    }
}
