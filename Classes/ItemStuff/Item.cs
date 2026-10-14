namespace DZ3;

public class Item
{
    public int id;
    public string name;
    public float price;

    public Item(int id, string name, float price)
    {
        this.id = id;
        this.name = name;
        this.price = price;
    }

    public void Use()
    {
        Console.WriteLine($"You use {name}.");
    }

    public static readonly List<Item> All = new()
    {
        new Item(1, "Stick", 1.0f),
        new Item(2, "Apple", 2.5f),
        new Item(3, "Steak", 8.0f),
        new Item(4, "Sword", 120.0f),
        new Item(5, "Machete", 90.0f),
        new Item(6, "War Axe", 200.0f),
        new Item(7, "Magic Spell", 500.0f),
        new Item(1488, "one shot stick that can also be shoved up into someone's ahh", 1000000000000000.0f),
    };

    public static Item? GetById(int id) => All.Find(x => x.id == id);

    public static Item? GetByName(string name) =>
        All.Find(x => string.Equals(x.name, name, StringComparison.OrdinalIgnoreCase));

    public static Item GetByNameOrThrow(string name) =>
        GetByName(name) ?? throw new ArgumentException($"Unknown item {name}", nameof(name));
}
