using Fenet.Html;

const string testInput =
    """
    <!DOCTYPE html>
    <html>
        <div class='flex flex-row' id="container">
        <!--This is a comment,} the parser ignores it-->
            <!-This is an incorrect comment, the parser should ignore it-->
            <h1>Hellope</h1>
            <p>Lorem ipsum</p>
            <button disabled>Disabled</button>
        </div>
        <input type="text" />
    </html>
    """;

var dom = new HtmlParser().Parse(testInput);
HtmlParser.PrettyPrint(dom);