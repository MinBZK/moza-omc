using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Moza.Omc.Api.Common.Enumerations;
using Moza.Omc.Api.Data.Entities;
using Moza.Omc.Api.Models;

namespace Moza.Omc.Api.Data.Repositories;

public class OndernemingRepository(OmcDbContext context, IMapper mapper)
{
    public async Task<OndernemingEntity> CreateAsync(Onderneming onderneming)
    {
        var ondernemingEntity = mapper.Map<OndernemingEntity>(onderneming);
        context.Ondernemingen.Add(ondernemingEntity);
        await context.SaveChangesAsync();
        return ondernemingEntity;
    }

    public async Task<OndernemingEntity> GetByKvkNummerAsync(string kvkNummer)
    {
        var entities = await context.Ondernemingen
            .Where(o => o.KvkNummer == kvkNummer)
            .Include(o => o.Notificaties)
            .ThenInclude(n => n.ContactMethodes)
            .Include(o => o.Notificaties)
            .ThenInclude(n => n.Events)
            .FirstOrDefaultAsync();

        if (entities != null)
        {
            SortNotificatiesAndEventsByCreatedAt(entities);
        }

        return mapper.Map<OndernemingEntity>(entities);
    }

    public async Task<OndernemingEntity> GetByKvkNummerStatusAsync(string kvkNummer, List<DeliveryStatuses> statusen)
    {
        var entities = await context.Ondernemingen
            .Where(o => o.KvkNummer == kvkNummer)
            .Include(o => o.Notificaties
                .Where(n => statusen.Contains(n.Status)))
            .ThenInclude(n => n.ContactMethodes)
            .Include(o => o.Notificaties
                .Where(n => statusen.Contains(n.Status)))
            .ThenInclude(n => n.Events)
            .FirstOrDefaultAsync();

        if (entities != null)
        {
            SortNotificatiesAndEventsByCreatedAt(entities);
        }

        return mapper.Map<OndernemingEntity>(entities);
    }

    public async Task<OndernemingEntity> GetByKvkNummerIncludeAsync(string kvkNummer)
    {
        var entities = await context.Ondernemingen
            .Where(o => o.KvkNummer == kvkNummer)
            .Include(o => o.Notificaties)
            .ThenInclude(n => n.ContactMethodes)
            .Include(o => o.Notificaties)
            .ThenInclude(n => n.Events)
            .FirstOrDefaultAsync();

        if (entities != null)
        {
            SortNotificatiesAndEventsByCreatedAt(entities);
        }

        return mapper.Map<OndernemingEntity>(entities);
    }

    public async Task<List<OndernemingEntity>> GetAllAsync()
    {
        var entities = await context.Ondernemingen.ToListAsync();
        return entities;
    }

    public async Task<int> DeleteAll(string kvkNummer)
    {
        var existingOndernemings = await context.Ondernemingen
            .Where(o => o.KvkNummer == kvkNummer)
            .ToListAsync();

        context.RemoveRange(existingOndernemings);
        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteAll()
    {
        var existingOndernemings = await context.Ondernemingen.ToListAsync();
        context.RemoveRange(existingOndernemings);
        return await context.SaveChangesAsync();
    }

    public static void SortNotificatiesAndEventsByCreatedAt(OndernemingEntity entity, bool ascending = false)
    {
        if (entity?.Notificaties == null) return;

        entity.Notificaties =
        [
            .. ascending
                ? entity.Notificaties.OrderBy(n => n.CreatedAt)
                : entity.Notificaties.OrderByDescending(n => n.CreatedAt)
        ];

        foreach (var notificatie in entity.Notificaties)
        {
            if (notificatie.Events != null)
            {
                notificatie.Events =
                [
                    .. ascending
                        ? notificatie.Events.OrderBy(e => e.DateCreated)
                        : notificatie.Events.OrderByDescending(e => e.DateCreated)
                ];
            }
        }
    }
}