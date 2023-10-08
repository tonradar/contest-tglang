using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;
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
            new("C#", "cs", "csharp", Language.CSharp),
            new("JavaScript", "js", "javascript", Language.JavaScript),
            new("TypeScript", "ts", "typescript", Language.TypeScript),
            new("XML", "xml", "xml", Language.Xml),
            new("CSS", "css", "css", Language.Css),
            new("Markdown", "md", "markdown", Language.Markdown),
            new("YAML", "yml", "yaml", Language.Yaml),
        };
    }
}
