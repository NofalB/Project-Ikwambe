using Domain;
using Infrastructure.DBContext;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.Clients;
using Infrastructure.Services.Transactions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Functions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectIkwambe.ErrorHandlerMiddleware;
using ProjectIkwambe.Security;

namespace ProjectIkwambe.Startup {
	public class Program {
		public static void Main() {
			IHost host = new HostBuilder()
				//.ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
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
			//Services.AddSingleton<IOpenApiHttpTriggerContext, OpenApiHttpTriggerContext>();
			//Services.AddSingleton<IOpenApiTriggerFunction, OpenApiTriggerFunction>();
			//jwt security
			Services.AddSingleton<ITokenService, TokenService>();

			// DBContext
			Services.AddDbContext<IkwambeContext>(option =>
            {
				//option.UseCosmos("https://projectikwambedb.documents.azure.com:443/", "0gHgOaqhe8NAjY0b02DurzqSZHiKI5NF9zQsRkAhqJsJmOIcPylMGZR44ZzmLSrbkhztzQeW8AKfu7BJnZ2nYQ==", "ProjectIkwambeDB");
				option.UseCosmos(
                    Builder.Configuration["CosmosDb:Account"],
                    Builder.Configuration["CosmosDb:Key"],
                    Builder.Configuration["CosmosDb:DatabaseName"]
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

			Services.AddHttpClient<PaypalClientService>(c => c.BaseAddress = new System.Uri("https://paypalmicroserviceikwambe.azurewebsites.net/api/"));

			Services.AddOptions<BlobCredentialOptions>()
			   .Configure<IConfiguration>((settings, configuration) =>
			   {
				   configuration.GetSection(nameof(BlobCredentialOptions)).Bind(settings);
			   });
		}
	}
}


