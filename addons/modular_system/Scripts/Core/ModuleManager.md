# ModuleManager

The [`ModuleManager`](ModuleManager.cs) is the central orchestrator of the modular system, responsible for discovering, loading, and managing modules and submodules throughout the application lifecycle.

## Overview

This singleton class automatically scans assemblies for types decorated with [`ModuleAttribute`](../Attributes/ModuleAttribute.cs) and [`SubmoduleAttribute`](../Attributes/SubmoduleAttribute.cs), instantiates them based on their configuration, and manages their lifecycle.

## Key Features

- **Automatic Discovery**: Scans all loaded assemblies for modules and submodules
- **Singleton Pattern**: Ensures only one instance manages the entire system
- **Load Order Support**: Respects load order specified in attributes
- **Lifecycle Management**: Handles initialization and cleanup of all modules
- **Service Integration**: Registers modules with the ServiceLocator
- **Error Handling**: Graceful error handling with logging

## Core Methods

### Module Access
```csharp
public T GetModule<T>() where T : class, IModule
```
Retrieves a specific module instance by type.

```csharp
public IReadOnlyList<ISubmodule> GetSubmodules<T>() where T : class, IModule
```
Gets all submodules associated with a parent module type.

### Internal Operations

#### [`ScanForModules()`](ModuleManager.cs:43)
Discovers and registers all modules/submodules across all loaded assemblies.

**Process:**
1. Iterates through all assemblies in the current AppDomain
2. Skips previously scanned assemblies to avoid duplicates
3. Calls [`ScanAssemblyForModules()`](ModuleManager.cs:66) for each assembly
4. Handles exceptions gracefully with error logging
5. Calls [`InitializeModules()`](ModuleManager.cs:169) after all scanning is complete

#### [`RegisterModule(Type)`](ModuleManager.cs:94)
Creates and registers individual modules with AutoLoad enabled.

**Process:**
1. Checks if module has `AutoLoad = true` in its [`ModuleAttribute`](../Attributes/ModuleAttribute.cs)
2. Creates instance using `Activator.CreateInstance()`
3. Sets module name from attribute if not already defined
4. Stores module in `_modules` dictionary using its type as key
5. For Node-based modules:
   - Registers with [`ServiceLocator`](ServiceLocator.cs)
   - Sets node name to module name
   - Adds as child to ModuleManager

#### [`RegisterSubmodule(Type)`](ModuleManager.cs:127)
Creates and registers submodules with their parent modules.

**Process:**
1. Checks if submodule has `AutoLoad = true` in its [`SubmoduleAttribute`](../Attributes/SubmoduleAttribute.cs)
2. Creates instance using `Activator.CreateInstance()`
3. Links submodule to its parent module via `ParentModule` property
4. Adds to `_submodules` dictionary grouped by parent module type
5. For Node-based submodules:
   - Adds as child to parent module if parent is also a Node
   - Otherwise, adds as child to ModuleManager

#### [`InitializeModules()`](ModuleManager.cs:169)
Initializes all registered modules and submodules in proper order.

**Process:**
1. **Phase 1**: Calls `Initialize()` on all main modules first
2. **Phase 2**: Calls `Initialize()` on all submodules after main modules
3. Each initialization is wrapped in try-catch for error handling
4. Errors are logged but don't stop the initialization of other components

## Lifecycle

1. **Discovery Phase**: Scans assemblies for decorated types
2. **Registration Phase**: Creates instances of modules with `AutoLoad = true`
3. **Initialization Phase**: Calls `Initialize()` on all modules, then submodules
4. **Runtime Phase**: Modules operate independently
5. **Cleanup Phase**: Calls `Cleanup()` on all components in reverse order

## Usage

The ModuleManager is automatically instantiated when added to the scene tree. Access modules through the singleton:

```csharp
var audioModule = ModuleManager.Instance.GetModule<AudioModule>();
var debugSubmodules = ModuleManager.Instance.GetSubmodules<DebugModule>();
```

## Important Notes

- Only one ModuleManager instance can exist (additional instances self-destruct)
- Modules must implement [`IModule`](../Interfaces/IModule.cs) interface
- Submodules must implement [`ISubmodule`](../Interfaces/ISubmodule.cs) interface
- Node-based modules are automatically added to the scene tree
- All operations include comprehensive error handling and logging