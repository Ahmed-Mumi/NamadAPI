using AutoMapper;
using NomadAPI.Dtos;
using NomadAPI.Entities;
using NomadAPI.Extensions;
using System;
using System.Linq;

namespace NomadAPI.Helpers
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<AppUser, NomadDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<NomadUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<Application, ApplicationDto>();
            CreateMap<TravelUpdateDto, Travel>();
            CreateMap<Report, ReportDto>().ReverseMap();
            CreateMap<Travel, TravelDto>().ReverseMap();
            CreateMap<CreateTravelDto, Travel>()
                .ForMember(dest => dest.PostedDate, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src => src.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src => src.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.RecipientFullName, opt => opt.MapFrom(src => src.Recipient.FullName))
                .ForMember(dest => dest.SenderFullName, opt => opt.MapFrom(src => src.Sender.FullName));
        }
    }
}
