using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TgLang.CodeCrawler.Services.Contracts;


var codeFolder = args.ElementAtOrDefault(1) ?? @"C:\TgCode";
var requiredSamples = int.Parse(args.ElementAtOrDefault(2) ?? "1000");

Console.WriteLine("TgCodeCrawler started.");

var host = Host.CreateDefaultBuilder()
                   .ConfigureServices((_, services) =>
                       {
                           services.AddSharedServices();
                       }
                   ).Build();

var codeCrawler = host.Services.GetRequiredService<ICodeCrawlerService>();
await codeCrawler.CrawlAsync(codeFolder, requiredSamples);








