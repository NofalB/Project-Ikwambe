using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PayPalCheckoutSdk.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace PaymentMicroservices.Services
{
    public class PaypalClientService
    {
        static readonly IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        static readonly String clientId = config["Values:CLIENT_ID"];
        static readonly String secret = config["Values:SECRET"];

        public static PayPalHttpClient Client()
        {
            // Creating a sandbox environment
            PayPalEnvironment environment = new SandboxEnvironment(clientId, secret);

            // Creating a client for the environment
            PayPalHttpClient client = new PayPalHttpClient(environment);
            return client;
        }

    }
}
