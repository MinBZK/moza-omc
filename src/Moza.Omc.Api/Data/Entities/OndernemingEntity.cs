using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moza.Omc.Api.Data.Entities;

[Table("ondernemingen")]
public class OndernemingEntity
{
    [Key]
    public string KvkNummer { get; set; } = string.Empty;

    public ICollection<NotificatieEntity> Notificaties { get; set; } = [];
}