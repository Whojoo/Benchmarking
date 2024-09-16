namespace Benchy.DapperVsEfCore.Models;

public class Vehicle : Entity
{
    public Vehicle(
        long id,
        long modelId,
        long thumbnailId,
        string licensePlate,
        EngineDetails? engineDetails,
        decimal price,
        List<Image> images,
        List<DamageImage> damageImages)
    {
        Id = id;
        ModelId = modelId;
        ThumbnailId = thumbnailId;
        LicensePlate = licensePlate;
        EngineDetails = engineDetails;
        Price = price;
        DetailImages = images;
        DamageImages = damageImages;
    }

    public Vehicle()
    {

    }

    public long ModelId { get; set; }
    public Model? Model { get; set; }
    public List<Option> Options { get; set; } = [];
    public List<Tag> Tags { get; set; } = [];
    public List<Image> DetailImages { get; set; } = [];
    public List<DamageImage> DamageImages { get; set; } = [];
    public long ThumbnailId { get; set; }
    public Image? Thumbnail { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public long EngineDetailsId { get; set; }
    public EngineDetails? EngineDetails { get; set; }
    public decimal Price { get; set; }
}