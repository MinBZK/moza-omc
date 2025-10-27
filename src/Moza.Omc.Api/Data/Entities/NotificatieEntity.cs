using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Moza.Omc.Api.Common.Enumerations;

namespace Moza.Omc.Api.Data.Entities;

[Table("notificaties")]
public class NotificatieEntity : AuditableEntity
{
    [Key]
    public int DbId { get; set; }

    public Guid Id { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string Recipient { get; set; } = string.Empty;
    public DeliveryStatuses Status { get; set; }
    public NotificationTypes Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? SentAt { get; set; }
    public Guid TemplateId { get; set; }
    public int TemplateVersion { get; set; }
    public required string KvkNummer { get; set; }
    public OndernemingEntity Onderneming { get; set; } = default!;
    public ICollection<NotificatieEventEntity> Events { get; set; } = new List<NotificatieEventEntity>();
    public List<ContactMethodeEntity> ContactMethodes { get; set; } = new();
}