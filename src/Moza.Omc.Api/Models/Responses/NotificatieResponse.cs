using System.Text.Json.Serialization;

namespace Moza.Omc.Api.Models.Responses;

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