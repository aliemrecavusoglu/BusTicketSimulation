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
        Task<bool> UserExistsAsync(string email);   //Veritabanında belirtilen e-posta adresiyle kayıtlı başka bir kullanıcı var mı?
        Task<User> RegisterAsync(UserRegisterDto dto);      //Yeni kullanıcı kaydı oluşturur
        Task<string?> LoginAsync(UserLoginDto dto);     //Başarılıysa JWT Token string'i döner, başarısızsa null
    }
}
