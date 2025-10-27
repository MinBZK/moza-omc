using System.Text.Json.Serialization;

namespace Moza.Omc.Api.Models;

public class NotifyNLNotificatie
{
    [JsonPropertyName("template_id")]
    public Guid TemplateId { get; set; }

    [JsonPropertyName("email_address")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? EmailAddress { get; set; }

    [JsonPropertyName("phone_number")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PhoneNumber { get; set; }

    [JsonPropertyName("personalisation")]
    public Dictionary<string, string> Personalisation { get; set; } = [];
}