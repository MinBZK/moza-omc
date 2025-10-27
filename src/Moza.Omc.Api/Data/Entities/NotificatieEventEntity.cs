using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Moza.Omc.Api.Common.Enumerations;

namespace Moza.Omc.Api.Data.Entities;

[Table("notificatieevents")]
public class NotificatieEventEntity : AuditableEntity
{
    [Key]
    public int Id { get; set; }

    public string Event { get; set; } = string.Empty;
    public DeliveryStatuses? Status { get; set; } = null;
    public string Reference { get; set; } = string.Empty;

    [ForeignKey(nameof(Reference))]
    public NotificatieEntity Notificatie { get; set; } = default!;
}