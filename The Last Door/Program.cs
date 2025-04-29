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
        for (int i = 0; i < game.roundsCount;)
        {
            // Выбираем дверь и спавним нового монстра
            if (!game.monster.IsAlive)
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
            if (!game.monster.IsAlive) continue;

            // Атакует монстр
            game.AttackMonster();
            Console.WriteLine($"Твое здоровье - {game.player.currentHealth}");
            Console.WriteLine($"Здоровье противника - {game.monster.currentHealth}");

            // Если игрок мёртв игра завершается
            if (!game.player.isAlive) game.GameOver();
        }

        // Если цикл завершился то игра закончена
        Console.WriteLine("Вы победили!");
        game.GameOver();
    }
}

class GameManager
{
    public int roundsCount = 5;

    public Player player = new Player();
    public Monster monster = new Monster();

    public void StartGame()
    {
        while (true)
        {
            player.currentHealth = player.maxHealth;
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
        foreach (var playerAbility in player.Abilities)
        {
            Console.WriteLine($"{index}. {playerAbility.description}");
            index++;
        }
    }

    public void CastSpell()
    {
        int abilityIndex = WaitForValidKey() - 1;

        //Todo Применение заклинания хила (костыль)
        if (abilityIndex == 2 && abilityIndex < player.Abilities.Count)
        {
            if (player.Abilities[abilityIndex].cooldown <= 0)
            {
                player.Abilities[abilityIndex].Use(player.attack, player);
                player.Abilities[abilityIndex].cooldown = player.Abilities[abilityIndex].maxCooldown;
            }
            else
            {
                Console.WriteLine(
                    $"\nЗаклинание не готово попробуйте через {player.Abilities[abilityIndex].cooldown} хода!");
                CastSpell();
            }
        }
        // Применение заклинания
        else if (abilityIndex >= 0 && abilityIndex < player.Abilities.Count)
        {
            if (player.Abilities[abilityIndex].cooldown <= 0)
            {
                player.Abilities[abilityIndex].Use(player.attack, monster);
                player.Abilities[abilityIndex].cooldown = player.Abilities[abilityIndex].maxCooldown;
            }
            else
            {
                Console.WriteLine(
                    $"\nЗаклинание не готово попробуйте через {player.Abilities[abilityIndex].cooldown} хода!");
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
        monster.IsAlive = true;
        monster.currentHealth = monster.maxHealth;
        monster.attack = monster.originalAttack;
        Random rnd = new Random();
        int index = rnd.Next(Monster.AllMonstersNames.Length);
        monster.name = Monster.AllMonstersNames[index];
        Console.WriteLine($"\nНа твоем пути {monster.name}, который готов напасть. Нанеси первый удар!");
    }

    public void AttackMonster()
    {
        // Применяет дебафы из списка, если они есть
        foreach (var debuff in monster.Debuffs)
        {
            debuff.ApplyDebuff(monster);
        }

        // Очищаем список дебаффов после применения
        monster.Debuffs.Clear();

        // Если монстр жив и не заморожен, проверяет на промах, наносит аттаку игроку
        if (monster.IsAlive && !monster.isFrozen)
        {
            Random rnd = new Random();
            int randomNumber = rnd.Next(1, 100);
            if (randomNumber <= player.evasion) Console.WriteLine($"\n{monster.name} промахнулся! Лох");
            else
            {
                player.currentHealth -= monster.attack;
                Console.WriteLine($"{monster.name} наносит {monster.attack} урона!");
            }
        }

        //todo Добавить метод который "сбрасывает" эфекты дебафа
        monster.isFrozen = false;
        monster.attack = monster.maxAttack;
    }

    public void UpdateCooldown()
    {
        foreach (var playerAbility in player.Abilities)
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