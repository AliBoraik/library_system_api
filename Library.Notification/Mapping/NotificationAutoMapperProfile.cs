using AutoMapper;
using Library.Domain.DTOs.Notification;
using Library.Domain.Models;

namespace Library.Notification.Mapping;

public class NotificationAutoMapperProfile : Profile
{
    public NotificationAutoMapperProfile()
    {
        CreateMap<NotificationModel, NotificationDto>().ReverseMap();
    }
}