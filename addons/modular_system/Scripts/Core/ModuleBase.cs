using Godot;

public abstract partial class ModuleBase : Node, IComponent
{
    public virtual void Initialize() { }

    public virtual void Cleanup() { }
}
