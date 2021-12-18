using Domain;
using Infrastructure.DBContext;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.Clients;
using Infrastructure.Services.KeyVault;
using Infrastructure.Services.Transactions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Functions;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectIkwambe.ErrorHandlerMiddleware;
using ProjectIkwambe.Security;
using ProjectIkwambe.Utils;
using System;

namespace ProjectIkwambe.Startup {
	public class Program {

		public static void Main() {
			IHost host = new HostBuilder()
				.ConfigureFunctionsWorkerDefaults((IFunctionsWorkerApplicationBuilder Builder) => {
					Builder.UseNewtonsoftJson().UseMiddleware<JwtMiddleware>();
					Builder.UseMiddleware<GlobalErrorHandler>();
				})
				.ConfigureOpenApi()
				.ConfigureServices(Configure)
				.Build();

			host.Run();
		}

		static void Configure(HostBuilderContext Builder, IServiceCollection Services) {
			//jwt security
			Services.AddSingleton<ITokenService, TokenService>();

			// DBContext
			Services.AddDbContext<IkwambeContext>(option =>
            {
                option.UseCosmos(
					Environment.GetEnvironmentVariable("CosmosDb:Account", EnvironmentVariableTarget.Process),
					GetCosmosDbKey(),
					Environment.GetEnvironmentVariable("CosmosDb:DatabaseName", EnvironmentVariableTarget.Process)
				);
			});

			// Repositories
			Services.AddTransient(typeof(ICosmosReadRepository<>), typeof(CosmosReadRepository<>));
			Services.AddTransient(typeof(ICosmosWriteRepository<>), typeof(CosmosWriteRepository<>));

            // Services
            Services.AddScoped<IDonationService, DonationService>();
            Services.AddScoped<IUserService, UserService>();
            Services.AddScoped<IStoryService, StoryService>();
            Services.AddScoped<IWaterpumpProjectService, WaterpumpProjectService>();
			Services.AddScoped<ITransactionService, TransactionService>();
			Services.AddScoped<IPaypalClientService, PaypalClientService>();
			Services.AddScoped<IKeyVaultService, KeyVaultService>();

			Services.AddHttpClient<PaypalClientService>();

			Services.AddOptions<BlobCredentialOptions>()
			   .Configure<IConfiguration>((settings, configuration) =>
			   {
				   configuration.GetSection(nameof(BlobCredentialOptions)).Bind(settings);
			   });
		}

		private static string GetCosmosDbKey()
        {
			AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
			var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

			var cosmosDbKey = keyVaultClient.GetSecretAsync(
				Environment.GetEnvironmentVariable("KeyVaultUri") + "CosmosDbkey").GetAwaiter().GetResult().Value;

			return cosmosDbKey;
		}
	}
}


