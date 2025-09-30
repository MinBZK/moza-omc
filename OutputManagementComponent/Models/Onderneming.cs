namespace OutputManagementComponent.Models;

public class Onderneming
{
    public Onderneming() { }

    public Onderneming(string kvkNummer)
    {
        this.KvkNummer = kvkNummer;
    }

    public string KvkNummer { get; set; } = string.Empty;
    public List<Notificatie> Notificaties { get; set; } = [];
}
