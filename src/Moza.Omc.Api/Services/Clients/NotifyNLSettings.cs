namespace Moza.Omc.Api.Services.Clients;

public class NotifyNLSettings
{
    public required string BaseUrl { get; set; }
    public required string SendSmsNotificationUrl { get; set; }
    public required string SendEmailNotificationUrl { get; set; }
    public required string ApiKey { get; set; }
}