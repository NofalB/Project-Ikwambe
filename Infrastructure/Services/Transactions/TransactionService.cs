using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.DTO;
using Infrastructure.Repositories;
using Infrastructure.Services.Clients;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Transactions
{
    public class TransactionService : ITransactionService
    {
        private readonly ICosmosReadRepository<Transaction> _transactionReadRepository;
        private readonly ICosmosWriteRepository<Transaction> _transactionWriteRepository;
        private readonly IPaypalClientService _paypalClientService;
        private readonly IDonationService _donationService;

        public TransactionService(ICosmosReadRepository<Transaction> transactionReadRepository, ICosmosWriteRepository<Transaction> transactionWriteRepository, IPaypalClientService paypalClientService, IDonationService donationService)
        {
            _transactionReadRepository = transactionReadRepository;
            _transactionWriteRepository = transactionWriteRepository;
            _paypalClientService = paypalClientService;
            _donationService = donationService;
        }
        public async Task<Transaction> AddTransaction(Transaction transaction)
        {
            transaction.PartitionKey = Guid.NewGuid().ToString();

            //Paypal payer name field returns null for full name and instead fills the given name and surname from purchase units, this fixes that
            transaction.Payer.Name.FullName = transaction.Payer.Name.GivenName;
            transaction.PurchaseUnits.ForEach(x => x.Shipping.ShippingId = Guid.NewGuid().ToString());
            //couldnt set they [key] attribute so have to set my own
            transaction.Payer.Address.AddressId = Guid.NewGuid().ToString();
            transaction.PurchaseUnits.ForEach(x => x.Shipping.Address.AddressId = Guid.NewGuid().ToString());
            transaction.PurchaseUnits.ForEach(p => p.Payments.PaymentsId = Guid.NewGuid().ToString());

            return await _transactionWriteRepository.AddAsync(transaction);
        }

        //public async Task<Transaction> DeleteTransaction(string transactionId)
        //{
        //    return await _transactionRepository.GetAll().FirstOrDefaultAsync(t => t.TransactionId == transactionId);
        //}

        public async Task<IEnumerable<Transaction>> GetAllTransactions()
        {
            return await _transactionReadRepository.GetAll().ToListAsync();
        }

        public async Task<Transaction> GetTransactionById(string transactionId)
        {
            return await _transactionReadRepository.GetAll().FirstOrDefaultAsync(t => t.TransactionId == transactionId);
        }

        public async Task CompleteTransaction(string transactionId,Guid userId)
        {
            var transaction=await _paypalClientService.GetTransaction(transactionId);
            if (transaction.Status == "APPROVED")
            {
                //this captures the funds after the payer buys or approves of the payment and sets the status as "Completed"
                await _paypalClientService.CaptureTransaction(transactionId);
                transaction = await _paypalClientService.GetTransaction(transactionId);

                if (transaction.Status == "COMPLETED")
                {
                    DonationDTO donation = new DonationDTO()
                    {
                        UserId = userId != Guid.Empty ? userId : Guid.Empty, 
                        ProjectId = Guid.NewGuid(),
                        TransactionId = transactionId,
                        Amount = double.Parse(transaction.PurchaseUnits[0].Amount.Value),
                        DonationDate = DateTime.Now,
                    };
                    await AddTransaction(transaction);
                    await _donationService.AddDonation(donation);
                }
            }
            
        }


        //public Task<Transaction> UpdateTransaction(Transaction transaction)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
