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

        public LanguageDef? GetLanguageOfUrl(string url)
        {
            var parts = url.Split('/');
            if (parts.Any() && parts.Last().Contains("."))
            {
                var fileParts = parts.Last().Split('.');
                var extension = fileParts.Last();

                return LanguageDefs.FirstOrDefault(l => l.Extension == extension);
            }

            return null;
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
