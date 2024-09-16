namespace Benchy.DapperVsEfCore.Models;

public class DamageImage : Entity
{
    public DamageImage(long id, string smallUrl, string mediumUrl, string originalUrl)
        : this(id, smallUrl, mediumUrl, originalUrl, 0)
    {
    }

    public DamageImage(long id, string smallUrl, string mediumUrl, string originalUrl, long vehicleId)
    {
        Id = id;
        SmallUrl = smallUrl;
        MediumUrl = mediumUrl;
        OriginalUrl = originalUrl;
        VehicleId = vehicleId;
    }

    public DamageImage()
    {
    }

    public string SmallUrl { get; set; } = string.Empty;
    public string MediumUrl { get; set; } = string.Empty;
    public string OriginalUrl { get; set; } = string.Empty;
    public long? VehicleId { get; set; }
}