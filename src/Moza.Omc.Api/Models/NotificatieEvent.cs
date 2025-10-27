using Moza.Omc.Api.Common.Enumerations;

namespace Moza.Omc.Api.Models;

public class NotificatieEvent(string @event, DeliveryStatuses status)
{
    public string Event { get; set; } = @event;
    public DeliveryStatuses? Status { get; set; } = status;
    public DateTime DateCreated { get; set; }
}