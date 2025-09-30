using AutoMapper;

using OutputManagementComponent.Data.Entities;
using OutputManagementComponent.Models;

namespace OutputManagementComponent.Mapping;

public class ContactMethodeMapper : Profile
{
    public ContactMethodeMapper()
    {
        CreateMap<ContactMethode, ContactMethodeEntity>()
            .PreserveReferences()
            .ReverseMap();

        CreateMap<ContactMethodeRequest, ContactMethode>()
            .ForMember(dest => dest.Order, opt => opt.Ignore())
            .ForMember(dest => dest.Attempted, opt => opt.Ignore())
            .ReverseMap();
    }
}
