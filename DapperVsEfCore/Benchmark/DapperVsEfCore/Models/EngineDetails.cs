namespace Benchy.DapperVsEfCore.Models;

public class EngineDetails : Entity
{
    public EngineDetails(long id, int pk, int maxSpeed, int numberOfCilinders)
    {
        Id = id;
        Pk = pk;
        MaxSpeed = maxSpeed;
        NumberOfCilinders = numberOfCilinders;
    }

    public EngineDetails()
    {
    }

    public int Pk { get; set; }
    public int MaxSpeed { get; set; }
    public int NumberOfCilinders { get; set; }
}