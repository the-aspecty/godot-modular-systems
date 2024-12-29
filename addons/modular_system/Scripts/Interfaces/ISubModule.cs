public interface ISubmodule : IModule
{
    IModule ParentModule { get; set; }
}
