using System.ComponentModel.DataAnnotations.Schema;

using Moza.Omc.Api.Common.Enumerations;

namespace Moza.Omc.Api.Data.Entities;

[Table("contactmethodes")]
public class ContactMethodeEntity
{
    public int Id { get; set; }
    public string Value { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool Attempted { get; set; } = false;
    public NotificationTypes Type { get; set; }
    public int NotificatieEntityDbId { get; set; }
    public NotificatieEntity Notificatie { get; set; } = default!;
}