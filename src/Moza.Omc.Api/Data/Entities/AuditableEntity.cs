namespace Moza.Omc.Api.Data.Entities;

public abstract class AuditableEntity
{
    public DateTime DateCreated { get; set; }
    public DateTime DateLastUpdated { get; set; }
}