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
        public bool IsActive = true;


        public LanguageDef(int telegramCode, string name, string? extension, Language? gitHubLanguage = null, bool isActive = true)
        {
            TelegramCode = telegramCode;
            Name = name;
            Extension = extension;
            GitHubLanguage = gitHubLanguage;
            IsActive = isActive;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
