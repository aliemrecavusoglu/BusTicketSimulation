using BusTicketSimulation.Core.DTOs;
using BusTicketSimulation.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTicketSimulation.Core.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> UserExistsAsync(string email);
        Task<User> RegisterAsync(UserRegisterDto dto);
        Task<string?> LoginAsync(UserLoginDto dto);     //Başarılıysa JWT Token string'i döner, başarısızsa null
    }
}
