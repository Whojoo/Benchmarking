namespace Benchy.DapperVsEfCore.Models;

public class Option : Entity
{
    public Option(long id, string name, string code, bool isActive)
    {
        Id = id;
        Name = name;
        Code = code;
        IsActive = isActive;
    }

    public Option()
    {
    }

    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}