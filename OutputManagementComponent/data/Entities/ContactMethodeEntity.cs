using OutputManagementComponent.Data.Entities;
using OutputManagementComponent.Enumerations;

namespace OutputManagementComponent.Data.Entities;

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
