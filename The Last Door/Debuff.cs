namespace The_Last_Door;

public interface IDebuff
{
    public void ApplyDebuff(ITarget target);
}

public class Burn : IDebuff
{
    public void ApplyDebuff(ITarget target)
    {
        int burnDamage = 5;
        target.currentHealth -= burnDamage;
        Console.WriteLine($"{target.name} в огне! И получает ещё {burnDamage} урона!");
    }
}

public class Freeze : IDebuff
{
    public void ApplyDebuff(ITarget target)
    {
        target.isFrozen = true;
        Console.WriteLine($"{target.name} все ещё льдышка! Пропускает ход");
    }
}

public class Weaken : IDebuff
{
    public void ApplyDebuff(ITarget target)
    {
        target.attack = target.attack / 2;
        Console.WriteLine($"{target.name} слабовато бьет! Атака -50%");
    }
}