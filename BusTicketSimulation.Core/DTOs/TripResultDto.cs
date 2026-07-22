using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTicketSimulation.Core.DTOs
{
    public class TripResultDto
    {
        public Guid Id { get; set; }
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public decimal Price { get; set; }
        public int SeatCount { get; set; }
        public string BusType { get; set; }
        public string BusPlateNumber { get; set; } = string.Empty;
        //Sefer listelerken istemciye tüm otobüs nesnesini göndermek yerine, sadece o seferi yapacak
        //otobüsün plakasını (BusPlateNumber) dönüyoruz. Bu hem veri boyutunu küçültür hem de şık bir çıktı sağlar

        //Ön yüze dolu koltukların listesini ve cinsiyet bilgilerini gönderir
        public List<SoldSeatResponseDto> SoldSeats { get; set; } = new List<SoldSeatResponseDto>();
    }
}
