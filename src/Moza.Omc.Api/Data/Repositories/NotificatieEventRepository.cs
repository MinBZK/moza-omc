using AutoMapper;

using Moza.Omc.Api.Data.Entities;
using Moza.Omc.Api.Models;

namespace Moza.Omc.Api.Data.Repositories;

public class NotificatieEventRepository(OmcDbContext context, IMapper mapper)
{
    public async Task<NotificatieEventEntity> CreateAsync(string reference, NotificatieEvent model)
    {
        var entity = mapper.Map<NotificatieEventEntity>(model);
        entity.Reference = reference;
        context.NotificatieEvents.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}