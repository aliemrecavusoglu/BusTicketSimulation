using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTicketSimulation.Core.DTOs
{
    //Bu sınıfın içinde ne Id var ne de CreatedDate. Sadece yeni bir otobüs oluşturulurken dışarıdan alınması şart olan verileri barındırıyor.
    public class BusCreateDto
    {
        public string PlateNumber { get; set; } = string.Empty;
        public int SeatCount { get; set; }
        public string BusType { get; set; }
    }
}
