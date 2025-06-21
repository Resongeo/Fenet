using Fenet.Dom;
using Fenet.Html;
using Fenet.Net;

namespace Fenet.Core;

public class BrowserEngine
{
    private readonly HtmlParser _htmlParser = new();
    private readonly ResourceLoader _resourceLoader = new();

    public async Task LoadUrlAsync(string url)
    {
        try
        {
            var htmlSource = await _resourceLoader.LoadStringAsync(url);
            var nodeTree = _htmlParser.Parse(htmlSource);
            HtmlParser.PrettyPrint(nodeTree);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}