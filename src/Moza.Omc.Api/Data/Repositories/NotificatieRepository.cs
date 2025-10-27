using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Moza.Omc.Api.Data.Entities;
using Moza.Omc.Api.Models;

namespace Moza.Omc.Api.Data.Repositories;

public class NotificatieRepository(OmcDbContext context, IMapper mapper)
{
    public async Task<NotificatieEntity> CreateAsync(string kvkNummer, Notificatie notificatie)
    {
        var notificatieEntity = mapper.Map<NotificatieEntity>(notificatie);
        notificatieEntity.KvkNummer = kvkNummer;
        context.Notificaties.Add(notificatieEntity);
        await context.SaveChangesAsync();
        return notificatieEntity;
    }

    public async Task<List<NotificatieEntity>> GetAllByRecipientAsync(string kvkNummer)
    {
        var entities = await context.Notificaties
            .Where(o => o.KvkNummer == kvkNummer)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return entities;
    }

    public async Task<List<NotificatieEntity>> GetAllAsync()
    {
        var entities = await context.Notificaties
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return entities;
    }

    public async Task<NotificatieEntity?> GetByRecipientAndReferenceAsync(string recipient, string reference)
    {
        var entity = await context.Notificaties
            .FirstOrDefaultAsync(o => o.Recipient == recipient && o.Reference == reference);

        return entity;
    }

    public async Task<NotificatieEntity> Update(Notificatie notificatie)
    {
        var existingNotificatie = await context.Notificaties
            .FirstOrDefaultAsync(o => o.Id == notificatie.Id && o.Reference == notificatie.Reference);

        if (existingNotificatie != null)
        {
            existingNotificatie.Status = notificatie.Status;
            existingNotificatie.SentAt = notificatie.SentAt;
            existingNotificatie.CompletedAt = notificatie.CompletedAt;
        }
        else
        {
            throw new KeyNotFoundException("Notificatie met deze reference niet gevonden.");
        }

        if (notificatie.ContactMethodes != null)
        {
            context.ContactMethodes.RemoveRange(existingNotificatie.ContactMethodes);
            existingNotificatie.ContactMethodes = mapper.Map<List<ContactMethodeEntity>>(notificatie.ContactMethodes);
        }

        await context.SaveChangesAsync();
        return existingNotificatie;
    }

    public async Task<int> DeleteAll(string recipient)
    {
        var existingNotificaties = await context.Notificaties
            .Where(o => o.Recipient == recipient)
            .ToListAsync();

        context.RemoveRange(existingNotificaties);
        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteAll()
    {
        var existingNotificaties = await context.Notificaties.ToListAsync();
        context.RemoveRange(existingNotificaties);
        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteSpecific(string kvkNummer, string tracenummer)
    {
        var existingNotificatie = await context.Notificaties
            .FirstOrDefaultAsync(o => o.KvkNummer == kvkNummer && o.Reference == tracenummer);

        if (existingNotificatie == null) throw new KeyNotFoundException("Notificatie niet gevonden.");

        context.Remove(existingNotificatie);
        return await context.SaveChangesAsync();
    }
}