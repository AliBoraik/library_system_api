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
            .ForMember(dest => dest.SentAt,
                opt => opt.MapFrom(src => Converter.ToUnixTimestampSeconds(src.SentAt)))
            .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => src.UserNotifications.First().IsRead));

        CreateMap<CreateNotificationDto, NotificationModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Id is auto-generated, so ignore it
            .ForMember(dest => dest.SentAt, opt => opt.MapFrom(_ => DateTime.UtcNow)) // Set current UTC time
            .ForMember(dest => dest.UserNotifications, opt => opt.MapFrom(src =>
                new List<UserNotification>
                {
                    new()
                    {
                        UserId = src.RecipientUserId,
                        IsRead = false
                    }
                }));
    }
}