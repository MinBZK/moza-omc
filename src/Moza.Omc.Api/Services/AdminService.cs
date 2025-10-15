using AutoMapper;

using Moza.Omc.Api.Data.Entities;
using Moza.Omc.Api.Data.Repositories;
using Moza.Omc.Api.Models;

namespace Moza.Omc.Api.Services;

public class AdminService(NotificatieRepository notificatieRepository, IMapper mapper)
{
    public async Task<List<Onderneming>> GetEchtAlleNotificaties()
    {
        var notificaties = await notificatieRepository.GetAllAsync();
        var ondernemingen = notificaties
            .GroupBy(n => n.KvkNummer)
            .Select(g => new OndernemingEntity { KvkNummer = g.Key, Notificaties = [.. g] })
            .ToList();

        return mapper.Map<List<Onderneming>>(ondernemingen);
    }

    public async Task<int> DeleteEchtAlleNotificaties() => await notificatieRepository.DeleteAll();
}