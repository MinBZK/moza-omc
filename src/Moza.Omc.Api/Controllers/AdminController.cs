using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using Moza.Omc.Api.Models.Responses;
using Moza.Omc.Api.Services;

namespace Moza.Omc.Api.Controllers;

[ApiController]
[Route("admin")]
public class AdminController(AdminService adminService, IMapper mapper) : ControllerBase
{
    [HttpGet("/notificaties")]
    public async Task<ActionResult<List<NotificatieResponse>>> GetEchtAlleNotificaties()
    {
        var ondernemingen = await adminService.GetEchtAlleNotificaties();
        var notificatieResponses = mapper.Map<List<NotificatieResponse>>(ondernemingen);
        return this.Ok(notificatieResponses);
    }

    [HttpDelete("/notificaties")]
    public async Task<ActionResult<NotificatieResponse>> DeleteEchtAlleNotificatie()
    {
        var rowsChanged = await adminService.DeleteEchtAlleNotificaties();
        return this.Ok(rowsChanged);
    }
}