namespace DZ3;

public class CommandType
{
    public string name;
    public Action<string[]> action;

    public CommandType(string name, Action<string[]> action)
    {
        this.name = name;
        this.action = action;
    }
}
