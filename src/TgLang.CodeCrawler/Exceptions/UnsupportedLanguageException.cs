namespace TgLang.CodeCrawler.Exceptions;

public class UnsupportedLanguageException : Exception
{
    public UnsupportedLanguageException() : base("The language is not supported for search")
    {
        
    }
}