using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TgLang.CodeCrawler.Models;
using TgLang.CodeCrawler.Services.Contracts;

namespace TgLang.CodeCrawler.Test
{
    public class LanguageDefServiceTests
    {
        [Fact]
        public async Task GetLanguageOfUrl_MustWork()
        {
            var testHost = Host.CreateDefaultBuilder()
                               .ConfigureServices((_, services) =>
                               {
                                   services.AddSharedServices();
                               }
                               ).Build();

            var languageDefService = testHost.Services.GetRequiredService<ILanguageDefService>();

            var (language, extension) = languageDefService.GetLanguageOfUrl("https://github.com/tonradar/contest-tglang/blob/main/src/TgLang.CodeCrawler/Services/Contracts/IGitHubService.cs");

            Assert.Equal("cs", language?.Extension);
            Assert.Equal("cs", extension);
        }
    }
}