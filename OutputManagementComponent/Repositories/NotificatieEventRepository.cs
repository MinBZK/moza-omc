using AutoMapper;

using Microsoft.EntityFrameworkCore;

using OutputManagementComponent.data;
using OutputManagementComponent.Data.Entities;
using OutputManagementComponent.Models;

namespace OutputManagementComponent.Repositories;

public class NotificatieEventRepository(OMCDbContext context, IMapper mapper)
{
    public async Task<NotificatieEventEntity> CreateAsync(string reference, NotificatieEvent model)
    {
        var entity = mapper.Map<NotificatieEventEntity>(model);
        entity.Reference = reference;

        context.notificatieevents.Add(entity);
        await context.SaveChangesAsync();

        return entity;
    }
}
