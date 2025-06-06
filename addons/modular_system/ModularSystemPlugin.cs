#if TOOLS
using Godot;
using System;

[Tool]
public partial class ModularSystemPlugin : EditorPlugin
{
    public override void _EnterTree()
    {
        // Register autoload singleton
        AddAutoloadSingleton(
            "ModuleManager",
            "res://addons/modular_system/Scripts/Core/ModuleManager.cs"
        );
        AddAutoloadSingleton(
            "SceneAutoProcessor",
            "res://addons/modular_system/Scripts/Processors/SceneAutoProcessor.cs"
        );
    }

    public override void _ExitTree()
    {
        // Remove autoload singleton
        RemoveAutoloadSingleton("ModuleManager");
        RemoveAutoloadSingleton("SceneAutoProcessor");
    }
}
#endif
