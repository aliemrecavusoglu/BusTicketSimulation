using System.Security.Claims;

namespace BusTicketSimulation.Infrastructure.Repositories
{
    internal class SecurityTokenDescriptor
    {
        public ClaimsIdentity Subject { get; set; }
        public DateTime Expires { get; set; }
        public object SigningCredentials { get; set; }
    }
}