namespace DZ3;

// Шкала це показник поточного і максимального рівня (здоров'я, мани, енергії).
public class Scale
{
    private float current;
    private float max;

    public float Current
    {
        get => current;
        set => current = Math.Clamp(value, 0.0f, max);
    }

    public float Max
    {
        get => max;
        set
        {
            max = Math.Max(0.0f, value);
            if (current > max) current = max;
        }
    }

    public Scale(float max)
    {
        Max = max;
        Current = max;
    }

    public void ChangeCurrent(float delta) => Current += delta;

    public void ChangeMax(float delta) => Max += delta;

    public override string ToString() => $"{(int)Current}/{(int)Max}";
}
