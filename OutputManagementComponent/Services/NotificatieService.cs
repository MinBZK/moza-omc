using AutoMapper;

using OutputManagementComponent.Enumerations;
using OutputManagementComponent.Models;
using OutputManagementComponent.Repositories;
using OutputManagementComponent.Requests;
using OutputManagementComponent.Services.Clients;

using Onderneming = OutputManagementComponent.Models.Onderneming;

namespace OutputManagementComponent.Services;

public class NotificatieService(
    NotificatieRepository notificatieRepository,
    OndernemingRepository ondernemingRepository,
    NotificatieEventRepository notificatieEventRepository,
    NotifyNLClient notifyNLClient,
    ProfielServiceClient profielServiceClient,
    LogiusBrievendienstClient logiusBrievendienstClient,
    IMapper mapper)
{
    public async Task<Onderneming> AddNotificatie(string kvkNummer, string reference,
        NotificatieCreateRequest notificatieCreateRequest)
    {
        var notificatie = this.CreateInitialNotificatie(kvkNummer, reference, request: notificatieCreateRequest);

        var onderneming = await this.CreateOrGetOnderneming(kvkNummer);

        await notificatieRepository.CreateAsync(onderneming.KvkNummer, notificatie);

        await notificatieEventRepository.CreateAsync(reference, new NotificatieEvent("Notificatie aangemaakt.", DeliveryStatuses.Created));

        await this.VerstuurNotificatie(notificatie);

        onderneming.Notificaties = mapper.Map<List<Notificatie>>(await notificatieRepository.GetAllByRecipientAsync(kvkNummer));

        return onderneming;
    }

    private async Task<DeliveryStatuses> RandomStatus(string reference)
    {
        Random _random = new();
        var roll = _random.Next(100);

        DeliveryStatuses returnStatus = DeliveryStatuses.Unknown;

        if (roll < 75)
        {
            //TODO [TB] Dit zou eingelijk via het callback endpoint moeten komen maar die gebruiken we nog niet.
            await notificatieEventRepository.CreateAsync(reference, new NotificatieEvent("Successvol antwoord ontvangen van NotifyNL", DeliveryStatuses.Created));
            returnStatus = DeliveryStatuses.Created;
            return returnStatus;
        }
        else
        {

            var errorStatuses = new[]
                {
                    DeliveryStatuses.Cancelled,
                    DeliveryStatuses.PermanentFailure,
                    DeliveryStatuses.TemporaryFailure,
                    DeliveryStatuses.TechnicalFailure
                };
            returnStatus = errorStatuses[_random.Next(errorStatuses.Length)];

            //TODO [TB] Dit zou eingelijk via het callback endpoint moeten komen maar die gebruiken we nog niet.
            await notificatieEventRepository.CreateAsync(reference, new NotificatieEvent("Faal antwoord ontvangen van NotifyNL", returnStatus));
        }

        return returnStatus;
    }

    private async Task<Onderneming> CreateOrGetOnderneming(string kvkNummer)
    {
        var onderneming = await ondernemingRepository.GetByKvkNummerAsync(kvkNummer);
        onderneming ??= await ondernemingRepository.CreateAsync(new Onderneming { KvkNummer = kvkNummer });

        return mapper.Map<Onderneming>(onderneming);
    }

    private async Task VerstuurNotificatie(Notificatie notificatie)
    {
        if (notificatie == null)
            return;

        foreach (var contactMethode in notificatie.contactMethodes)
        {
            var contactValue = contactMethode.Value;
            var contactType = contactMethode.Type;
            notificatie.Type = contactType;
            contactMethode.Attempted = true;
            if (contactMethode.Type != NotificationTypes.Brief)
            {
                if (contactMethode.Type == NotificationTypes.Kvk)
                {
                    contactValue = await GetEmailFromProfielService(contactValue, notificatie.Reference!);
                    contactType = NotificationTypes.Email;
                    notificatie.Type = contactType;
                }
                await CallNotifyNL(contactValue, contactType, notificatie);
                notificatie.Status = await RandomStatus(notificatie.Reference!);

                if (notificatie.Status < DeliveryStatuses.Cancelled)
                {
                    notificatie.Type = contactMethode.Type;
                    break;
                }
            }
            else
            {
                notificatie = await VerstuurBrief(notificatie, contactValue);
                notificatie.Type = NotificationTypes.Brief;
                break;
            }
        }

        notificatie.SentAt = DateTime.UtcNow;
        await notificatieRepository.Update(notificatie);
    }

    private async Task<Notificatie> VerstuurBrief(Notificatie notificatie, string contactValue)
    {
        await notificatieEventRepository.CreateAsync(notificatie.Reference!, new NotificatieEvent($"Notificatie verstuurd naar Logius Brieven Dienst voor adres {contactValue}", DeliveryStatuses.Sent));
        notificatie.Status = DeliveryStatuses.Sent;
        return notificatie;
    }

    private async Task CallNotifyNL(string contactValue, NotificationTypes contactType, Notificatie notificatie)
    {
        var deliveryStatus = DeliveryStatuses.Sending;

        await notificatieEventRepository.CreateAsync(notificatie.Reference!, new NotificatieEvent($"Notificatie verstuurd naar NotifyNL via {contactType} ({contactValue})", deliveryStatus));
        var result = await notifyNLClient.SendNotificationAsync(notificatie.Type, contactValue, notificatie);
    }

    private async Task<string> GetEmailFromProfielService(string kvkNummer, string reference)
    {
        var profielResponse = await profielServiceClient.OndernemingenGETAsync(kvkNummer);
        //TODO [TB] Status accepted is eigenlijk van een brief die verstuurd moet worden, maar was hier geen mooie status voor voor nu.
        await notificatieEventRepository.CreateAsync(reference, new NotificatieEvent($"Email opgehaald bij de profiel service voor {kvkNummer} ({profielResponse.Onderneming.Email})", DeliveryStatuses.Accepted));
        return profielResponse.Onderneming.Email;
    }

    public async Task<Onderneming> UpdateNotificatie(Notificatie notificatie)
    {
        var updatedNotificatie = await notificatieRepository.Update(notificatie);

        var ondermening = new Onderneming
        {
            KvkNummer = updatedNotificatie.KvkNummer,
            Notificaties = [mapper.Map<Notificatie>(updatedNotificatie)]
        };

        if (updatedNotificatie.Status >= DeliveryStatuses.Cancelled)
        {

            var verstuurBrief = logiusBrievendienstClient.VerstuurBrief();

            if (!verstuurBrief)
            {
                //Start retry/queue voor opnieuw proberen brief versturen
                //Of log error of iets dergelijks.
            }
        }

        return ondermening;
    }

    public async Task<Onderneming> GetAlleNotificaties(string kvkNummer) =>
        mapper.Map<Onderneming>(await ondernemingRepository.GetByKvkNummerAsync(kvkNummer));

    public async Task<Onderneming> GetAlleNotificatiesStatus(string kvkNummer, List<DeliveryStatuses> statusen) =>
        mapper.Map<Onderneming>(await ondernemingRepository.GetByKvkNummerStatusAsync(kvkNummer, statusen));

    public async Task<Onderneming?> GetNotificatie(string kvkNummer, string reference)
    {
        var entity = await ondernemingRepository.GetByKvkNummerIncludeAsync(kvkNummer);
        if (entity == null)
            return null;

        entity.Notificaties = [.. entity.Notificaties.Where(n => n.Reference == reference)];

        return mapper.Map<Onderneming>(entity);
    }

    private Notificatie CreateInitialNotificatie(string kvkNummer, string reference, NotificatieCreateRequest request)
    {
        var notificatie = mapper.Map<Notificatie>(request);
        notificatie.Recipient = kvkNummer;
        notificatie.Reference = reference;
        notificatie.Status = DeliveryStatuses.Sent;
        notificatie.Type = request.contactMethodes.First().Type;
        notificatie.CreatedAt = DateTime.UtcNow;
        notificatie.CompletedAt = null;
        notificatie.SentAt = null;
        notificatie.TemplateId = null;
        notificatie.TemplateVersion = 1;

        notificatie.contactMethodes = request.contactMethodes
            .Select((item, index) =>
            {
                var mapped = mapper.Map<ContactMethode>(item);
                mapped.Order = index;
                return mapped;
            })
            .ToList();

        return notificatie;
    }



    public async Task<int> DeleteNotificatie(string kvkNummer, string reference) =>
        await notificatieRepository.DeleteSpecific(kvkNummer, reference);

    public async Task<int> DeleteAlleNotificaties(string kvkNummer) => await notificatieRepository.DeleteAll(kvkNummer);
}
