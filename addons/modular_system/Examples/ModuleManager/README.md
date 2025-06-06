# ModuleManager Example

This example demonstrates how to use the **ModuleManager** system in the Godot Modular System addon. The ModuleManager is the core component that automatically discovers, loads, and manages modules and submodules in your application.

## Overview

The [`ModuleManager`](../../Scripts/Core/ModuleManager.cs) provides:

- **Automatic Discovery**: Scans assemblies for classes decorated with [`[Module]`](../../Scripts/Attributes/ModuleAttribute.cs) and [`[Submodule]`](../../Scripts/Attributes/SubmoduleAttribute.cs) attributes
- **Singleton Access**: Global access point via `ModuleManager.Instance`
- **Module Retrieval**: Get specific modules using `GetModule<T>()`
- **Submodule Management**: Access submodules via `GetSubmodules<T>()`
- **Lifecycle Management**: Handles initialization and cleanup automatically

## Example Structure

The example includes:

1. **ModuleManagerExample**: Main demonstration script
2. **GameModule**: A Node-based module that gets added to the scene tree
3. **InventoryModule**: A simple POCO module for managing items
4. **PlayerStatsSubmodule**: A Node-based submodule attached to GameModule
5. **SaveSystemSubmodule**: A POCO submodule for save/load functionality

## How It Works

### 1. Module Declaration

Modules are declared using the [`[Module]`](../../Scripts/Attributes/ModuleAttribute.cs) attribute:

```csharp
[Module("GameModule", autoLoad: true, loadOrder: 1)]
public partial class GameModule : Node, IModule
{
    public string Name { get; private set; } = "GameModule";
    public bool IsInitialized { get; private set; }
    
    public void Initialize() { /* initialization code */ }
    public void Cleanup() { /* cleanup code */ }
}
```

**Attribute Parameters:**
- `name`: Optional display name for the module
- `autoLoad`: Whether to automatically load the module (default: true)
- `loadOrder`: Order in which modules are loaded (lower numbers load first)

### 2. Submodule Declaration

Submodules are declared using the [`[Submodule]`](../../Scripts/Attributes/SubmoduleAttribute.cs) attribute:

```csharp
[Submodule(typeof(GameModule), autoLoad: true, loadOrder: 1)]
public partial class PlayerStatsSubmodule : Node, ISubmodule
{
    public string Name { get; private set; } = "PlayerStatsSubmodule";
    public bool IsInitialized { get; private set; }
    public IModule ParentModule { get; set; }
    
    public void Initialize() { /* initialization code */ }
    public void Cleanup() { /* cleanup code */ }
}
```

**Attribute Parameters:**
- `parentModuleType`: The Type of the parent module
- `autoLoad`: Whether to automatically load the submodule (default: true)
- `loadOrder`: Order in which submodules are loaded within their parent

### 3. Accessing Modules

Once the ModuleManager has loaded modules, you can access them:

```csharp
// Get a specific module
var gameModule = ModuleManager.Instance.GetModule<GameModule>();

// Get all submodules for a parent module
var gameSubmodules = ModuleManager.Instance.GetSubmodules<GameModule>();

// Find a specific submodule
PlayerStatsSubmodule playerStats = null;
foreach (var submodule in gameSubmodules)
{
    if (submodule is PlayerStatsSubmodule stats)
    {
        playerStats = stats;
        break;
    }
}
```

## Module Types

### Node-based Modules
- Inherit from Godot's `Node` class
- Automatically added to the scene tree
- Can have visual components or child nodes
- Example: `GameModule`, `PlayerStatsSubmodule`

### POCO Modules
- Plain C# classes that implement [`IModule`](../../Scripts/Interfaces/IModule.cs)
- Managed in memory only
- Ideal for data management and business logic
- Example: `InventoryModule`, `SaveSystemSubmodule`

## Lifecycle

The ModuleManager handles the complete lifecycle:

1. **Discovery**: Scans all loaded assemblies for decorated classes
2. **Registration**: Creates instances of modules with `AutoLoad = true`
3. **Initialization**: Calls `Initialize()` on all modules (main modules first, then submodules)
4. **Runtime**: Modules are available via `GetModule<T>()` and `GetSubmodules<T>()`
5. **Cleanup**: Calls `Cleanup()` on shutdown (submodules first, then main modules)

## Running the Example

1. Add the `ModuleManagerExample` script to a Node in your scene
2. The modules and submodules will be automatically discovered and loaded
3. Check the output console to see the initialization sequence and example functionality

## Expected Output

When you run the example, you should see output similar to:

```
=== ModuleManager Example ===
Initializing GameModule
Initializing InventoryModule
Initializing PlayerStatsSubmodule (Parent: GameModule)
Initializing SaveSystemSubmodule (Parent: GameModule)
Found GameModule: GameModule
Game Module Status: Running
Found InventoryModule: InventoryModule
Added item: Health Potion
Added item: Magic Sword
Inventory items: 2
GameModule has 2 submodules:
  - PlayerStatsSubmodule
  - SaveSystemSubmodule
Level up! Now level 2
Player Level: 2
```

## Best Practices

1. **Load Order**: Use the `loadOrder` parameter to control initialization sequence
2. **Error Handling**: The ModuleManager catches and logs initialization errors
3. **Singleton Pattern**: Access modules through `ModuleManager.Instance` rather than direct instantiation
4. **Interface Segregation**: Keep module interfaces focused and specific
5. **Dependency Management**: Use submodules for functionality that depends on a specific parent module

## Integration with ServiceLocator

Node-based modules are automatically registered with the [`ServiceLocator`](../../Scripts/Core/ServiceLocator.cs) system, allowing dependency injection throughout your application.

## See Also

- [Module Interfaces](../../Scripts/Interfaces/)
- [Module Attributes](../../Scripts/Attributes/)
- [Service Locator Example](../ServiceLocator/)
- [Getting Started Guide](../../../docs/getting-started/quick-start.md)
