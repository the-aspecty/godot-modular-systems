using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;

public partial class ModuleManager : Node
{
    private static ModuleManager _instance;
    public static ModuleManager Instance => _instance;

    private readonly Dictionary<Type, IModule> _modules = new();
    private readonly Dictionary<Type, List<ISubmodule>> _submodules = new();
    private readonly List<Assembly> _scannedAssemblies = new();

    public override void _EnterTree()
    {
        if (_instance == null)
        {
            _instance = this;
            ScanForModules();
        }
        else
        {
            QueueFree();
        }
    }

    public T GetModule<T>()
        where T : class, IModule
    {
        return _modules.TryGetValue(typeof(T), out var module) ? (T)module : null;
    }

    public IReadOnlyList<ISubmodule> GetSubmodules<T>()
        where T : class, IModule
    {
        return _submodules.TryGetValue(typeof(T), out var submodules)
            ? submodules.AsReadOnly()
            : new List<ISubmodule>().AsReadOnly();
    }

    private void ScanForModules()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            if (_scannedAssemblies.Contains(assembly))
                continue;

            try
            {
                ScanAssemblyForModules(assembly);
                _scannedAssemblies.Add(assembly);
            }
            catch (Exception e)
            {
                GD.PrintErr($"Error scanning assembly {assembly.FullName}: {e.Message}");
            }
        }

        InitializeModules();
    }

    private void ScanAssemblyForModules(Assembly assembly)
    {
        var moduleTypes = assembly
            .GetTypes()
            .Where(t => t.GetCustomAttribute<ModuleAttribute>() != null)
            .OrderBy(t => t.GetCustomAttribute<ModuleAttribute>().LoadOrder);

        foreach (var moduleType in moduleTypes)
        {
            RegisterModule(moduleType);
        }

        var submoduleTypes = assembly
            .GetTypes()
            .Where(t => t.GetCustomAttribute<SubmoduleAttribute>() != null)
            .OrderBy(t => t.GetCustomAttribute<SubmoduleAttribute>().LoadOrder);

        foreach (var submoduleType in submoduleTypes)
        {
            RegisterSubmodule(submoduleType);
        }
    }

    private void RegisterModule(Type moduleType)
    {
        var attr = moduleType.GetCustomAttribute<ModuleAttribute>();
        if (attr?.AutoLoad != true)
            return;

        try
        {
            var module = Activator.CreateInstance(moduleType) as IModule;
            if (module == null)
                return;

            if (string.IsNullOrEmpty(module.Name) && !string.IsNullOrEmpty(attr.Name))
            {
                var nameProperty = moduleType.GetProperty("Name");
                nameProperty?.SetValue(module, attr.Name);
            }

            _modules[moduleType] = module;

            if (module is Node nodeModule)
            {
                ServiceLocator.RegisterServiceInferType(module);
                nodeModule.Name = module.Name;
                AddChild(nodeModule, true);
            }
        }
        catch (Exception e)
        {
            GD.PrintErr($"Error creating module {moduleType.Name}: {e.Message}");
        }
    }

    private void RegisterSubmodule(Type submoduleType)
    {
        var attr = submoduleType.GetCustomAttribute<SubmoduleAttribute>();
        if (attr?.AutoLoad != true)
            return;

        try
        {
            var submodule = Activator.CreateInstance(submoduleType) as ISubmodule;
            if (submodule == null)
                return;

            if (_modules.TryGetValue(attr.ParentModuleType, out var parentModule))
            {
                submodule.ParentModule = parentModule;

                if (!_submodules.ContainsKey(attr.ParentModuleType))
                {
                    _submodules[attr.ParentModuleType] = new List<ISubmodule>();
                }

                _submodules[attr.ParentModuleType].Add(submodule);

                if (submodule is Node nodeSubmodule)
                {
                    if (parentModule is Node nodeParent)
                    {
                        nodeParent.AddChild(nodeSubmodule);
                    }
                    else
                    {
                        AddChild(nodeSubmodule);
                    }
                }
            }
        }
        catch (Exception e)
        {
            GD.PrintErr($"Error creating submodule {submoduleType.Name}: {e.Message}");
        }
    }

    private void InitializeModules()
    {
        // Initialize main modules first
        foreach (var module in _modules.Values)
        {
            try
            {
                module.Initialize();
            }
            catch (Exception e)
            {
                GD.PrintErr($"Error initializing module {module.Name}: {e.Message}");
            }
        }

        // Then initialize submodules
        foreach (var submoduleList in _submodules.Values)
        {
            foreach (var submodule in submoduleList)
            {
                try
                {
                    submodule.Initialize();
                }
                catch (Exception e)
                {
                    GD.PrintErr($"Error initializing submodule {submodule.Name}: {e.Message}");
                }
            }
        }
    }

    public override void _ExitTree()
    {
        // Cleanup in reverse order: submodules first, then modules
        foreach (var submoduleList in _submodules.Values)
        {
            foreach (var submodule in submoduleList)
            {
                try
                {
                    submodule.Cleanup();
                }
                catch (Exception e)
                {
                    GD.PrintErr($"Error cleaning up submodule {submodule.Name}: {e.Message}");
                }
            }
        }

        foreach (var module in _modules.Values)
        {
            try
            {
                module.Cleanup();
            }
            catch (Exception e)
            {
                GD.PrintErr($"Error cleaning up module {module.Name}: {e.Message}");
            }
        }

        if (_instance == this)
        {
            _instance = null;
        }
    }
}
