using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using OutputManagementComponent.Models;

namespace OutputManagementComponent.Requests;

public class NotificatieCreateRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificatieCreateRequest" /> struct.
    /// </summary>
    public NotificatieCreateRequest()
    {
    }

    /// <summary>
    /// The contact method.
    /// </summary>
    [Required]
    [JsonRequired]
    [JsonPropertyName("contact_methode")]
    [JsonPropertyOrder(7)]
    public ICollection<ContactMethodeRequest> contactMethodes { get; set; } = [];
}
