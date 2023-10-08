using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;
using TgLang.CodeCrawler.Services.Contracts;
using TgLang.CodeCrawler.Services.Implementations;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static void AddSharedServices(this IServiceCollection services)
    {
        services.AddTransient<IGitHubService, GitHubService>();
        services.AddTransient<ILanguageDefService, LanguageDefService>();
        //services.AddAppHook<ServerBadgeSystemAppHook>();
        //services.AddTransient<ILearnerService, ServerLearnerService>();
        // ToDo: Complete.
        services.AddHttpClient();
        services.AddTransient(CreateGitHubClient);
    }

    private static GitHubClient CreateGitHubClient(IServiceProvider serviceProvider)
    {
        var productHeaderValue = new ProductHeaderValue("CS-System");
        var gitHubToken = "ghp_" + serviceProvider.GetRequiredService<IConfiguration>().GetSection("GitHub")["GitHubAccessToken"];
        var tokenAuth = new Credentials(gitHubToken);
        var client = new GitHubClient(productHeaderValue)
        {
            Credentials = tokenAuth
        };
        return client;
    }
}
