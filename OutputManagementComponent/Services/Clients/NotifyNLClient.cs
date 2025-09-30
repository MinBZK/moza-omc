using AutoMapper;

using JWT;
using JWT.Algorithms;
using JWT.Serializers;

using Microsoft.Extensions.Options;

using OutputManagementComponent.Enumerations;
using OutputManagementComponent.Models;

namespace OutputManagementComponent.Services.Clients;

public class NotifyNLClient
{
    private readonly NotifyNLSettings _settings;

    private Dictionary<NotificationTypes, string> NotificationEndpoints =>
        new()
        {
            { NotificationTypes.Sms, _settings.SendSmsNotificationUrl },
            { NotificationTypes.Email, _settings.SendEmailNotificationUrl }
        };


    private readonly HttpClient httpClient;
    private readonly string Secret;
    private readonly string ServiceId;
    private readonly IMapper mapper;

    public NotifyNLClient(HttpClient httpClient, IMapper mapper, IOptions<NotifyNLSettings> settings)
    {
        _settings = settings.Value;
        this.httpClient = httpClient;
        this.mapper = mapper;

        var (serviceId, secret) = ExtractServiceIdAndApiKey(_settings.ApiKey);
        this.ServiceId = serviceId;
        this.Secret = secret;
    }

    public async Task<HttpResponseMessage> SendNotificationAsync(NotificationTypes type, string receiver, Notificatie notificatie)
    {
        if (!NotificationEndpoints.TryGetValue(type, out var relativeUrl))
            throw new ArgumentOutOfRangeException(nameof(type), $"Unsupported notification type: {type}");

        var fullUrl = $"{_settings.BaseUrl}/{relativeUrl}";

        var payload = mapper.Map<NotifyNLNotificatie>(notificatie);
        if (NotificationTypes.Email == type)
        {
            payload.EmailAddress = receiver;
            payload.TemplateId = Guid.Parse("cc4c3190-e543-4f6c-95a4-ab6e8bb3522d");
        }
        else if (NotificationTypes.Sms == type)
        {
            payload.PhoneNumber = receiver;
            payload.TemplateId = Guid.Parse("bda36fc5-79df-4423-bb79-37abe212b432");
        }

        var request = new HttpRequestMessage(HttpMethod.Post, fullUrl)
        {
            Content = JsonContent.Create(payload)
        };
        string token = CreateToken(this.Secret, this.ServiceId);
        request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");
        var result = await this.httpClient.SendAsync(request);
        return result;
    }

    private static (string serviceId, string secret) ExtractServiceIdAndApiKey(string fromApiKey)
    {
        if (string.IsNullOrWhiteSpace(fromApiKey) || fromApiKey.Contains(' ') || fromApiKey.Length < 74)
            throw new NotifyAuthException(
                "The API Key provided is invalid. Please ensure you are using a v2 API Key that is not empty or null");

        var serviceId = fromApiKey.Substring(fromApiKey.Length - 73, 36);
        var secret = fromApiKey.Substring(fromApiKey.Length - 36, 36);

        return (serviceId, secret);
    }

    private static string CreateToken(string secret, string serviceId)
    {
        ValidateGuids(secret, serviceId);

        var payload = new Dictionary<string, object>
        {
            { "iss", serviceId },
            { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds() }
        };

        HMACSHA256Algorithm algorithm = new();
        IJsonSerializer serializer = new JsonNetSerializer();
        IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
        JwtEncoder encoder = new(algorithm, serializer, urlEncoder);

        return encoder.Encode(payload, secret);
    }

    public static void ValidateGuids(params string[] stringGuids)
    {
        if (stringGuids == null)
            return;

        for (var i = 0; i < stringGuids.Length; i++)
            if (!Guid.TryParse(stringGuids[i], out _))
                throw new NotifyAuthException("Invalid secret or serviceId. Please check that your API Key is correct");
    }
}

public class NotifyAuthException(string message) : Exception(message)
{
}
