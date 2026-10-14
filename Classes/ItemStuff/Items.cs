namespace DZ3;

public class Items
{
    public Item item;
    public int quantity;

    public Items(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public Items(string itemName, int quantity) : this(Item.GetByNameOrThrow(itemName), quantity)
    {
    }
}
