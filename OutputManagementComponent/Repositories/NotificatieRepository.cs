using AutoMapper;

using Microsoft.EntityFrameworkCore;

using OutputManagementComponent.data;
using OutputManagementComponent.Data.Entities;
using OutputManagementComponent.Models;

namespace OutputManagementComponent.Repositories;

public class NotificatieRepository(OMCDbContext context, IMapper mapper)
{
    public async Task<NotificatieEntity> CreateAsync(string kvkNummer, Notificatie notificatie)
    {
        var notificatieEntity = mapper.Map<NotificatieEntity>(notificatie);
        notificatieEntity.KvkNummer = kvkNummer;

        context.notificaties.Add(notificatieEntity);
        await context.SaveChangesAsync();

        return notificatieEntity;
    }

    public async Task<List<NotificatieEntity>> GetAllByRecipientAsync(string kvkNummer)
    {
        var entities = await context.notificaties
            .Where(o => o.KvkNummer == kvkNummer)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return entities;
    }

    public async Task<List<NotificatieEntity>> GetAllAsync()
    {
        var entities = await context.notificaties
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return entities;
    }

    public async Task<NotificatieEntity?> GetByRecipientAndReferenceAsync(string recipient, string reference)
    {
        var entity = await context.notificaties
            .FirstOrDefaultAsync(o => o.Recipient == recipient && o.Reference == reference);

        return entity;
    }

    public async Task<NotificatieEntity> Update(Notificatie notificatie)
    {
        var existingNotificatie = await context.notificaties
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

        if (notificatie.contactMethodes != null)
        {
            context.contactmethodes.RemoveRange(existingNotificatie.ContactMethodes);
            existingNotificatie.ContactMethodes = mapper.Map<List<ContactMethodeEntity>>(notificatie.contactMethodes);
        }

        await context.SaveChangesAsync();

        return existingNotificatie;
    }

    public async Task<int> DeleteAll(string recipient)
    {
        var existingNotificaties = await context.notificaties
            .Where(o => o.Recipient == recipient)
            .ToListAsync();

        context.RemoveRange(existingNotificaties);
        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteAll()
    {
        var existingNotificaties = await context.notificaties.ToListAsync();
        context.RemoveRange(existingNotificaties);
        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteSpecific(string kvkNummer, string tracenummer)
    {
        var existingNotificatie = await context.notificaties
            .FirstOrDefaultAsync(o => o.KvkNummer == kvkNummer && o.Reference == tracenummer);

        context.Remove(existingNotificatie);
        return await context.SaveChangesAsync();
    }
}
