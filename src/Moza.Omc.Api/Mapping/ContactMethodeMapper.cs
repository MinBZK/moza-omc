using AutoMapper;

using Moza.Omc.Api.Data.Entities;
using Moza.Omc.Api.Models;
using Moza.Omc.Api.Models.Requests;

namespace Moza.Omc.Api.Mapping;

public class ContactMethodeMapper : Profile
{
    public ContactMethodeMapper()
    {
        this.CreateMap<ContactMethode, ContactMethodeEntity>()
            .PreserveReferences()
            .ReverseMap();

        this.CreateMap<ContactMethodeRequest, ContactMethode>()
            .ForMember(dest => dest.Order, opt => opt.Ignore())
            .ForMember(dest => dest.Attempted, opt => opt.Ignore())
            .ReverseMap();
    }
}
