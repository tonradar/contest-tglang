// See https://aka.ms/new-console-template for more information

using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TgLang.CodeCrawler.Services.Contracts;


var codeFolder = @"C:\TgCode";


Console.WriteLine("TgCodeCrawler started.");

var host = Host.CreateDefaultBuilder()
                   .ConfigureServices((_, services) =>
                       {
                           services.AddSharedServices();
                       }
                   ).Build();

var gitHubService = host.Services.GetRequiredService<IGitHubService>();
var languageDefService = host.Services.GetRequiredService<ILanguageDefService>();

var languages = languageDefService.GetLanguageDefs();

var validLanguages =
    from language in languages
    where language.GitHubTag is not null
    select language;

foreach (var language in validLanguages)
{
    

    var repos = await gitHubService.GetLanguageRepoUrlsAsync(language);

    foreach (var repo in repos.Take(3))
    {
        var files = await gitHubService.GetCodeFilesAsync(repo);

        foreach (var file in files)
        {
            var fileLanguage = languageDefService.GetLanguageOfUrl(file.Url);

            if (fileLanguage is null)
                continue;

            var languageFolder = Path.Combine(codeFolder, fileLanguage.Extension);
            if (!Path.Exists(languageFolder))
            {
                Directory.CreateDirectory(languageFolder);
            }

            var content = await gitHubService.GetCodeFileContentAsync(file.RepoId, file.Sha);
            var filePath = Path.Combine(languageFolder, file.Url.Replace("/", "_"));
            await File.WriteAllTextAsync(filePath, content);
        }
    }
}




