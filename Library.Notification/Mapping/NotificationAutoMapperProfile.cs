using AutoMapper;
using Library.Domain.DTOs.Notification;
using Library.Domain.Models.MongoDbModels;

namespace Library.Notification.Mapping;

public class NotificationAutoMapperProfile : Profile
{
    public NotificationAutoMapperProfile()
    {
        CreateMap<NotificationModel, NotificationDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ReverseMap();
    }
}