using AutoMapper;

using OutputManagementComponent.Data.Entities;
using OutputManagementComponent.Models;
using OutputManagementComponent.Responses;

namespace OutputManagementComponent.Mapping;

public class OndernemingMappingProfile : Profile
{
    public OndernemingMappingProfile()
    {
        this.CreateMap<OndernemingEntity, Onderneming>().ReverseMap();

        this.CreateMap<NotificatieResponse, Onderneming>().ReverseMap();
    }
}
