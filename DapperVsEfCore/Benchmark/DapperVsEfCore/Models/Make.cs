namespace Benchy.DapperVsEfCore.Models;

public class Make : Entity
{
    public Make(long id, string name)
    {
        Id = id;
        Name = name;
    }

    public Make()
    {
    }

    public string Name { get; set; } = string.Empty;
}