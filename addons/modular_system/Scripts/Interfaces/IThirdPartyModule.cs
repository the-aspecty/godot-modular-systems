public interface IThirdPartyModule : IModule
{
    string LibraryName { get; }
    string Version { get; }
    bool IsCompatible { get; }
    void ValidateCompatibility();
}
