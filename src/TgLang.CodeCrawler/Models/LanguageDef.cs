using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgLang.CodeCrawler.Models
{
    public class LanguageDef
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public string? GitHubTag { get; set; }
        public int? TelegramCode { get; set; }

        public LanguageDef(string name, string extension, string? gitHubTag = null, int? telegramCode = null)
        {
            Name = name;
            Extension = extension;
            GitHubTag = gitHubTag;
            TelegramCode = telegramCode;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
