using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgLang.CodeCrawler.Models
{
    public class GitHubFile
    {
        public long RepoId { get; set; }
        public string Sha { get; set; }
        public string Url { get; set; }
        public string Path { get; set; }
        public int Size { get; set; }
    }
}
