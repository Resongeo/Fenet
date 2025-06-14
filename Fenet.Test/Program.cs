using Fenet.Core;
using Fenet.Html;

const string testInput = @"
<html>
    <div>
        <h1>Hellope</h1>
        <p>Lorem ipsum</p>
        <ul>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
        </ul>
    </div>
</html>
";

var browser = new BrowserEngine();
var dom = browser.TestParse(testInput);
HtmlParser.PrettyPrint(dom);