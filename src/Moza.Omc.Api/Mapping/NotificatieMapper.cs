using AutoMapper;

using Moza.Omc.Api.Data.Entities;
using Moza.Omc.Api.Models;
using Moza.Omc.Api.Models.Requests;

namespace Moza.Omc.Api.Mapping;

public class NotificatieMappingProfile : Profile
{
    public NotificatieMappingProfile()
    {
        // Create -> Model
        this.CreateMap<NotificatieCreateRequest, Notificatie>()
            .ForMember(dest => dest.ContactMethodes, opt => opt.MapFrom(src => src.ContactMethodes))
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
        this.CreateMap<DeliveryReceipt, Notificatie>();

        // Entity <-> Model
        this.CreateMap<NotificatieEntity, Notificatie>().ReverseMap();

        this.CreateMap<Notificatie, NotifyNLNotificatie>()
            .ForMember(dest => dest.TemplateId, opt => opt.MapFrom(src => src.TemplateId))
            .ForMember(dest => dest.EmailAddress, opt => opt.Ignore())
            .ForMember(dest => dest.Personalisation, opt => opt.MapFrom(src =>
                new Dictionary<string, string> { { "name", ExtractNameFromEmail(src.Recipient!) } }));
    }

    private static string ExtractNameFromEmail(string email)
    {
        //Niet relevant maar we sturen nog geen personalisations mee
        var atIndex = email.IndexOf('@');
        return atIndex <= 0 ? email : email[..atIndex];
    }
}