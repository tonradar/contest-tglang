using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TgLang.CodeCrawler.Exceptions;
using TgLang.CodeCrawler.Models;
using TgLang.CodeCrawler.Services.Contracts;

namespace TgLang.CodeCrawler.Services.Implementations
{
    public class CodeCrawlerService : ICodeCrawlerService
    {
        public IGitHubService GitHubService { get; }
        public ILanguageDefService LanguageDefService { get; }

        public CodeCrawlerService(IGitHubService gitHubService, ILanguageDefService languageDefService)
        {
            GitHubService = gitHubService;
            LanguageDefService = languageDefService;
        }

        public async Task CrawlUsingReposAsync(string codeFolder, int sampleCount)
        {
            var languages = LanguageDefService.GetLanguageDefs();

            var validLanguages =
                from language in languages
                where language.GitHubTag is not null
                select language;

            foreach (var language in validLanguages)
            {


                var repos = await GitHubService.GetLanguageRepoUrlsAsync(language);

                foreach (var repo in repos.Take(3))
                {
                    var files = await GitHubService.GetFilesByUrlAsync(repo);

                    foreach (var file in files)
                    {
                        var (fileLanguage, extension) = LanguageDefService.GetLanguageOfUrl(file.Path);

                        if (fileLanguage is null)
                            continue;

                        var languageFolder = Path.Combine(codeFolder, language.Name);
                        var filePath = Path.Combine(languageFolder, file.RepoId + "_" + file.Sha + "." + extension);

                        if (File.Exists(filePath))
                            continue;

                        if (!Path.Exists(languageFolder))
                        {
                            Directory.CreateDirectory(languageFolder);
                        }

                        var content = await GitHubService.GetCodeFileContentAsync(file.RepoId, file.Sha);
                        await File.WriteAllTextAsync(filePath, content);
                    }
                }
            }
        }

        public async Task CrawlUsingSearchAsync(string codeFolder, int sampleCount)
        {
            var pageSize = 100;
            
            var languages = LanguageDefService.GetLanguageDefs();

            var validLanguages =
                from language in languages
                where language.IsActive
                select language;

            foreach (var language in validLanguages)
            {
                var currentSamples = GetCurrentSamplesCount(codeFolder, language);
                var neededSamples = sampleCount - currentSamples;

                var loadedSamples = 0;
                
                if (currentSamples >= sampleCount)
                    continue;

                Console.WriteLine($"Getting {language.Name}: [Current:{currentSamples}] [Needed:{neededSamples}]");
                var pageCount = currentSamples / pageSize;
                var isTriedWithTags = false;
                try
                {
                    while (true)
                    {
                        if (loadedSamples >= neededSamples)
                            break;

                        if (isTriedWithTags)
                            break;

                        var files = await GitHubService.GetFilesBySearchAsync(language, pageCount++, pageSize);

                        if (files.Count == 0)
                        {
                            Console.WriteLine($"[{language.Name}]: Not enough files by search, trying TAGS!!!");
                            files = await GitHubService.GetFilesByTagAsync(language, neededSamples);
                            isTriedWithTags = true;
                            if (files.Count == 0)
                            {
                                Console.WriteLine($"[{language.Name}]: CAN NOT PROVIDE BY SEARCH!!!");
                                break;
                            }

                            Console.WriteLine($"[{language.Name}]: Provided with tags ({files.Count} files)");
                        }

                        foreach (var file in files)
                        {
                            if (loadedSamples >= neededSamples)
                                break;

                            var (fileLanguage, extension) = LanguageDefService.GetLanguageOfUrl(file.Path);

                            if (extension is null)
                                continue;

                            var languageFolder = Path.Combine(codeFolder, language.Name);
                            var filePath = Path.Combine(languageFolder,
                                file.RepoId + "_" + file.Sha + "." + extension);

                            if (File.Exists(filePath))
                                continue;

                            if (!Path.Exists(languageFolder))
                            {
                                Directory.CreateDirectory(languageFolder);
                            }

                            var content = await GitHubService.GetCodeFileContentAsync(file.RepoId, file.Sha);
                            await File.WriteAllTextAsync(filePath, content);
                            loadedSamples++;
                            //await Task.Delay(TimeSpan.FromSeconds(1));
                        }
                    }
                    Console.WriteLine($"[{language.Name}]: DONE   [loaded:{loadedSamples}]     [all:{loadedSamples+currentSamples}]");
                }
                catch (UnsupportedLanguageException)
                {
                    Console.WriteLine($"[{language.Name}]: UNSUPPORTED!!!");
                }
            }
        }

        public int GetCurrentSamplesCount(string codeFolder, LanguageDef language)
        {
            var langFolder = Path.Combine(codeFolder, language.Name);
            if (!Directory.Exists(langFolder))
                return 0;

            var files = Directory.GetFiles(langFolder, $"*.{language.Extension}", searchOption: SearchOption.TopDirectoryOnly);
            return files.Length;

        }
    }
}
