namespace SnippetManagement.Service.Model;

public class ProgramingLanguageDto
{
    public ProgramingLanguageDto(string name, string url)
    {
        Name = name;
        Url = url;
    }

    public string Name { get; set; }
    public string Url { get; set; }
}