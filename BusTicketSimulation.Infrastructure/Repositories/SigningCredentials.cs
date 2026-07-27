using Microsoft.IdentityModel.Tokens;

namespace BusTicketSimulation.Infrastructure.Repositories
{
    internal class SigningCredentials
    {
        private SymetricSecurityKey key;
        private object hmacSha512Signature;
        private SymmetricSecurityKey key1;

        public SigningCredentials(SymetricSecurityKey key, object hmacSha512Signature)
        {
            this.key = key;
            this.hmacSha512Signature = hmacSha512Signature;
        }

        public SigningCredentials(SymmetricSecurityKey key1, object hmacSha512Signature)
        {
            this.key1 = key1;
            this.hmacSha512Signature = hmacSha512Signature;
        }
    }
}