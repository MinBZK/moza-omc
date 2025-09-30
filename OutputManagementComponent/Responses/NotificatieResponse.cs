using System.Text.Json.Serialization;

using OutputManagementComponent.Models;

namespace OutputManagementComponent.Responses;

public class NotificatieResponse
{
    /// <summary>
    /// De notificatie
    /// </summary>
    [JsonPropertyName(nameof(KvkNummer))]
    public required string KvkNummer { get; set; }

    /// <summary>
    /// De notificatie
    /// </summary>
    [JsonPropertyName(nameof(Notificaties))]
    public required List<Notificatie> Notificaties { get; set; } = [];
}
