using Godot;

public partial class NodeReferenceExample : Control
{
    [NodeReference("Label")]
    private Label _label;

    public override void _Ready()
    {
        _label.Text = "Node Reference Example Ready!";
    }
}
