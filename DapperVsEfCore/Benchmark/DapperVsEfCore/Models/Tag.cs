namespace Benchy.DapperVsEfCore.Models;

public class Tag : Entity
{
    public Tag(long id, string name, string code)
    {
        Id = id;
        Name = name;
        Code = code;
    }

    public Tag()
    {
    }

    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}