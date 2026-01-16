using Markdig;

if (args.Length < 3)
{
    Console.WriteLine("Usage: MarkdownConverter <source.md> <output.html> <collapsible-output.html> [split-marker]");
    return 1;
}

var sourceFile = args[0];
var outputFile = args[1];
var collapsibleOutputFile = args[2];
var splitMarker = args.Length > 3 ? args[3] : "## Public Holiday Substitution Rules";

var markdown = File.ReadAllText(sourceFile);
var splitIndex = markdown.IndexOf(splitMarker);

if (splitIndex > 0)
{
    var alwaysVisibleMd = markdown[..splitIndex];
    var collapsibleMd = markdown[(splitIndex + splitMarker.Length)..];

    File.WriteAllText(outputFile, Markdown.ToHtml(alwaysVisibleMd));
    File.WriteAllText(collapsibleOutputFile, Markdown.ToHtml(collapsibleMd));
}
else
{
    File.WriteAllText(outputFile, Markdown.ToHtml(markdown));
    File.WriteAllText(collapsibleOutputFile, "");
}

Console.WriteLine($"Converted {sourceFile} to {outputFile} and {collapsibleOutputFile}");
return 0;
