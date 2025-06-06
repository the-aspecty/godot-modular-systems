using System;
using System.Reflection;
using Godot;

public class NodeReferenceProcessor
{
    private static NodeReferenceProcessor _instance;
    public static NodeReferenceProcessor Instance => _instance ??= new NodeReferenceProcessor();

    public void ProcessNodeReferences(Node node)
    {
        var type = node.GetType();
        var fields = type.GetFields(
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
        );

        foreach (var field in fields)
        {
            var attribute = field.GetCustomAttribute<NodeReferenceAttribute>();
            if (attribute == null)
                continue;

            if (!typeof(Node).IsAssignableFrom(field.FieldType))
            {
                GD.PrintErr($"Field {field.Name} must be of type Node or derived from Node");
                continue;
            }

            try
            {
                var referencedNode = node.GetNode(attribute.Path);
                if (referencedNode != null)
                {
                    if (field.FieldType.IsAssignableFrom(referencedNode.GetType()))
                    {
                        field.SetValue(node, referencedNode);
                        GD.Print($"Linked node at {attribute.Path} to {field.Name}");
                    }
                    else
                    {
                        GD.PrintErr(
                            $"Node at {attribute.Path} is not of type {field.FieldType.Name}"
                        );
                    }
                }
                else if (attribute.Required)
                {
                    GD.PrintErr($"Required node not found at path: {attribute.Path}");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Error linking node for {field.Name}: {ex.Message}");
            }
        }
    }
}
