using Godot;

public partial class SceneAutoProcessor : Node
{
    public override void _EnterTree()
    {
        GetTree().NodeAdded += OnNodeAdded;
    }

    public override void _ExitTree()
    {
        GetTree().NodeAdded -= OnNodeAdded;
    }

    private void OnNodeAdded(Node node)
    {
        SceneReferenceProcessor.Instance.ProcessSceneReferences(node);
        NodeReferenceProcessor.Instance.ProcessNodeReferences(node);
        CooldownProcessor.Instance.ProcessCooldowns(node);
        //InputActionProcessor.Instance.ProcessInputActions(node);
    }
}
