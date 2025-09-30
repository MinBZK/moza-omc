using AutoMapper;

using OutputManagementComponent.Data.Entities;
using OutputManagementComponent.Models;
using OutputManagementComponent.Repositories;

namespace OutputManagementComponent.Services;

public class AdminService(NotificatieRepository notificatieRepository, IMapper mapper)
{
    public async Task<List<Onderneming>> GetEchtAlleNotificaties()
    {
        var notificaties = await notificatieRepository.GetAllAsync();

        var ondernemingen = notificaties
            .GroupBy(n => n.KvkNummer)
            .Select(g => new OndernemingEntity
            {
                KvkNummer = g.Key,
                Notificaties = [.. g]
            })
            .ToList();

        return mapper.Map<List<Onderneming>>(ondernemingen);
    }

    public async Task<int> DeleteEchtAlleNotificaties() => await notificatieRepository.DeleteAll();
}
