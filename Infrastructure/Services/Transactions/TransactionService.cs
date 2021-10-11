using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.DTO;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Transactions
{
    public class TransactionService : ITransactionService
    {
        private readonly ICosmosRepository<Transaction> _transactionRepository;

        public TransactionService(ICosmosRepository<Transaction> transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public async Task<Transaction> AddTransaction(Transaction transaction)
        {
            return await _transactionRepository.AddAsync(transaction);
        }

        //public async Task<Transaction> DeleteTransaction(string transactionId)
        //{
        //    return await _transactionRepository.GetAll().FirstOrDefaultAsync(t => t.TransactionId == transactionId);
        //}

        public async Task<IEnumerable<Transaction>> GetAllTransactions()
        {
            return await _transactionRepository.GetAll().ToListAsync();
        }

        public async Task<Transaction> GetTransactionById(string transactionId)
        {
            return await _transactionRepository.GetAll().FirstOrDefaultAsync(t => t.TransactionId == transactionId);
        }

        //public Task<Transaction> UpdateTransaction(Transaction transaction)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
