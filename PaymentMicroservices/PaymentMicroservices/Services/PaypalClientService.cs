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
        static readonly string _clientId = config["Values:CLIENT_ID"];
        static readonly string _secret = config["Values:SECRET"];

        public static PayPalHttpClient Client()
        {
            // Creating a sandbox environment
            PayPalEnvironment environment = new SandboxEnvironment(_clientId, _secret);

            // Creating a client for the environment
            PayPalHttpClient client = new PayPalHttpClient(environment);
            return client;
        }

        public static String ObjectToJSONString(Object serializableObject)
        {
            MemoryStream memoryStream = new MemoryStream();
            var writer = JsonReaderWriterFactory.CreateJsonWriter(
                        memoryStream, Encoding.UTF8, true, true, "  ");
            DataContractJsonSerializer ser = new DataContractJsonSerializer(serializableObject.GetType(), new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true });
            ser.WriteObject(writer, serializableObject);
            memoryStream.Position = 0;
            StreamReader sr = new StreamReader(memoryStream);
            return sr.ReadToEnd();
        }

    }
}
