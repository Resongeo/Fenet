using Fenet.Core;
using Fenet.Html;

const string testInput =
"""
 <!DOCTYPE html>
 <html>
     <div class='flex flex-row' id="container">
     <!--This is a comment,} the parser ignores it-->
         <!-This is an incorrect comment, the parser should ignore it-->
         <h1>Hellope</h1>
         <p disabled>Lorem ipsum</p>
     </div>
 </html>
 """;

var browser = new BrowserEngine();
var dom = browser.TestParse(testInput);
HtmlParser.PrettyPrint(dom);