using AutoMapper;

using Moza.Omc.Api.Data.Entities;
using Moza.Omc.Api.Models;
using Moza.Omc.Api.Models.Responses;

namespace Moza.Omc.Api.Mapping;

public class OndernemingMappingProfile : Profile
{
    public OndernemingMappingProfile()
    {
        this.CreateMap<OndernemingEntity, Onderneming>().ReverseMap();
        this.CreateMap<NotificatieResponse, Onderneming>().ReverseMap();
    }
}