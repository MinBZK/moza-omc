using System.ComponentModel.DataAnnotations;

namespace OutputManagementComponent.Data.Entities;

public class OndernemingEntity
{
    [Key]
    public string KvkNummer { get; set; } = string.Empty;

    public ICollection<NotificatieEntity> Notificaties { get; set; } = [];
}
