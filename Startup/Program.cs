using Microsoft.Azure.Functions.Worker.Extensions.OpenApi;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Functions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectIkwambe.CosmosDAL;
using ProjectIkwambe.Repositories;
using ProjectIkwambe.Services;

namespace ProjectIkwambe.Startup {
	public class Program {
		public static void Main() {
			IHost host = new HostBuilder()
				.ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
				.ConfigureServices(Configure)
				.Build();

			host.Run();
		}

		static void Configure(HostBuilderContext Builder, IServiceCollection Services) {
			Services.AddSingleton<IOpenApiHttpTriggerContext, OpenApiHttpTriggerContext>();
			Services.AddSingleton<IOpenApiTriggerFunction, OpenApiTriggerFunction>();

            Services.AddDbContext<IkambeContext>(option =>
            {
                option.UseCosmos(
                    Builder.Configuration["CosmosDb:Account"],
                    Builder.Configuration["CosmosDb:Key"],
                    Builder.Configuration["CosmosDb:DatabaseName"]
                );
            });

			Services.AddTransient<ICosmosRepository, CosmosRepository>();
			Services.AddScoped<IDonationService, DonationService>();
        }
	}
}


