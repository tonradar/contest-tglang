using TgLang.CodeCrawler.Models;

namespace TgLang.CodeCrawler.Services.Contracts;

public interface ILanguageDefService
{
    List<LanguageDef> GetLanguageDefs();
}