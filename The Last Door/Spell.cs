namespace The_Last_Door;

public interface IAbility
{
    string description { get; set; }
    int maxCooldown { get; set; }
    int cooldown { get; set; }
    void Use(int playerAttack, ITarget target);
}

class Spell
{
    public int missChance = 5;
    public int maxCooldown { get; set; } = 1;
    public int cooldown { get; set; }

    public bool CheckMiss(int missChance)
    {
        Random rnd = new Random();
        int randomNumber = rnd.Next(1, 100);

        if (randomNumber <= missChance) return true;
        return false;
    }
}

class FireBall : Spell, IAbility
{
    public string description { get; set; } = "Запустить огненый шар!";

    // Конструктор
    public FireBall()
    {
        missChance += 3;
        maxCooldown += 1;
    }


    public void Use(int playerAttack, ITarget target = null)
    {
        int damage = playerAttack / 2 + maxCooldown;

        if (CheckMiss(missChance))
        {
            Console.WriteLine($"\nЭто было сложно, но вы промахнулись :( ");
            return;
        }

        Console.WriteLine($"\nОгонь охватывает все тело противника! И наносит {damage} урона!");
        target.currentHealth -= damage;
        if (target is Monster monster)
        {
            monster.Debuffs.Add(new Burn());
        }
    }
}

class FrozenTouch : Spell, IAbility
{
    public string description { get; set; } = "Потрогать холодными руками!";
    public int cooldown;

    public FrozenTouch()
    {
        missChance += 2;
        maxCooldown += 2;
    }

    public void Use(int playerAttack, ITarget target = null)
    {
        int damage = playerAttack + maxCooldown;

        if (CheckMiss(missChance))
        {
            Console.WriteLine($"\nВы только пощекотали {target.name}. Недостаточно холодные руки.");
            return;
        }

        Console.WriteLine($"\nХолод медленно расползаеется по телу! И наносит {damage} урона. " +
                          $"\nВраг не может пошевелится");
        target.currentHealth -= damage;
        if (target is Monster monster)
        {
            monster.Debuffs.Add(new Freeze());
        }
    }
}

class Heal : Spell, IAbility
{
    public string description { get; set; } = "Приложить к ране подорожник!";
    public int cooldown;

    public Heal()
    {
        missChance += 5;
        maxCooldown += 3;
    }

    public void Use(int playerAttack, ITarget target = null)
    {
        int amount = playerAttack;

        if (CheckMiss(missChance))
        {
            Console.WriteLine($"\nНаверняка тот гном-торговец был шарлатаном... это не сработало ");
        }

        Console.WriteLine($"\nУдивительно, но это работает! Ты востанавливаешь {amount} здоровья");
        target.currentHealth += amount;
        if (target.currentHealth > target.maxHealth) target.currentHealth = target.maxHealth;
    }
}

class StoneSpikes : Spell, IAbility
{
    public string description { get; set; } = "Вызвать каменные шипы!";
    public int cooldown;

    public StoneSpikes()
    {
        missChance += 1;
        maxCooldown += 3;
    }

    public void Use(int playerAttack, ITarget target = null)
    {
        int damage = playerAttack + 1 + maxCooldown;

        if (CheckMiss(missChance))
        {
            Console.WriteLine($"\nВы вскопали землю. Это не совсем то, но... можете посадить помидоры?");
            return;
        }

        Console.WriteLine($"\nШипы вонзаются в тело врага, ослабляя его! И наносят {damage} урона!");
        target.currentHealth -= damage;
        if (target is Monster monster)
        {
            monster.Debuffs.Add(new Weaken());
        }
    }
}