namespace DZ3;

internal class Program
{
    public static void Main()
    {
        Player player = new("Hero", 100.0f, 100.0f, 100.0f);
        Commands commands = new(player);

        Console.WriteLine("Type 'help' for a list of commands, 'exit' to quit.");
        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (input is null) break;

            string trimmed = input.Trim();
            if (trimmed.Length == 0) continue;
            if (trimmed.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

            commands.Run(trimmed);
        }
    }
}
