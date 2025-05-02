namespace The_Last_Door;

public class DB
{
    public static List<Monster> Monsters = new()
    {
        new Monster("Дракон", 200, 30, new List<string> { "fire" }),
        new Monster("Скелет", 80, 15, new List<string> { "ice" }),
        new Monster("Призрак", 60, 10, new List<string> { "physical", "fire" }),
        new Monster("Паук", 90, 12, new List<string>()), // без резистов
        new Monster("Голем", 150, 20, new List<string> { "ice", "fire" }),
        new Monster("Вампир", 100, 18, new List<string> { "dark" }),
        new Monster("Орк", 120, 22, new List<string>()),
        new Monster("Лич", 140, 25, new List<string> { "fire", "magic" }),
        new Monster("Ядовитый плющ", 110, 14, new List<string> { "poison" }),
        new Monster("Огненный элементаль", 130, 26, new List<string> { "fire" }),
        new Monster("Тролль", 180, 20, new List<string> { "poison" }),
        new Monster("Мимик", 100, 25, new List<string> { "magic" }),
        new Monster("Темный рыцарь", 160, 28, new List<string> { "physical", "ice" }),
        new Monster("Гигантская крыса", 70, 10, new List<string>()),
        new Monster("Архимаг", 90, 30, new List<string> { "fire", "magic" }),
        new Monster("Некромант", 110, 22, new List<string> { "dark", "fire" }),
        new Monster("Медуза", 120, 18, new List<string> { "ice", "poison" }),
        new Monster("Каменный змей", 130, 24, new List<string> { "physical" }),
        new Monster("Шипастый слизень", 95, 16, new List<string> { "poison", "ice" }),
        new Monster("Песчаный червь", 150, 21, new List<string> { "fire", "earth" })
    };
    
    public static Monster? GetRandomMonster()
    {
        if (Monsters.Count == 0) return null;

        Random rnd = new();
        return Monsters[rnd.Next(Monsters.Count)];
    }
    
    public static void RemoveMonster(Monster monster)
    {
        Monsters.Remove(monster);
    }
    
}