using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IWaterpumpProjectService _waterpumpProjectService;
        private readonly IUserService _userService;

        public TransactionService(ICosmosReadRepository<Transaction> transactionReadRepository, ICosmosWriteRepository<Transaction> transactionWriteRepository, IPaypalClientService paypalClientService, IDonationService donationService, IWaterpumpProjectService waterpumpProjectService, IUserService userService)
        {
            _transactionReadRepository = transactionReadRepository;
            _transactionWriteRepository = transactionWriteRepository;
            _paypalClientService = paypalClientService;
            _donationService = donationService;
            _waterpumpProjectService = waterpumpProjectService;
            _userService = userService;
        }
        public async Task<Transaction> AddTransaction(Transaction transaction)
        {
            transaction.PartitionKey = transaction.TransactionId;

            //Paypal payer name field returns null for full name and instead fills the given name and surname from purchase units, this fixes that
            transaction.Payer.Name.FullName = transaction.Payer.Name.GivenName;
            transaction.PurchaseUnits.ForEach(x => x.Shipping.ShippingId = Guid.NewGuid().ToString());
            //couldnt set the [key] attribute so have to set my own
            transaction.Payer.Address.AddressId = Guid.NewGuid().ToString();
            transaction.PurchaseUnits.ForEach(x => x.Shipping.Address.AddressId = Guid.NewGuid().ToString());
            transaction.PurchaseUnits.ForEach(p => p.Payments.PaymentsId = Guid.NewGuid().ToString());

            return await _transactionWriteRepository.AddAsync(transaction);
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactions()
        {
            return await _transactionReadRepository.GetAll().ToListAsync();
        }

        public async Task<Transaction> GetTransactionById(string transactionId)
        {
            var transaction =  await _transactionReadRepository.GetAll().FirstOrDefaultAsync(t => t.TransactionId == transactionId);

            if (transaction == null)
            {
                throw new InvalidOperationException($"No transaction exists with the ID {transactionId}");
            }

            return transaction;
        }

        public async Task CompleteTransaction(string transactionId,string projectId, string userId)
        {
            var transaction = await _paypalClientService.GetTransaction(transactionId);
            var project = await _waterpumpProjectService.GetWaterPumpProjectById(projectId);
            
            //check if the userid is given or not. if user is not provided set it to null, if provided convert to Guid.
            Guid? userNumb = null;
            if (!string.IsNullOrEmpty(userId))
            {
                userNumb = Guid.Parse(userId);
            }

            if (project != null)
            {
                if (transaction.Status == "APPROVED")
                {
                    //this captures the funds after the payer buys or approves of the payment and sets the status as "Completed"
                    await _paypalClientService.CaptureTransaction(transactionId);
                    transaction = await _paypalClientService.GetTransaction(transactionId);
                    transaction.ProjectId = Guid.Parse(projectId);

                    if (transaction.Status == "COMPLETED")
                    {
                        DonationDTO donationDTO = new DonationDTO()
                        {
                            UserId = userNumb,
                            ProjectId = Guid.Parse(projectId),
                            TransactionId = transactionId,
                            //Amount = double.Parse(transaction.PurchaseUnits[0].Amount.Value, CultureInfo.InvariantCulture),
                        };
                        await AddTransaction(transaction);
                        var donationDb = await _donationService.AddDonation(donationDTO);

                        var donationCheck = await _donationService.GetDonationByIdAsync(donationDb.DonationId.ToString());
                        if (donationCheck != null)
                        {
                            project.CurrentTotal += donationDb.Amount;
                            //check with the guys about this.
                            project.TotalNumbOfDonators += 1;
                            await _waterpumpProjectService.UpdateWaterPumpProject(project);
                        }
                    }
                }
            }
        }


        public async Task CompleteTransaction1(DonationDTO donationDTO)
        {
            var transaction = await _paypalClientService.GetTransaction(donationDTO.TransactionId);
            var project = await _waterpumpProjectService.GetWaterPumpProjectById(donationDTO.ProjectId.ToString());

            //check if the userid is given or not. if user is not provided set it to null, if provided convert to Guid.
            Guid? userNumb = null;
            if (!string.IsNullOrEmpty(donationDTO.UserId.ToString()))
            {
                userNumb = Guid.Parse(donationDTO.ToString());
            }

            if (project != null)
            {
                if (transaction.Status == "APPROVED")
                {
                    //this captures the funds after the payer buys or approves of the payment and sets the status as "Completed"
                    await _paypalClientService.CaptureTransaction(donationDTO.TransactionId);
                    transaction = await _paypalClientService.GetTransaction(donationDTO.TransactionId);
                    transaction.ProjectId = Guid.Parse(donationDTO.ProjectId.ToString());

                    if (transaction.Status == "COMPLETED")
                    {
                        DonationDTO NewDonationDTO = new DonationDTO()
                        {
                            UserId = userNumb,
                            ProjectId = Guid.Parse(donationDTO.ProjectId.ToString()),
                            TransactionId = donationDTO.TransactionId,
                            //Amount = double.Parse(transaction.PurchaseUnits[0].Amount.Value, CultureInfo.InvariantCulture),
                            Comment = donationDTO.Comment,
                            Name = donationDTO.Name
                        };
                        await AddTransaction(transaction);
                        await _donationService.AddDonation(NewDonationDTO);
                    }
                }
            }
        }
    }
}
