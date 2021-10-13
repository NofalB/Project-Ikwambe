using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Clients
{
    public interface IPaypalClientService
    {
        Task<CheckoutUrl> GetCheckoutUrl(string currencyCode, string value);
        Task<Transaction> GetTransaction(string orderId);
        Task<Transaction> CaptureTransaction(string orderId);
    }
}
