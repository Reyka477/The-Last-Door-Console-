using System.Security.Cryptography;

namespace The_Last_Door;

class Program
{
    static void Main()
    {
        GameManager game = new GameManager();

        Console.WriteLine($"Добро пожаловать в игру \"Последняя дверь\" \nДля старта нажмите 1");
        game.StartGame();

        // Количество монстров зависит от количества раундов
        for (int i = 0; i < game.RoundsCount;)
        {
            // Выбираем дверь и спавним нового монстра
            if (!game.Monster.IsAlive)
            {
                game.ChooseTreDoor();
                game.SpawnMonster();
                i++;
            }

            // Герой выбирает действие
            game.AskNextMove();

            // Каст заклинания
            game.CastSpell();

            // Уменьшаем кд у заклинаний
            game.UpdateCooldown();

            // Если монстр умирает переходим к следующему
            if (!game.Monster.IsAlive) continue;

            // Атакует монстр
            game.AttackMonster();
            Console.WriteLine($"Твое здоровье - {game.Player.currentHealth}");
            Console.WriteLine($"Здоровье противника - {game.Monster.currentHealth}");

            // Если игрок мёртв игра завершается
            if (!game.Player.isAlive) game.GameOver();
        }

        // Если цикл завершился то игра закончена
        Console.WriteLine("Вы победили!");
        game.GameOver();
    }
}

class GameManager
{
    // Количество раундов
    public int RoundsCount = 5;

    public Player Player = new Player();
    public Monster Monster = DB.GetRandomMonster();
    
    public void StartGame()
    {
        while (true)
        {
            Player.currentHealth = Player.maxHealth;
            Player.attack = Player.basicAttack;
            if (WaitForValidKey() == 1)
            {
                Console.WriteLine($"\nИгра началась! Выберите дверь");
                break;
            }
            else
            {
                Console.WriteLine($"\nНеверная клавиша. Попробуйте снова.");
            }
        }
    }

    public int WaitForValidKey()
    {
        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            switch (keyInfo.Key)
            {
                case ConsoleKey.D1:
                    return 1;
                case ConsoleKey.D2:
                    return 2;
                case ConsoleKey.D3:
                    return 3;
                case ConsoleKey.D4:
                    return 4;
                default:
                    Console.WriteLine($"\nНеверная клавиша. Попробуйте снова.");
                    break;
            }
        }
    }

    public void AskNextMove()
    {
        // Выводим список доступных действий
        int index = 1;
        foreach (var playerAbility in Player.Abilities)
        {
            Console.WriteLine($"{index}. {playerAbility.description}");
            index++;
        }
    }

    public void CastSpell()
    {
        int abilityIndex = WaitForValidKey() - 1;

        if (Player.Abilities[abilityIndex] is Spell)
        {
            if (Player.Abilities[abilityIndex].cooldown <= 0)
            {
                if (Player.Abilities[abilityIndex] is Heal)
                {
                    Player.Abilities[abilityIndex].Use(Player.attack, Player);
                    Player.Abilities[abilityIndex].cooldown = Player.Abilities[abilityIndex].maxCooldown;
                }
                else
                {
                    Player.Abilities[abilityIndex].Use(Player.attack, Monster);
                    Player.Abilities[abilityIndex].cooldown = Player.Abilities[abilityIndex].maxCooldown;
                }
            }
            else
            {
                Console.WriteLine(
                    $"\nЗаклинание не готово попробуйте через {Player.Abilities[abilityIndex].cooldown} хода!");
                CastSpell();
            }
        }
        else
        {
            Console.WriteLine("\nНеверный выбор заклинания.");
        }
    }

    public void ChooseTreDoor()
    {
        string[] allDoors =
        {
            "Красная",
            "Синяя",
            "Зеленая",
            "Желтая",
            "Фиолетовая",
            "Белая"
        };
        List<int> doors = [];
        Random random = new Random();

        while (doors.Count < 3)
        {
            int number = random.Next(0, allDoors.Length);

            if (!doors.Contains(number))
            {
                doors.Add(number);
            }
        }

        Console.WriteLine();
        for (int i = 0; i < doors.Count; i++)
        {
            Console.WriteLine($"{i + 1}.{allDoors[doors[i]]} дверь");
        }

        WaitForValidKey();
    }

    public void SpawnMonster()
    {
        DB.GetRandomMonster();
        Console.WriteLine($"\nНа твоем пути {Monster.name}, который готов напасть. Нанеси первый удар!");
    }

    public void AttackMonster()
    {
        // Применяет дебафы из списка, если они есть
        foreach (var debuff in Monster.Debuffs)
        {
            debuff.ApplyDebuff(Monster);
        }

        // Очищаем список дебаффов после применения
        Monster.Debuffs.Clear();

        // Если монстр жив и не заморожен, проверяет на промах, наносит аттаку игроку
        if (Monster.IsAlive && !Monster.isFrozen)
        {
            Random rnd = new Random();
            int randomNumber = rnd.Next(1, 100);
            if (randomNumber <= Player.evasion) Console.WriteLine($"\n{Monster.name} промахнулся! Лох");
            else
            {
                Player.currentHealth -= Monster.attack;
                Console.WriteLine($"{Monster.name} наносит {Monster.attack} урона!");
            }
        }

        //todo Добавить метод который "сбрасывает" эфекты дебафа
        Monster.isFrozen = false;
        Monster.attack = Monster.BaseAttack;
        //todo Сделать нормальную проверку на смерть игрока
        Player.Die();
    }

    public void UpdateCooldown()
    {
        foreach (var playerAbility in Player.Abilities)
        {
            playerAbility.cooldown -= 1;
        }
    }

    public void GameOver()
    {
        Console.WriteLine("\nВот и пу пу пу. Хотите прокатится по хуям ещё раз?" +
                          "\n1. Да, конечно!" +
                          "\n2. Оооо дааааааа!" +
                          "\n3. Нет, говно игра." +
                          "\n4. Нет, мама кушать зовет :(");
        int choice = WaitForValidKey();
        if (choice == 1 || choice == 2)
        {
            StartGame();
            return;
        }

        if (choice == 3 || choice == 4)
        {
            Console.WriteLine($"\nНе ну по фактам :(");
        }
    }
}