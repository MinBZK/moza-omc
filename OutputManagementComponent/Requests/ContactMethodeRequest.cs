using OutputManagementComponent.Enumerations;

namespace OutputManagementComponent.Models;

public class ContactMethodeRequest
{
    public string Value { get; set; } = string.Empty;

    public NotificationTypes Type { get; set; }
}
