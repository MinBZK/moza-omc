using Moza.Omc.Api.Common.Enumerations;

namespace Moza.Omc.Api.Models.Requests;

public class ContactMethodeRequest
{
    public string Value { get; set; } = string.Empty;
    public NotificationTypes Type { get; set; }
}