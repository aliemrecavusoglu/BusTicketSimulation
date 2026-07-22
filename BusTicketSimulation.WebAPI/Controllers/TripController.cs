using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusTicketSimulation.Core.Entities;
using BusTicketSimulation.Core.Interfaces;
using BusTicketSimulation.Core.DTOs;
using AutoMapper;

namespace BusTicketSimulation.WebAPI.Controllers
{
    [Route("api/[controller]")]     //kontrolcüye erişilecek internet adresi
    [ApiController]
    public class TripController : ControllerBase    
    {
        private readonly ITripRepository tripRepository;
        private readonly IMapper mapper;

        public TripController(ITripRepository tRepository, IMapper htmlMapper)
        {
            tripRepository = tRepository;
            mapper = htmlMapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var trips = await tripRepository.GetAllAsync();
            var result = mapper.Map<IEnumerable<TripResultDto>>(trips); //Ham veritabanı modellerini Vue tarafının anlayacağı TripResultDto listesine dönüştürüyoruz
            return Ok(result);   //Http 200
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTrips([FromQuery] string from, [FromQuery] string to)
        {
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
                return BadRequest("Kalkış ve varış yerleri boş bırakılamaz❗");   //Http 400 hatası döner

            var trips = await tripRepository.GetTripsByRouteAsync(from, to);
            var result = mapper.Map<IEnumerable<TripResultDto>>(trips);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TripCreateDto dto)
        {
            var trip = mapper.Map<Trip>(dto);
            await tripRepository.AddAsync(trip);
            await tripRepository.SaveChangesAsync();

            return Ok("Sefer başarıyla oluşturuldu ✅");   // Http 200
        }

        //Toplu bilet alma
        [HttpPost("buy-tickets-bulk")]
        public async Task<IActionResult> BuyTicketsBulk([FromBody] BulkTicketDto dto)
        {
            var trip = await tripRepository.GetByIdAsync(dto.TripId);
            if (trip == null) return NotFound("Sefer bulunamadı❗");

            //Sadece önceden veritabanına kaydedilmiş (kesin satılmış) koltukları çekiyoruz
            var dbSoldSeats = trip.SoldSeats.ToList();

            //Sepetteki biletleri tek tek kontrol et
            foreach (var ticketDto in dto.Tickets)
            {
                //Çift rezervasyon kontrolü
                //Eğer bu koltuk numarası veritabanından çektiğimiz dbSoldSeats listesinde zaten varsa direkt engelle
                var alreadySold = dbSoldSeats.Any(s => s.SeatNumber == ticketDto.SeatNumber);
                if (alreadySold)
                {
                    return BadRequest($"{ticketDto.SeatNumber} numaralı koltuk zaten başka bir yolcu tarafından satın alınmış❗");
                }

                //Gelen veriyi temizle, büyük harfe çevir
                string busType = trip.Bus?.BusType?.Replace(" ", "").ToUpper() ?? "2+2";

                //Eğer gelen veri "2+2" veya "2+1" değilse, sistemi doğrudan varsayılana ("2+2") eşitle!
                if (busType != "2+2" && busType != "2+1")
                {
                    busType = "2+2";
                }

                //Yan koltuk numarasını hesapla
                int neighborSeatNumber = 0;

                if (busType == "2+2")
                {
                    neighborSeatNumber = (ticketDto.SeatNumber % 2 == 1) ? ticketDto.SeatNumber + 1 : ticketDto.SeatNumber - 1;
                }
                else if(busType == "2+1")
                {
                    if(ticketDto.SeatNumber % 3 == 1)
                    {
                        neighborSeatNumber = 0;
                    }
                    else
                    {
                        neighborSeatNumber = (ticketDto.SeatNumber % 3 == 2) ? ticketDto.SeatNumber + 1 : ticketDto.SeatNumber - 1;
                    }
                }

                //Eğer geçerli bir yan koltuk numarası belirlendiyse
                if(neighborSeatNumber > 0)
                {
                    //Yan koltuğu SADECE veritabanındaki (önceden alınmış) koltuklar arasında arıyoruz!
                    //Aynı sepette gelen diğer biletleri bu filtreye dahil etmiyoruz
                    var dbNeighborSeat = dbSoldSeats.FirstOrDefault(s => s.SeatNumber == neighborSeatNumber);

                    // Eğer yan koltuk önceden veritabanına kaydedilmişse VE cinsiyeti şu an alınandan farklıysa:
                    if (dbNeighborSeat != null && dbNeighborSeat.Gender != ticketDto.Gender)
                    {
                        return BadRequest($"{neighborSeatNumber} numaralı koltuk daha önce farklı bir cinsiyet ({dbNeighborSeat.Gender}) tarafından satın alındığı için, yanındaki {ticketDto.SeatNumber} numaralı koltuğu bu cinsiyetle satın alamazsınız❗");
                    }
                }    
            }

            // Eğer tüm sepetteki biletler yukarıdaki "önceden alınmış yabancı koltuk" testinden geçtiyse, veritabanına kaydet:
            foreach (var ticketDto in dto.Tickets)
            {
                var newSoldSeat = new SoldSeat
                {
                    TripId = dto.TripId,
                    SeatNumber = ticketDto.SeatNumber,
                    Gender = ticketDto.Gender
                };
                await tripRepository.AddSoldSeatAsync(newSoldSeat);
            }

            await tripRepository.SaveChangesAsync();
            return Ok(new { Message = "Seçtiğiniz tüm biletler başarıyla onaylandı ve satın alındı. 🎫", NewSeats = dto.Tickets });
        }
    }
}
