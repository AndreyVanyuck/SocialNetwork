using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SocialNetwork.Domain.Core;
using Azure.Identity;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace SocialNetwork
{
    public class Program
    {
      
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            //host.Run();
           

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var configs = services.GetRequiredService<IConfiguration>();
                    await RoleInitializer.InitializeAsync(userManager, rolesManager, configs);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
          /*  .ConfigureAppConfiguration((context, config) =>
            {
                //var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
                var client = new Uri("https://socialnetworkvault.vault.azure.net/");
                config.AddAzureKeyVault(
                client,
                new DefaultAzureCredential());
            })*/
          .ConfigureAppConfiguration((ctx, builder) =>
          {
              var keyVaultEndpoint = GetKeyVaultEndpoint();
              if (!string.IsNullOrEmpty(keyVaultEndpoint))
              {
                  var azureServiceTokenProvider = new AzureServiceTokenProvider();
                  var keyVaultClient = new KeyVaultClient(
                      new KeyVaultClient.AuthenticationCallback(
                          azureServiceTokenProvider.KeyVaultTokenCallback));
                  builder.AddAzureKeyVault(
                  keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
              }
          })
          
              .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                    logging.AddDebug();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        private static string GetKeyVaultEndpoint() => Environment.GetEnvironmentVariable("VaultUri");
    }
}
