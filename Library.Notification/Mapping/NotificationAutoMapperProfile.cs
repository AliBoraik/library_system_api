using AutoMapper;
using Library.Domain;
using Library.Domain.DTOs.Notification;
using Library.Domain.Models;

namespace Library.Notification.Mapping;

public class NotificationAutoMapperProfile : Profile
{
    public NotificationAutoMapperProfile()
    {
        CreateMap<NotificationModel, NotificationDto>()
            .ForMember(
                dest => dest.SentAt,
                opt => opt.MapFrom(src => Converter.ToUnixTimestampSeconds(src.SentAt))
            )
            .ReverseMap();
    }
}