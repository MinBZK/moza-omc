using OutputManagementComponent.Enumerations;

namespace OutputManagementComponent.Models;

public class Notificatie
{
    public Notificatie() { }

    public Notificatie(Guid id, string? reference, string recipient, DeliveryStatuses status, NotificationTypes type,
        DateTime createdAt, DateTime completedAt, DateTime sentAt, Guid templateId, int templateVersion, List<NotificatieEvent> events)
    {
        this.Id = id;
        this.Reference = reference;
        this.Recipient = recipient;
        this.Status = status;
        this.Type = type;
        this.CreatedAt = createdAt;
        this.CompletedAt = completedAt;
        this.SentAt = sentAt;
        this.TemplateId = templateId;
        this.TemplateVersion = templateVersion;
        this.Events = events;
    }

    public Guid Id { get; set; } = Guid.Parse("cc4c3190-e543-4f6c-95a4-ab6e8bb3522d");
    public string? Reference { get; set; }
    public string Recipient { get; set; }
    public DeliveryStatuses Status { get; set; }
    public NotificationTypes Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? SentAt { get; set; }
    public Guid? TemplateId { get; set; }
    public int TemplateVersion { get; set; }

    public ICollection<NotificatieEvent> Events { get; set; } = new List<NotificatieEvent>();

    public IEnumerable<ContactMethode> contactMethodes { get; set; } = [];

}
