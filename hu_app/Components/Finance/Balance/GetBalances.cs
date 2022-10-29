using AutoMapper;
using FluentValidation;
using hu_app.Models.Entities.Finance;
using hu_app.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hu_app.Components.Finance.Balance
{
    public class GetBalancesRequest : IHuRequest
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public Guid? UserId { get; set; }
    }

    public class GetBalancesValidatior : AbstractValidator<GetBalancesRequest>
    {
        public GetBalancesValidatior()
        {
            RuleFor(x => x.Year).NotEmpty().GreaterThanOrEqualTo(2020).LessThanOrEqualTo(2099);
            RuleFor(x => x.Month).NotEmpty().GreaterThanOrEqualTo(1).LessThanOrEqualTo(12);
            RuleFor(x => x.UserId).NotEmpty();
        }
    }

    public class GetBalancesHandler : HuRequestHandler<GetBalancesRequest>
    {
        private readonly IMapper _mapper;
        private readonly HuRepository<FinanceTransaction> _transactionRepo;
        private readonly string[] MerchantsToIgnore = new string[] { "Credit Card Payment" };
        private readonly string[] MerchantsToKeep = new string[] { "Rent" };

        public GetBalancesHandler(IMapper mapper, HuRepository<FinanceTransaction> transactionRepo)
        {
            _mapper = mapper;
            _transactionRepo = transactionRepo;
        }

        public override async Task Load(GetBalancesRequest request)
        {
            var startDate = new DateTime(request.Year.Value, request.Month.Value, 1);
            var endDate = new DateTime(request.Year.Value, request.Month.Value, DateTime.DaysInMonth(request.Year.Value, request.Month.Value));

            var transactions = await _transactionRepo.GetQueryable()
                .Include(x => x.Item.Merchant)
                .Include(x => x.OtherItem.Merchant)
                .Where(x => x.UserId == request.UserId.Value
                         && x.Date.Year == request.Year.Value
                         && x.Date.Month == request.Month.Value)
                .OrderByDescending(x => x.Date).ThenBy(x => x.Item.Merchant.Name).ThenBy(x => x.Item.Name)
                .ToListAsync();

            var transactionDTOs = transactions.Select(x => _mapper.Map<TransactionDTO>(x)).ToList();

            var creditDTOs = new List<BalanceDTO>();
            var debitDTOs = new List<BalanceDTO>();
            foreach (var t in transactions)
            {
                var merchantId = t.OtherItem?.MerchantId ?? t.Item?.MerchantId;
                var merchantName = t.OtherItem?.Merchant?.Name ?? t.Item?.Merchant?.Name;
                if (string.IsNullOrEmpty(merchantName)
                    || MerchantsToIgnore.Contains(merchantName)
                        || (!t.InBalance && !MerchantsToKeep.Contains(merchantName)))
                {
                    continue;
                }
                if (t.Credit.HasValue)
                {
                    var creditDTO = creditDTOs.FirstOrDefault(x => x.MerchantName == merchantName);
                    if (creditDTO == null)
                    {
                        creditDTO = new BalanceDTO { MerchantId = merchantId.Value, MerchantName = merchantName };
                        creditDTOs.Add(creditDTO);
                    }
                    creditDTO.Amount += t.Credit.Value;
                }
                if (t.Debit.HasValue)
                {
                    var debitDTO = debitDTOs.FirstOrDefault(x => x.MerchantName == merchantName);
                    if (debitDTO == null)
                    {
                        debitDTO = new BalanceDTO { MerchantId = merchantId.Value, MerchantName = merchantName };
                        debitDTOs.Add(debitDTO);
                    }
                    debitDTO.Amount += t.Debit.Value;
                }
            }

            Data = new BalanceDetailsDTO
            {
                Credits = creditDTOs.OrderBy(x => x.Amount).ToList(),
                Debits = debitDTOs.OrderBy(x => x.Amount).ToList(),
                Transactions = transactionDTOs
            };
        }
    }
}
