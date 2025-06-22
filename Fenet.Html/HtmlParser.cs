using System.Text;
using Fenet.Dom;
using Attribute = Fenet.Dom.Attribute;

namespace Fenet.Html;

public class HtmlParser
{
    private string? _source;
    private int _pos;
    
    public Node Parse(string source)
    {
        _source = source;
        var nodes = ParseNodes();

        if (nodes.Count == 1)
        {
            return nodes[0];
        }
        
        return Node.CreateElement("html", [], nodes);
    }

    private List<Node> ParseNodes()
    {
        var nodes = new List<Node>();
        
        while (true)
        {
            ConsumeWhitespace();

            while (StartsWith("<!"))
            {
                ConsumeComment();
                ConsumeWhitespace();
            }
            
            if (Eof() || StartsWith("</"))
            {
                break;
            }
            nodes.Add(ParseNode());
        }
        
        return nodes;
    }

    private Node ParseNode()
    {
        if (StartsWith("<"))
        {
            return ParseElement();
        }

        return ParseText();
    }

    private Node ParseElement()
    {
        // Opening tag
        Expect("<");
        var tagName = ParseName();
        var attributes = ParseAttributes();

        if (tagName is "meta" or "link" or "base")
        {
            Expect(">");
            return Node.CreateElement(tagName, attributes, []);
        }
        
        // Self closing element
        if (NextChar() == '/')
        {
            Expect("/>");
            return Node.CreateElement(tagName, attributes, []);
        }
        
        Expect(">");

        var children = ParseNodes();
        
        // Opening tag
        Expect("</");
        Expect(tagName);
        Expect(">");
        
        return Node.CreateElement(tagName, attributes, children);
    }

    private List<Attribute> ParseAttributes()
    {
        var attributes = new List<Attribute>();

        while (true)
        {
            ConsumeWhitespace();
            
            if (NextChar() == '>' || NextChar() == '/')
            {
                break;
            }

            var (name, value) = ParseAttribute();
            attributes.Add(new Attribute
            {
                Name = name,
                Value = value
            });
        }
        
        return attributes;
    }

    private (string, string?) ParseAttribute()
    {
        var name = ParseName();
        
        if (NextChar() != '=')
        {
            return (name, null);
        }

        ConsumeChar();
        
        var openingQuote = ConsumeChar();
        var value = ConsumeWhile(ch => ch != openingQuote);
        
        ConsumeChar();

        return (name, value);
    }

    private Node ParseText()
    {
        return Node.CreateText(ConsumeWhile(ch => ch != '<'));
    }

    private string ParseName()
    {
        return ConsumeWhile(char.IsAsciiLetterOrDigit);
    }

    private void Expect(string value)
    {
        if (StartsWith(value))
        {
            _pos += value.Length;
        }
        else
        {
            throw new Exception($"Expected {value} at pos {_pos} but it was not found");
        }
    }

    private char NextChar()
    {
        if (_source == null)
        {
            throw new Exception("Tried to read next char but source was null");
        }
        
        return _source[_pos];
    }

    private char ConsumeChar()
    {
        var ch = NextChar();
        _pos++;
        return ch;
    }
    
    private bool Eof()
    {
        return _source != null && _pos >= _source.Length;
    }

    private bool StartsWith(string value)
    {
        return _source != null && _source[_pos..].StartsWith(value);
    }

    private void ConsumeComment()
    {
        ConsumeWhile(ch => ch != '>');
        ConsumeChar();
    }

    private void ConsumeWhitespace()
    {
        ConsumeWhile(char.IsWhiteSpace);
    }

    private string ConsumeWhile(Func<char, bool> test)
    {
        var result = new StringBuilder();
        
        while (!Eof() && test(NextChar()))
        {
            result.Append(ConsumeChar());
        }
        
        return result.ToString();
    }

    public static void PrettyPrint(Node node, int indentLevel = 0)
    {
        var indentBuilder = new StringBuilder();
        for (var i = 0; i < indentLevel; i++)
        {
            indentBuilder.Append("   ");
        }
        var indent = indentBuilder.ToString();
        
        if (node.Type == NodeType.Text)
        {
            Console.WriteLine($"{indent}Text Node:");
            Console.WriteLine($"{indent}- value: {node.Value}");
        }
        else
        {
            Console.WriteLine($"{indent}Element Node:");
            Console.WriteLine($"{indent}- tag name: {node.TagName}");
            if (node.Attributes.Count > 0)
            {
                Console.WriteLine($"{indent}- attributes:");
                foreach (var attrib in node.Attributes)
                {
                    Console.WriteLine($"{indent}  - {attrib.Name}: {attrib.Value ?? ""}");
                }
            }
        }

        foreach (var child in node.Children)
        {
            PrettyPrint(child, indentLevel + 1);
        }
    }
}