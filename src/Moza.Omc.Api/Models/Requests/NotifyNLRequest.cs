using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Moza.Omc.Api.Common.Enumerations;

namespace Moza.Omc.Api.Models.Requests;

public class NotifyNLRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotifyNLRequest" /> struct.
    /// </summary>
    public NotifyNLRequest()
    {
    }

    /// <summary>
    /// The notification type.
    /// </summary>
    [Required]
    [JsonRequired]
    [JsonPropertyName("notification_type")]
    [JsonPropertyOrder(7)]
    public NotificationTypes Type { get; set; }
}