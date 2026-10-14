namespace DZ3;

public class Commands
{
    private readonly Player player;
    private readonly List<CommandType> commands = new();

    public Commands(Player player)
    {
        this.player = player;
        Build();
    }

    public IReadOnlyList<CommandType> All => commands;

    public void Run(string input)
    {
        string[] tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (tokens.Length == 0) return;

        CommandType? match = null;
        int matchLength = 0;

        foreach (var command in commands)
        {
            string[] nameTokens = command.name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (nameTokens.Length <= matchLength || nameTokens.Length > tokens.Length) continue;

            bool matches = true;
            for (int i = 0; i < nameTokens.Length; i++)
            {
                if (!string.Equals(nameTokens[i], tokens[i], StringComparison.OrdinalIgnoreCase))
                {
                    matches = false;
                    break;
                }
            }

            if (!matches) continue;
            match = command;
            matchLength = nameTokens.Length;
        }

        if (match is null)
        {
            Console.WriteLine("Unknown command. Type 'help'.");
            return;
        }

        string[] args = tokens[matchLength..];
        try
        {
            match.action(args);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void Build()
    {
        commands.Add(new CommandType("help", _ => Help()));
        commands.Add(new CommandType("clear", _ => Console.Clear()));

        commands.Add(new CommandType("player name show", _ => Console.WriteLine($"Name: {player.name}")));
        commands.Add(new CommandType("player name set", args =>
        {
            if (args.Length < 1) { Usage("<name>"); return; }
            player.name = args[0];
        }));

        commands.Add(new CommandType("player capacity show", _ =>
            Console.WriteLine($"Slots: {player.inventory.content.Count}/{player.inventory.Capacity}")));
        commands.Add(new CommandType("player capacity set", args =>
        {
            if (args.Length < 1 || !int.TryParse(args[0], out int slots)) { Usage("<slots>"); return; }
            player.inventory.Capacity = slots;
        }));

        AddScaleCommands("player health", () => player.health);
        AddScaleCommands("player mana", () => player.mana);
        AddScaleCommands("player energy", () => player.energy);

        commands.Add(new CommandType("player inventory add", args =>
        {
            if (!TryParseItem(args, out string name, out int quantity, out bool force)) return;
            if (player.inventory.Add(name, quantity, force))
                Console.WriteLine($"Added {quantity}x {name}.");
        }));

        commands.Add(new CommandType("player inventory remove", args =>
        {
            if (!TryParseItem(args, out string name, out int quantity, out bool force)) return;
            if (player.inventory.Remove(name, quantity, force))
                Console.WriteLine($"Removed {quantity}x {name}.");
        }));

        commands.Add(new CommandType("player inventory show", _ => player.inventory.Show()));
        commands.Add(new CommandType("player inventory hide", _ => Console.Clear()));

        commands.Add(new CommandType("player inventory use", args =>
        {
            if (args.Length < 1) { Usage("<id|name>"); return; }
            if (int.TryParse(args[0], out int id)) player.inventory.Use(id);
            else player.inventory.Use(args[0]);
        }));
    }

    private void AddScaleCommands(string prefix, Func<Scale> scale)
    {
        commands.Add(new CommandType($"{prefix} show", _ =>
            Console.WriteLine($"{prefix}: {scale()}")));

        commands.Add(new CommandType($"{prefix} set", args =>
            WithAmount(args, amount => scale().Current = amount)));

        commands.Add(new CommandType($"{prefix} add", args =>
            WithAmount(args, amount => scale().ChangeCurrent(amount))));

        commands.Add(new CommandType($"{prefix} remove", args =>
            WithAmount(args, amount => scale().ChangeCurrent(-amount))));

        commands.Add(new CommandType($"{prefix} max set", args =>
            WithAmount(args, amount => scale().Max = amount)));

        commands.Add(new CommandType($"{prefix} max add", args =>
            WithAmount(args, amount => scale().ChangeMax(amount))));
    }

    private void Help()
    {
        Console.WriteLine("Available commands:");
        foreach (var command in commands) Console.WriteLine($"  {command.name}");
    }

    private static void WithAmount(string[] args, Action<float> apply)
    {
        if (args.Length < 1 || !float.TryParse(args[0], out float amount)) { Usage("<amount>"); return; }
        apply(amount);
    }

    private static bool TryParseItem(string[] args, out string name, out int quantity, out bool force)
    {
        name = string.Empty;
        quantity = 0;
        force = false;

        if (args.Length < 2) { Usage("<name> <quantity> [force]"); return false; }
        name = args[0];
        if (!int.TryParse(args[1], out quantity)) { Usage("<name> <quantity> [force]"); return false; }
        if (quantity <= 0) { Console.WriteLine("Quantity must be a positive integer."); return false; }

        if (args.Length >= 3)
        {
            force = args[2].ToLowerInvariant() switch
            {
                "true" or "1" or "yes" or "y" or "force" => true,
                _ => false
            };
        }

        return true;
    }

    private static void Usage(string usage) => Console.WriteLine($"Usage: {usage}");
}
