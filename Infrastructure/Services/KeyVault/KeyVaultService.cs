using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.KeyVault
{
    public class KeyVaultService : IKeyVaultService
    {
        public async Task<string> GetSecretValue(string secretName)
        {
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            
            var secret = await keyVaultClient.GetSecretAsync(Environment.GetEnvironmentVariable("KeyVaultUri") + secretName);
            
            return secret.Value;
        }
    }
}
