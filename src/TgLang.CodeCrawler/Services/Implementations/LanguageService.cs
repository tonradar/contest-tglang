using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TgLang.CodeCrawler.Models;
using TgLang.CodeCrawler.Services.Contracts;

namespace TgLang.CodeCrawler.Services.Implementations
{
    public class LanguageDefService : ILanguageDefService
    {
        public List<LanguageDef> GetLanguageDefs()
        {
            return LanguageDefs;
        }

        private List<LanguageDef> LanguageDefs { get; set; } = new List<LanguageDef>()
        {
            new("C#", "cs", "csharp"),
            new("JavaScript", "js"),
            new("TypeScript", "ts"),
            new("HTML", "html"),
        };
    }
}
