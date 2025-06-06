using System.Collections.Generic;
using System.Linq;
using Godot;

/// <summary>
/// Example demonstrating how to use the ModuleManager system.
/// This example shows how to create modules, submodules, and access them through the ModuleManager.
/// </summary>
public partial class ModuleManagerExample : Node
{
    public override void _Ready()
    {
        GD.Print("=== ModuleManager Example ===");

        // The ModuleManager automatically scans and loads modules on startup
        // Let's demonstrate accessing modules through the manager

        // Get the GameModule (should be auto-loaded)
        var gameModule = ModuleManager.Instance.GetModule<GameModule>();
        if (gameModule != null)
        {
            GD.Print($"Found GameModule: {gameModule.Name}");
            GD.Print($"Game Module Status: {gameModule.GetStatus()}");
        }
        else
        {
            GD.Print("GameModule not found - it may not be auto-loaded");
        }

        // Get the InventoryModule
        var inventoryModule = ModuleManager.Instance.GetModule<InventoryModule>();
        if (inventoryModule != null)
        {
            GD.Print($"Found InventoryModule: {inventoryModule.Name}");
            inventoryModule.AddItem("Health Potion");
            inventoryModule.AddItem("Magic Sword");
            GD.Print($"Inventory items: {inventoryModule.GetItemCount()}");
        }

        // Get submodules for the GameModule
        var gameSubmodules = ModuleManager.Instance.GetSubmodules<GameModule>();
        GD.Print($"GameModule has {gameSubmodules.Count} submodules:");

        foreach (var submodule in gameSubmodules)
        {
            GD.Print($"  - {submodule.Name}");
        }

        // Demonstrate accessing a specific submodule
        PlayerStatsSubmodule playerStatsSubmodule = null;
        foreach (var submodule in gameSubmodules)
        {
            if (submodule is PlayerStatsSubmodule stats)
            {
                playerStatsSubmodule = stats;
                break;
            }
        }
        if (playerStatsSubmodule != null)
        {
            playerStatsSubmodule.AddExperience(100);
            GD.Print($"Player Level: {playerStatsSubmodule.GetLevel()}");
        }
    }
}

// Example Module 1: GameModule
[Module("GameModule", autoLoad: true, loadOrder: 1)]
public partial class GameModule : Node, IModule
{
    public string Name { get; private set; } = "GameModule";
    public bool IsInitialized { get; private set; }

    private bool _isGameRunning;

    public void Initialize()
    {
        GD.Print($"Initializing {Name}");
        _isGameRunning = true;
        IsInitialized = true;
    }

    public void Cleanup()
    {
        GD.Print($"Cleaning up {Name}");
        _isGameRunning = false;
        IsInitialized = false;
    }

    public string GetStatus()
    {
        return _isGameRunning ? "Running" : "Stopped";
    }
}

// Example Module 2: InventoryModule
[Module("InventoryModule", autoLoad: true, loadOrder: 2)]
public class InventoryModule : IModule
{
    public string Name { get; private set; } = "InventoryModule";
    public bool IsInitialized { get; private set; }

    private readonly List<string> _items = new();

    public void Initialize()
    {
        GD.Print($"Initializing {Name}");
        IsInitialized = true;
    }

    public void Cleanup()
    {
        GD.Print($"Cleaning up {Name}");
        _items.Clear();
        IsInitialized = false;
    }

    public void AddItem(string item)
    {
        _items.Add(item);
        GD.Print($"Added item: {item}");
    }

    public int GetItemCount()
    {
        return _items.Count;
    }
}

// Example Submodule 1: PlayerStatsSubmodule
[Submodule(typeof(GameModule), autoLoad: true, loadOrder: 1)]
public partial class PlayerStatsSubmodule : Node, ISubmodule
{
    public string Name { get; private set; } = "PlayerStatsSubmodule";
    public bool IsInitialized { get; private set; }
    public IModule ParentModule { get; set; }

    private int _level = 1;
    private int _experience = 0;

    public void Initialize()
    {
        GD.Print($"Initializing {Name} (Parent: {ParentModule?.Name})");
        IsInitialized = true;
    }

    public void Cleanup()
    {
        GD.Print($"Cleaning up {Name}");
        IsInitialized = false;
    }

    public void AddExperience(int exp)
    {
        _experience += exp;
        // Simple leveling system
        while (_experience >= _level * 100)
        {
            _experience -= _level * 100;
            _level++;
            GD.Print($"Level up! Now level {_level}");
        }
    }

    public int GetLevel()
    {
        return _level;
    }
}

// Example Submodule 2: SaveSystemSubmodule
[Submodule(typeof(GameModule), autoLoad: true, loadOrder: 2)]
public class SaveSystemSubmodule : ISubmodule
{
    public string Name { get; private set; } = "SaveSystemSubmodule";
    public bool IsInitialized { get; private set; }
    public IModule ParentModule { get; set; }

    public void Initialize()
    {
        GD.Print($"Initializing {Name} (Parent: {ParentModule?.Name})");
        IsInitialized = true;
    }

    public void Cleanup()
    {
        GD.Print($"Cleaning up {Name}");
        IsInitialized = false;
    }

    public void SaveGame()
    {
        GD.Print("Game saved!");
    }

    public void LoadGame()
    {
        GD.Print("Game loaded!");
    }
}
