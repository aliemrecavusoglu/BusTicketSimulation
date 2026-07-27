namespace BusTicketSimulation.Infrastructure.Repositories
{
    internal class SymetricSecurityKey
    {
        private byte[] bytes;

        public SymetricSecurityKey(byte[] bytes)
        {
            this.bytes = bytes;
        }
    }
}