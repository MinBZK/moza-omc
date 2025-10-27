using Moza.Omc.Api.Common.Enumerations;

namespace Moza.Omc.Api.Models;

public class ContactMethode
{
    public string Value { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool Attempted { get; set; } = false;
    public NotificationTypes Type { get; set; }
}