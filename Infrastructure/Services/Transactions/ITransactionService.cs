using Domain;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetAllTransactions();

        Task<Transaction> GetTransactionById(string transactionId);

        Task<Transaction> AddTransaction(Transaction transaction);
        Task CompleteTransaction(string transactionId, Guid userId, string projectId);
    }
}
