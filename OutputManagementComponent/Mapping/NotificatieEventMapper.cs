using AutoMapper;

using OutputManagementComponent.Data.Entities;
using OutputManagementComponent.Models;

namespace OutputManagementComponent.Mapping;

public class NotificatieEventMapper : Profile
{
    public NotificatieEventMapper()
    {
        CreateMap<NotificatieEvent, NotificatieEventEntity>()
            .PreserveReferences()
            .ReverseMap();
    }
}
