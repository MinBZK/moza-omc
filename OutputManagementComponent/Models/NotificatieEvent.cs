using OutputManagementComponent.Enumerations;

namespace OutputManagementComponent.Models;

public class NotificatieEvent(string @event, DeliveryStatuses @status)
{
    public string Event { get; set; } = @event;
    public DeliveryStatuses? Status { get; set; } = status;
    public DateTime DateCreated { get; set; }

}

