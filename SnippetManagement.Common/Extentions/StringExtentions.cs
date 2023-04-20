namespace SnippetManagement.Common.Extentions;

public static class StringExtentions
{
    public static string? Capitalize(this string? word)
    {
        if (string.IsNullOrWhiteSpace(word))
            return word;
        return word.Substring(0, 1).ToUpper() + word.Substring(1);
    }
}