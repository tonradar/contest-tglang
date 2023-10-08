// See https://aka.ms/new-console-template for more information

using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TgLang.CodeCrawler.Services.Contracts;


var codeFolder = @"C:\TgCode";
var requiredSamples = 5;

Console.WriteLine("TgCodeCrawler started.");

var host = Host.CreateDefaultBuilder()
                   .ConfigureServices((_, services) =>
                       {
                           services.AddSharedServices();
                       }
                   ).Build();

var codeCrawler = host.Services.GetRequiredService<ICodeCrawlerService>();

//await codeCrawler.CrawlUsingReposAsync(codeFolder);

await codeCrawler.CrawlUsingSearchAsync(codeFolder, requiredSamples);








