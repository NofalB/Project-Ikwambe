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
            
            //Paypal payer name field returns null for full name and instead fills the given name and surname from purchase units, this fixes that
            transaction.Payer.Name.FullName = transaction.Payer.Name.GivenName;
            transaction.PurchaseUnits.ForEach(x => x.Shipping.ShippingId = Guid.NewGuid().ToString());
            //couldnt set they [key] attribute so have to set my own
            transaction.Payer.Address.AddressId = Guid.NewGuid().ToString();
            transaction.PurchaseUnits.ForEach(x => x.Shipping.Address.AddressId = Guid.NewGuid().ToString());
            transaction.PurchaseUnits.ForEach(p => p.Payments.PaymentsId = Guid.NewGuid().ToString());

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
