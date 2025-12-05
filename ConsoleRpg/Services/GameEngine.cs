using ConsoleRpg.Helpers;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;
using ConsoleRpgEntities.Models.Equipments;

namespace ConsoleRpg.Services;

public class GameEngine
{
    private readonly GameContext _context;
    private readonly MenuManager _menuManager;
    private readonly OutputManager _outputManager;
    private IPlayer _player;
    private IMonster _goblin;

    public GameEngine(GameContext context, MenuManager menuManager, OutputManager outputManager)
    {
        _menuManager = menuManager;
        _outputManager = outputManager;
        _context = context;
    }

    public void Run()
    {
        if (_menuManager.ShowMainMenu())
        {
            SetupGame();
        }
    }

    private void GameLoop()
    {
        _outputManager.Clear();

        while (true)
        {
            _outputManager.WriteLine("Choose an action:", ConsoleColor.Cyan);
            _outputManager.WriteLine("1. Attack");
            _outputManager.WriteLine("2. List Inventory");
            _outputManager.WriteLine("3. List Equipment");
            _outputManager.WriteLine("4. Search Inventory");
            _outputManager.WriteLine("5. Quit");

            _outputManager.Display();

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Attack();
                    break;
                case "2":
                    ShowInventory();
                    break;
                case "3":
                    ShowEquipment();
                    break;
                case "4":
                    SearchInventory();
                    break;
                case "5":
                    _outputManager.WriteLine("Exiting game...", ConsoleColor.Red);
                    _outputManager.Display();
                    Environment.Exit(0);
                    break;
                default:
                    _outputManager.WriteLine("Invalid selection. Please choose 1.", ConsoleColor.Red);
                    break;
            }
        }
    }

    private void Attack()
    {
        // _context.Entry(_goblin).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        if (_goblin is ITargetable targetableGoblin && targetableGoblin.Health > 0)
        {
            // At one point an issue started popping up with regard to the provided Equipment to Items relationship
            // and I'm no longer able to run the Attack simulation or query the equipment.

            // This suddenly occurred with what felt like no changes to any of the logic at all.

            // It worked consistently until I returned from a grocery shopping trip and now the actual
            // Weapon object on the Equipment model isn't returning the Sword from the items table anymore.

            // Before the sudden breakage the goblin was dropping loot perfectly fine and that's what's in the inventory on the DB now.
            // You can see the logic I was working on testing with Equipment when this blockage occurred.
            _player.Attack(targetableGoblin);
            _player.UseAbility(_player.Abilities.First(), targetableGoblin);
            _outputManager.WriteLine($"{targetableGoblin.Name} has {targetableGoblin.Health} health remaining.");

            if (targetableGoblin.Health <= 0)
            {
                var loot = _context.Items.ToList()[new Random().Next(_context.Items.ToList().Count())];
                _outputManager.WriteLine($"As {_player.Name} deals the final blow, the dying {targetableGoblin.Name} drops an item to the floor: {loot.Name}");
                _player.LootItem(loot);

                // Prevent actually killing this goblin as the assignment is based around inventory and equipment
                _context.Entry(_goblin).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                _context.SaveChanges();
                _context.Entry(_goblin).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                _outputManager.WriteLine($"A passing necromancer cackles as he twirls his fingers in the air.");
                _outputManager.WriteLine($"No one defeats {targetableGoblin.Name}");
                LoadMonsters();
            }
        }
    }

    private void ShowInventory()
    {
        var inventory = _context.Items.Where(item => item.InventoryId == _player.Inventory.Id);

        Console.WriteLine("Choose a sorting option: (or just enter for no sorting)");
        Console.WriteLine("1. Sort by Attack Asc");
        Console.WriteLine("2. Sort by Attack Desc");
        Console.WriteLine("3. Sort by Defense Asc");
        Console.WriteLine("4. Sort by Defense Desc");

        var sortingOptions = Console.ReadLine();

        switch (sortingOptions)
        {
            case "1": inventory = inventory.OrderBy(item => item.Attack); break;
            case "2": inventory = inventory.OrderByDescending(item => item.Attack); break;
            case "3": inventory = inventory.OrderBy(item => item.Defense); break;
            case "4": inventory = inventory.OrderByDescending(item => item.Defense); break;
            default: break;
        }

        _outputManager.WriteLine($"{_player.Name} is carrying the following items in their inventory: ");

        _outputManager.WriteLine("__________\n");
        foreach (Item item in inventory.ToList())
        {
            _outputManager.WriteLine($"{item.Name}");
            _outputManager.WriteLine($"{item.Type}");
            _outputManager.WriteLine($"Attack: {item.Attack}");
            _outputManager.WriteLine($"Defense: {item.Defense}");
            _outputManager.WriteLine($"Weight: {item.Weight}");
            _outputManager.WriteLine($"Value: {item.Value}");
            _outputManager.WriteLine("__________\n");
        }
    }

    private void SearchInventory()
    {
        var inventory = _context.Items.Where(item => item.InventoryId == _player.Inventory.Id).ToList();

        var searching = "y";

        Console.WriteLine("Search for a weapon by name or type: ");

        var searchTerm = Console.ReadLine();
        do
        {
            Console.WriteLine($"{_player.Name} is carrying the following items in their inventory: ");

            var searchResults = inventory.Where(item => (
                item.Name.Contains(searchTerm)
                ||
                item.Type.Contains(searchTerm)
                )).ToList();

            if (searchResults.Count() > 0)
            {
                Console.WriteLine("__________\n");
                foreach (Item item in searchResults)
                {
                    Console.WriteLine($"{item.Name}");
                    Console.WriteLine($"{item.Type}");
                    Console.WriteLine($"Attack: {item.Attack}");
                    Console.WriteLine($"Defense: {item.Defense}");
                    Console.WriteLine($"Weight: {item.Weight}");
                    Console.WriteLine($"Value: {item.Value}");
                    Console.WriteLine("__________\n");
                }
            }
            else
            {
                Console.WriteLine("No items matched your query.");
            }

            searching = "n";
        } while (searching.Equals("y"));
    }

    private void ShowEquipment()
    {
        // There's some additional lore above as to why this isn't running.

        // Sudden breakage to the _player.Equipment -> Items table relationship without any apparent
        // code changes has me stopping here.

        // I get frustrated enough at other little things in these templates so I"m choosing this as a natural stopping point.

        // I don't know if something snuck it's way or it was something else but whatever.

        // I would never want to show a recruiter any of what we did in this class and that makes focusing on it harder.
        var weapon = _player.Equipment.Weapon;
        var armor = _player.Equipment.Armor;

        Console.WriteLine($"Weapon: {weapon.Name ?? "Unequipped"}");
        Console.WriteLine($"Armor: {armor.Name ?? "Unequipped"}");
    }

    private void EquipItem()
    {
        // Currentlly broken due to the faulty and ridiculous relationship forced onto us by the assignment template
    }

    private void SetupGame()
    {
        _player = _context.Players.FirstOrDefault();
        _outputManager.WriteLine($"{_player.Name} has entered the game.", ConsoleColor.Green);

        // Load monsters into random rooms 
        LoadMonsters();

        // Pause before starting the game loop
        Thread.Sleep(500);
        GameLoop();
    }

    private void LoadMonsters()
    {
        _goblin = _context.Monsters.OfType<Goblin>().FirstOrDefault();
        // Avoid updating the Goblin's health in teh database, realistically it should have a second field
        // that indicates it's current health vs it's total health.
        // Monsters tend to respawn in games.
        _context.Entry(_goblin).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
    }

}
