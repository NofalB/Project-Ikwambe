using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.KeyVault
{
    public interface IKeyVaultService
    {
        Task<string> GetSecretValue(string secretName);
    }
}
