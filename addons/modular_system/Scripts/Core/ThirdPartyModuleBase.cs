using Godot;

public abstract partial class ThirdPartyModuleBase : Node, IThirdPartyModule
{
    public string Name { get; protected set; }
    public string LibraryName { get; protected set; }
    public string Version { get; protected set; }
    public bool IsInitialized { get; protected set; }
    public bool IsCompatible { get; protected set; }

    public virtual void Initialize()
    {
        ValidateCompatibility();
        if (!IsCompatible)
        {
            GD.PrintErr($"Third party module {LibraryName} {Version} is not compatible!");
            return;
        }
        OnInitialize();
        IsInitialized = true;
    }

    public virtual void Cleanup()
    {
        OnCleanup();
        IsInitialized = false;
    }

    public virtual void ValidateCompatibility()
    {
        IsCompatible = true; // Override this for specific compatibility checks
    }

    protected virtual void OnInitialize() { }

    protected virtual void OnCleanup() { }
}
