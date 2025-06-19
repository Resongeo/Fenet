namespace Fenet.Dom;

public enum NodeType
{
    Text,
    Element,
}

public struct Attribute
{
    public string Name;
    public string? Value;
}

// TODO: may be possible to convert to a ref struct
public struct Node
{
    public string Value = string.Empty;
    public string TagName = string.Empty;
    public List<Attribute> Attributes = [];
    public List<Node> Children = [];
    public NodeType Type  = NodeType.Text;

    public Node() {}
    
    public static Node CreateElement(string tagName, List<Attribute> attributes, List<Node> children)
    {
        return new Node
        {
            TagName    = tagName,
            Attributes = attributes,
            Type       = NodeType.Element,
            Children   = children
        };
    }

    public static Node CreateText(string value)
    {
        return new Node
        {
            Value = value,
            Type  = NodeType.Text
        };
    }
}