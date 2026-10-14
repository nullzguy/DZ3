namespace DZ3;

public class Inventory
{
    private int capacity = 10;

    public int Capacity
    {
        get => capacity;
        set => capacity = Math.Max(1, value);
    }

    public List<Items> content = new();

    public bool Add(string itemName, int quantity, bool force = false)
    {
        Item item = Item.GetByNameOrThrow(itemName);
        return Add(item, quantity, force);
    }

    public bool Add(Item item, int quantity, bool force = false)
    {
        if (quantity <= 0)
        {
            Console.WriteLine("Quantity must be positive.");
            return false;
        }

        int index = content.FindIndex(x => x.item.id == item.id);

        if (index >= 0)
        {
            content[index].quantity += quantity;
            return true;
        }

        if (content.Count >= Capacity && !force)
        {
            Console.WriteLine("Inventory is full.");
            return false;
        }

        content.Add(new Items(item, quantity));
        return true;
    }

    public bool Remove(string itemName, int quantity, bool force = false)
    {
        if (quantity <= 0)
        {
            Console.WriteLine("Quantity must be positive.");
            return false;
        }

        int index = content.FindIndex(x =>
            string.Equals(x.item.name, itemName, StringComparison.OrdinalIgnoreCase));
        if (index < 0) return false;

        int available = content[index].quantity;
        if (!force && available < quantity) return false;

        int howMuchToRemove = Math.Min(quantity, available);
        if (howMuchToRemove == available) content.RemoveAt(index);
        else content[index].quantity -= howMuchToRemove;
        return true;
    }

    public bool Use(string? itemName)
    {
        if (itemName is null) return false;

        Item? item = Item.GetByName(itemName);
        if (item is null)
        {
            Console.WriteLine($"Unknown item {itemName}.");
            return false;
        }

        return Use(item.id);
    }
    
    public bool Use(int id)
    {
        int index = content.FindIndex(x => x.item.id == id);

        if (index < 0)
        {
            Console.WriteLine($"No item with id {id} in inventory.");
            return false;
        }

        content[index].item.Use();

        content[index].quantity--;
        if (content[index].quantity <= 0) content.RemoveAt(index);
        return true;
    }

    public void Show()
    {
        const int idW = 4, nameW = 12, priceW = 10, qtyW = 10;
        string border = "   +" + new string('-', idW + nameW + priceW + qtyW + 11) + "+";

        Console.WriteLine("       -Inventory-");
        Console.WriteLine($"      Slots: {content.Count}/{Capacity}");
        Console.WriteLine(border);
        Console.WriteLine($"   | {"ID",-idW} | {"NAME",-nameW} | {"PRICE",-priceW} | {"QTY",-qtyW} |");
        Console.WriteLine(border);

        foreach (var stack in content)
        {
            Console.WriteLine($"   | {stack.item.id,-idW} | {stack.item.name,-nameW} | {stack.item.price,-priceW} | {stack.quantity,-qtyW} |");
        }

        Console.WriteLine(border);
    }
}
