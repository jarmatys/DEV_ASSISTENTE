using ASSISTENTE.Common.Results;
using ASSISTENTE.Infrastructure.MarkDownParser.Models;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.MarkDownParser.ValueObjects;

public sealed class FileContent
{
    public string Title { get; set; }
    public IEnumerable<string> TextBlocks { get; }

    private FileContent(string title, IEnumerable<string> textBlocks)
    {
        Title = title;
        TextBlocks = textBlocks;
    }

    public static Result<FileContent> Create(string title, List<ElementBase> elements)
    {
        if (elements.Count == 0)
            return Result.Failure<FileContent>(FileContentErrors.EmptyContent.Build());

        var blocks = new List<string>();
        
        var containsHeadings = elements.Any(x => x is Heading);
        if (containsHeadings)
        {
            var headings = elements.OfType<Heading>().ToList();
            var previousHeadingLocation = 0;
            foreach (var heading in headings)
            {
                // TODO: remove blocks with only headings
                var headingLocation = elements.IndexOf(heading);
                
                var take = previousHeadingLocation == 0 
                    ? headingLocation 
                    : headingLocation - previousHeadingLocation;

                var blockElements = elements
                    .Skip(previousHeadingLocation)
                    .Take(take);

                blocks.Add(CreateBlock(title, blockElements));
                
                previousHeadingLocation = headingLocation;
            }

            var lastBlockElements = elements.Skip(previousHeadingLocation);
            
            blocks.Add(CreateBlock(title, lastBlockElements));
        }
        else
        {
            blocks.Add(CreateBlock(title, elements));
        }

        return new FileContent(title, blocks);
    }

    private static string CreateBlock(string fileName, IEnumerable<ElementBase> blocks)
    {
        var content = string.Join("\n", blocks.Select(x => x.GetContent()));
        
        return $"File name (title): '{fileName}'\n\n{content}";
    }
       
}

public static class FileContentErrors
{
    public static readonly Error EmptyContent = new(
        "FileContent.EmptyContent", "File content is empty");
}