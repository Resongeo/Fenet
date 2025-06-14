using Fenet.Dom;
using Fenet.Html;

namespace Fenet.Core;

public class BrowserEngine
{
    private readonly HtmlParser _htmlParser = new();

    public Node TestParse(string source)
    {
        return _htmlParser.Parse(source);
    }
}