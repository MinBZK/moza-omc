using AutoMapper;

using OutputManagementComponent.data.Entities;
using OutputManagementComponent.Data.Entities;
using OutputManagementComponent.Models;
using OutputManagementComponent.Requests;

namespace OutputManagementComponent.Mapping;

public class NotificatieMappingProfile : Profile
{
    public NotificatieMappingProfile()
    {
        // Create -> Model
        CreateMap<NotificatieCreateRequest, Notificatie>()
            .ForMember(dest => dest.contactMethodes, opt => opt.MapFrom(src => src.contactMethodes))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Recipient, opt => opt.Ignore())
            .ForMember(dest => dest.Reference, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.Type, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CompletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.SentAt, opt => opt.Ignore())
            .ForMember(dest => dest.TemplateId, opt => opt.Ignore())
            .ForMember(dest => dest.TemplateVersion, opt => opt.Ignore())
            .ForMember(dest => dest.Events, opt => opt.Ignore());



        // Update -> Model
        CreateMap<DeliveryReceipt, Notificatie>();

        // Entity <-> Model
        CreateMap<NotificatieEntity, Notificatie>()
            .ReverseMap();

        CreateMap<Notificatie, NotifyNLNotificatie>()
                .ForMember(dest => dest.TemplateId, opt => opt.MapFrom(src => src.TemplateId))
                .ForMember(dest => dest.EmailAddress, opt => opt.Ignore())
                .ForMember(dest => dest.Personalisation, opt => opt.MapFrom(src =>
                    new Dictionary<string, string> { { "name", ExtractNameFromEmail(src.Recipient) } }));
    }

    private static string ExtractNameFromEmail(string email)
    {
        //Niet relevant maar we sturen nog geen personalisations mee
        var atIndex = email.IndexOf('@');
        if (atIndex <= 0)
            return email;
        return email[..atIndex];
    }
}

