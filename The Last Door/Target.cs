namespace The_Last_Door;

public interface ITarget
{
    int maxHealth { get; set; }
    int currentHealth { get; set; }
    string name { get; set; }
    int attack { get; set; }
    bool isFrozen { get; set; }
}

public class Player : ITarget
{
    public string name { get; set; } = "Стиви";
    public int maxHealth { get; set; } = 100;
    public int currentHealth { get; set; }
    public int attack { get; set; } = 5;
    public bool isAlive = true;
    public bool isFrozen { get; set; } = false;
    public int evasion = 10;

    public List<IAbility> Abilities =
    [
        new FireBall(),
        new FrozenTouch(),
        new Heal(),
        new StoneSpikes()
    ];

    public void Die()
    {
        if (currentHealth <= 0)
        {
            Console.Write("Вы мертвы");
            isAlive = false;
        }
    }
}

public class Monster : ITarget
{
    public string name { get; set; }
    public int maxHealth { get; set; } = 100;
    private int _currentHealth;
    public int currentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = value;
            if (_currentHealth <= 0) Die();
        }
    }

    public int attack { get; set; } = 10;
    public int maxAttack = 10;
    public int originalAttack = 10;
    public bool IsAlive = false;
    public bool isFrozen { get; set; } = false;

    public static string[] AllMonstersNames =
    {
        "Паук",
        "Скелет",
        "Дракон",
        "Призрак",
        "Ядовитый плющ"
    };

    public List<IDebuff> Debuffs { get; } = new List<IDebuff>();

    public void Die()
    {
        Console.Write($"{name} погиб!");
        IsAlive = false;
    }
}

public class Boss : Monster
{
}