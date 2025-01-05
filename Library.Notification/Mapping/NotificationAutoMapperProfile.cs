using AutoMapper;
using Library.Domain.DTOs.Notification;
using Library.Domain.Models;
using Library.Domain.Utilities;

namespace Library.Notification.Mapping;

public class NotificationAutoMapperProfile : Profile
{
    public NotificationAutoMapperProfile()
    {
        
        // Map NotificationModel to NotificationDto
        CreateMap<NotificationModel, NotificationDto>()
            .ForMember(dest => dest.SentAt, opt => opt.MapFrom(src => Converter.ToUnixTimestampSeconds(src.SentAt)));

        // Mapping from UserNotification to NotificationDto
        CreateMap<UserNotification, NotificationDto>()
            .IncludeMembers(src => src.Notification); // Automatically include Notification properties


        CreateMap<NotificationEvent, NotificationModel>()
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