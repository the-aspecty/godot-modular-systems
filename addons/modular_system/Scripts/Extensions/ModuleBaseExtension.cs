using Godot;

namespace ModularSystem
{
    public static class ModuleBaseExtension
    {
        public static void RegisterModule(this ModuleBase module)
        {
            if (module == null)
                return;

            module.RegisterModule();
        }

        public static void CleanupModule(this ModuleBase module)
        {
            if (module == null)
                return;

            module.Cleanup();
        }
    }
}
