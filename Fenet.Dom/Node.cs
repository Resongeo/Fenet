namespace Fenet.Dom;

public enum NodeType
{
    Text,
    Element,
}

// TODO: may be possible to convert to a ref struct
public struct Node
{
    public string Value = string.Empty;
    public string TagName = string.Empty;
    public NodeType Type  = NodeType.Text;
    public List<Node> Children = [];

    public Node() {}
    
    public static Node CreateElement(string tagName, List<Node> children)
    {
        return new Node
        {
            TagName  = tagName,
            Type     = NodeType.Element,
            Children = children
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