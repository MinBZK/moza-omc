using AutoMapper;

using Moza.Omc.Api.Data.Entities;
using Moza.Omc.Api.Models;

namespace Moza.Omc.Api.Mapping;

public class NotificatieEventMapper : Profile
{
    public NotificatieEventMapper()
    {
        this.CreateMap<NotificatieEvent, NotificatieEventEntity>()
            .PreserveReferences()
            .ReverseMap();
    }
}