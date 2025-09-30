using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using OutputManagementComponent.Responses;
using OutputManagementComponent.Services;

namespace OutputManagementComponent.controllers;

[ApiController]
[Route("admin")]
public class AdminController(AdminService adminService, IMapper mapper) : ControllerBase
{
    [HttpGet("/notificaties")]
    //[Authorize] // Uitcommenten voor auth
    public async Task<ActionResult<List<NotificatieResponse>>> GetEchtAlleNotificaties()
    {
        var ondernemingen = await adminService.GetEchtAlleNotificaties();

        var notificatieResponses = mapper.Map<List<NotificatieResponse>>(ondernemingen);

        return this.Ok(notificatieResponses);
    }

    [HttpDelete("/notificaties")]
    //[Authorize] // Uitcommenten voor auth
    public async Task<ActionResult<NotificatieResponse>> DeleteEchtAlleNotificatie()
    {
        var rowsChanged = await adminService.DeleteEchtAlleNotificaties();

        return this.Ok(rowsChanged);
    }
}
