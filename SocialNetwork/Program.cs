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
using Serilog;
using Serilog.Events;

namespace SocialNetwork
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var configs = services.GetRequiredService<IConfiguration>();
                    await RoleInitializer.InitializeAsync(userManager, rolesManager, configs);

                    Log.Logger = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .WriteTo.File(configs["all-logs"])
                        .WriteTo.Logger(lc => lc
                        .Filter.ByIncludingOnly(le => le.Level == LogEventLevel.Error)
                        .WriteTo.File(configs["error-logs"]))
                        .CreateLogger();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

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
              .ConfigureWebHostDefaults(webBuilder =>
              {
                  webBuilder.UseStartup<Startup>();
              });
        private static string GetKeyVaultEndpoint() => Environment.GetEnvironmentVariable("VaultUri");
    }
}
