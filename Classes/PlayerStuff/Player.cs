namespace DZ3;

public class Player
{
    public string name;
    public Scale health;
    public Scale mana;
    public Scale energy;
    public Inventory inventory = new();

    public Player(string name, float health, float mana, float energy)
    {
        this.name = name;
        this.health = new Scale(health);
        this.mana = new Scale(mana);
        this.energy = new Scale(energy);
    }
}
