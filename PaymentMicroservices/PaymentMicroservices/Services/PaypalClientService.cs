using PayPalCheckoutSdk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentMicroservices.Services
{
    public class PaypalClientService
    {
        static String clientId = "AVqSBkkqxw0jOZsOL1meIPnI25Ozcj12Ec8xgZqTrf6R80GftrWaxPDFvC0j4YO2K_Kyze2yNTgwhPRi";
        static String secret = "EBBGNsdoiHdTKNZk1ax86VSNmzxIYG3A1rO2m0C3A2Hoa24Z5hvrQsm2oZmnF0KF2dL3iOkQNtf7tob5";

        public static PayPalHttpClient client()
        {
            // Creating a sandbox environment
            PayPalEnvironment environment = new SandboxEnvironment(clientId, secret);

            // Creating a client for the environment
            PayPalHttpClient client = new PayPalHttpClient(environment);
            return client;
        }
    }
}
