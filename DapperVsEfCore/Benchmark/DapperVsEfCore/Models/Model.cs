namespace Benchy.DapperVsEfCore.Models;

public class Model : Entity
{
    public Model(long id, string name, long makeId)
    {
        Id = id;
        Name = name;
        MakeId = makeId;
    }

    public Model()
    {
    }

    public long MakeId { get; set; }
    public Make? Make { get; set; }
    public string Name { get; set; } = string.Empty;
}