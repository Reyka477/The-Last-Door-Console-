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
    public int basicAttack = 5;
    public int evasion = 10;
    public int attack { get; set; }
    public bool isAlive = true;
    public bool isFrozen { get; set; } = false;

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
            Console.WriteLine("\nВы мертвы");
            isAlive = false;
        }
    }
}

public class Monster : ITarget
{
    public Monster(string name, int maxHealth, int baseAttack, List<string>? resistances = null)
    {
        this.name = name;
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth;
        this.BaseAttack = baseAttack;
        this.Resistances = resistances ?? []; 
    } 
    public string name { get; set; }
    public int maxHealth { get; set; }
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
    public int BaseAttack;
    public int attack { get; set; } = 10;
    public bool IsAlive = true;
    public bool isFrozen { get; set; } = false;
    public List<string> Resistances = [];

    public List<IDebuff> Debuffs { get; } = new List<IDebuff>();

    public void Die()
    {
        Console.Write($"{name} погиб!");
        IsAlive = false;
        DB.RemoveMonster(this);
    }
}
