using System;

[AttributeUsage(AttributeTargets.Class)]
public class SubmoduleAttribute : Attribute
{
    public Type ParentModuleType { get; }
    public bool AutoLoad { get; }
    public int LoadOrder { get; }

    public SubmoduleAttribute(Type parentModuleType, bool autoLoad = true, int loadOrder = 0)
    {
        ParentModuleType = parentModuleType;
        AutoLoad = autoLoad;
        LoadOrder = loadOrder;
    }
}
