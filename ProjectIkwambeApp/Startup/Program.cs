using Domain;
using Infrastructure.DBContext;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Functions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectIkwambe.Security;

namespace ProjectIkwambe.Startup {
	public class Program {
		public static void Main() {
			IHost host = new HostBuilder()
				//.ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
				.ConfigureFunctionsWorkerDefaults((IFunctionsWorkerApplicationBuilder Builder) => {
					Builder.UseNewtonsoftJson().UseMiddleware<JwtMiddleware>();
				})
				.ConfigureServices(Configure)
				.Build();

			host.Run();
		}

		static void Configure(HostBuilderContext Builder, IServiceCollection Services) {
			Services.AddSingleton<IOpenApiHttpTriggerContext, OpenApiHttpTriggerContext>();
			Services.AddSingleton<IOpenApiTriggerFunction, OpenApiTriggerFunction>();
			//jwt security
			Services.AddSingleton<ITokenService, TokenService>();

			// DBContext
			Services.AddDbContext<IkwambeContext>(option =>
            {
                option.UseCosmos(
                    Builder.Configuration["CosmosDb:Account"],
                    Builder.Configuration["CosmosDb:Key"],
                    Builder.Configuration["CosmosDb:DatabaseName"]
                );
            });

			// Repositories
			Services.AddTransient(typeof(ICosmosRepository<>), typeof(CosmosRepository<>));


            // Services
            Services.AddScoped<IDonationService, DonationService>();
            Services.AddScoped<IUserService, UserService>();
            Services.AddScoped<IStoryService, StoryService>();
            /*Services.AddScoped<IWaterpumpProjectService, WaterpumpProjectService>();*/
        }
	}
}


