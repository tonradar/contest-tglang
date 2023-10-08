using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace TgLang.CodeCrawler.Models
{
    public class LanguageDef
    {
        public string Name { get; set; }
        public string? Extension { get; set; }
        public string? GitHubTag { get; set; }
        public Language? GitHubLanguage { get; set; }
        public int? TelegramCode { get; set; }

        public LanguageDef(string name, string extension, string? gitHubTag = null, Language? gitHubLanguage = null, int? telegramCode = null)
        {
            Name = name;
            Extension = extension;
            GitHubTag = gitHubTag;
            GitHubLanguage = gitHubLanguage;
            TelegramCode = telegramCode;
        }


        public LanguageDef(string name, string? extension, Language? gitHubLanguage = null)
        {
            Name = name;
            Extension = extension;
            GitHubLanguage = gitHubLanguage;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
