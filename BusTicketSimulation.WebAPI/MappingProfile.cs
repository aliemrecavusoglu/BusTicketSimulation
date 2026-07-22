using BusTicketSimulation.Core.DTOs;
using BusTicketSimulation.Core.Entities;
using AutoMapper;   

namespace BusTicketSimulation.WebAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //BusCreateDto' dan Bus nesnesine dönüşüm
            //Dış dünyadan gelen bilgileri veritabanına kaydeder
            CreateMap<BusCreateDto, Bus>();     //CreateMap<kaynak, hedef>

            //Swagger'dan gelen temiz TripCreateDto bilgisini veritabanındaki Trip entity'sine eşler
            CreateMap<TripCreateDto, Trip>();

            //Trip nesnesinden TripResultDto nesnesine dönüşüm
            //Veritabanından çekilen bilgileri dış dünyaya gösterir
            CreateMap<Trip, TripResultDto>()
                .ForMember(destination => destination.BusPlateNumber,
                    options => options.MapFrom(source => source.Bus != null ? source.Bus.PlateNumber : "Otobüs Atanmamış"))
                .ForMember(destination => destination.SeatCount,
                    options => options.MapFrom(source => source.Bus != null ? source.Bus.SeatCount : 40))  //Varasyılan 40
                .ForMember(destination => destination.BusType,
                    options => options.MapFrom(soruce => soruce.Bus != null ? soruce.Bus.BusType : "2+2"))  //Varsayılan 2+2
                .ForMember(destination => destination.SoldSeats, 
                    options => options.MapFrom(source => source.SoldSeats));    //Koltukların kaybolmasını engelleyen güvenli eşleme satırı

            CreateMap<SoldSeat, SoldSeatResponseDto>().ReverseMap();
        }
    }
}
