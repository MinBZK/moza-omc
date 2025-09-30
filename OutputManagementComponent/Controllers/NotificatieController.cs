using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OutputManagementComponent.Enumerations;
using OutputManagementComponent.Models;
using OutputManagementComponent.Requests;
using OutputManagementComponent.Responses;
using OutputManagementComponent.Services;

namespace OutputManagementComponent.Controllers;

[ApiController]
[Route("notificaties")]
public class NotificatieController(NotificatieService notificatieService, IMapper mapper) : ControllerBase
{
    [HttpGet("{kvkNummer}")]
    //[Authorize] // Uitcommenten voor auth
    public async Task<ActionResult<NotificatieResponse>> GetAlleNotificaties(string kvkNummer)
    {
        var notificaties = await notificatieService.GetAlleNotificaties(kvkNummer);

        var notificatieResponses = mapper.Map<NotificatieResponse>(notificaties);

        return this.Ok(notificatieResponses);
    }

    [HttpGet("status/{kvkNummer}")]
    //[Authorize] // Uitcommenten voor auth
    public async Task<ActionResult<NotificatieResponse>> GetAlleNotificatiesStatus(string kvkNummer,
        [FromQuery] List<DeliveryStatuses> statusen)
    {
        var notificaties = await notificatieService.GetAlleNotificatiesStatus(kvkNummer, statusen);

        var notificatieResponses = mapper.Map<NotificatieResponse>(notificaties);

        return this.Ok(notificatieResponses);
    }

    [HttpGet("{kvkNummer}/{reference}")]
    //[Authorize] // Uitcommenten voor auth
    public async Task<ActionResult<Notificatie>> GetNotificatie(string kvkNummer, string reference)
    {
        var onderneming = await notificatieService.GetNotificatie(kvkNummer, reference);

        Notificatie? notificatieResponse = null;
        if (onderneming != null)
            notificatieResponse = onderneming.Notificaties.First();

        return this.Ok(notificatieResponse);
    }

    [HttpPost("{kvkNummer}/{reference}")]
    //[Authorize] // Uitcommenten voor auth
    public async Task<ActionResult<NotificatieResponse>> AddNotificatie(string kvkNummer, string reference,
        NotificatieCreateRequest notificatieRequest)
    {
        var onderneming = await notificatieService.AddNotificatie(kvkNummer, reference, notificatieRequest);

        var notificatieResponse = mapper.Map<NotificatieResponse>(onderneming);

        return this.Ok(notificatieResponse);
    }

    [HttpPut("/UpdateNotificatie")]
    [Authorize] // Uitcommenten voor auth
    public async Task<ActionResult<NotificatieResponse>> UpdateNotificatie(DeliveryReceipt deliveryReceipt)
    {
        var notificatie = mapper.Map<Notificatie>(deliveryReceipt);

        var updatedEntity = await notificatieService.UpdateNotificatie(notificatie);

        var notificatieResponse = mapper.Map<NotificatieResponse>(updatedEntity);

        return this.Ok(notificatieResponse);
    }

    [HttpDelete("{kvkNummer}/{reference}")]
    //[Authorize] // Uitcommenten voor auth
    public async Task<ActionResult<NotificatieResponse>> DeleteNotificatie(string kvkNummer, string reference)
    {
        var rowsChanged = await notificatieService.DeleteNotificatie(kvkNummer, reference);

        return this.Ok(rowsChanged);
    }

    [HttpDelete("{kvkNummer}")]
    //[Authorize] // Uitcommenten voor auth
    public async Task<ActionResult<NotificatieResponse>> DeleteAlleNotificatie(string kvkNummer)
    {
        var rowsChanged = await notificatieService.DeleteAlleNotificaties(kvkNummer);

        return this.Ok(rowsChanged);
    }
}
