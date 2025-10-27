using System.ComponentModel.DataAnnotations;

namespace Moza.Omc.Api.Common.Enumerations;

public enum NotificationTypes
{
    [Display(Name = "unknown")]
    Unknown = 0,

    [Display(Name = "email")]
    Email = 1,

    [Display(Name = "sms")]
    Sms = 2,

    [Display(Name = "brief")]
    Brief = 3,

    [Display(Name = "kvk")]
    Kvk = 4
}