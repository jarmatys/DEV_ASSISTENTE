namespace ASSISTENTE.Domain.Entities.Resources;

// TODO: configure the class to be a value object
// TODO: setup metadata properties for the Resource entity
// INFO: Metadata are important to provide information about the resource for better understanding by LLM 

public class Metadata
{
    public string Title { get; set; }   
    public string Source { get; set; }   
    public int Index { get; set; }   
    public int Tokens { get; set; }   
    public List<Headers> Headers { get; set; }  
    public List<string> Urls { get; set; }
    public List<string> ImageUrls { get; set; }
    public List<string> Tags { get; set; }
}

public class Headers
{
    public string Type { get; set; }   // e.g. "h1", "h2", "h3", "h4", "h5", "h6"
    public string Text { get; set; }
}