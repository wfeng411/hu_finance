using AutoMapper;
using FluentValidation;
using hu_app.Models.Entities.Finance;
using hu_app.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hu_app.Components.Finance.Transaction
{
    public class SaveTransactionsRequest : IHuRequest
    {
        public List<SaveTransactionDTO> Transactions { get; set; } = new List<SaveTransactionDTO>();
    }

    public class SaveTransactionsValidatior : AbstractValidator<SaveTransactionsRequest>
    {
        public SaveTransactionsValidatior()
        {
            RuleFor(x => x.Transactions).NotEmpty();
            RuleForEach(x => x.Transactions).SetValidator(new TransactionValidator());
        }
    }

    public class TransactionValidator : AbstractValidator<SaveTransactionDTO>
    {
        public TransactionValidator()
        {
            RuleFor(x => x.Date).NotEmpty().NotEqual(DateTime.MinValue).NotEqual(DateTime.MaxValue);
            RuleFor(x => x.ItemName).NotEmpty();
            RuleFor(x => x.TransactionTypeId).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(x => x.UserId).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(x => x.Debit).NotEmpty().GreaterThan(0).When(x => !x.Credit.HasValue);
            RuleFor(x => x.Credit).NotEmpty().GreaterThan(0).When(x => !x.Debit.HasValue);
        }
    }

    public class SaveTransactionsHandler : HuRequestHandler<SaveTransactionsRequest>
    {
        private readonly HuRepository<FinanceTransaction> _transactionRepo;
        private readonly HuRepository<FinanceItem> _itemRepo;
        private readonly HuRepository<FinanceItemIndex> _itemIndexRepo;
        private readonly IMapper _mapper;

        public SaveTransactionsHandler(
            HuRepository<FinanceTransaction> transactionRepo,
            HuRepository<FinanceItem> itemRepo,
            HuRepository<FinanceItemIndex> itemIndexRepo,
            IMapper mapper)
        {
            _transactionRepo = transactionRepo;
            _itemRepo = itemRepo;
            _itemIndexRepo = itemIndexRepo;
            _mapper = mapper;
        }

        public override async Task Load(SaveTransactionsRequest request)
        {
            int transactionsCreatedCount = 0;
            int itemsCreatedCount = 0;
            int itemsNeedToMapCount = 0;

            var itemIndexes = await _itemIndexRepo.GetQueryable()
                .Include(x => x.Merchant)
                .ToListAsync();

            foreach (var t in request.Transactions)
            {
                var itemName = t.ItemName.Trim();
                var item = _itemRepo.GetQueryable()
                    .Include(x => x.Merchant)
                    .SingleOrDefault(x => x.Name == itemName);
                if (item == null)
                {
                    item = new FinanceItem { Name = itemName };
                    var matchedItems = itemIndexes.Where(x => itemName.ToUpper().Contains(x.Name)).ToList();
                    if (matchedItems.Count == 1)
                    {
                        item.MerchantId = matchedItems[0].MerchantId;
                        item.Merchant = matchedItems[0].Merchant;
                    }
                    else
                    {
                        itemsNeedToMapCount++;
                    }
                    await _itemRepo.Create(item);
                    itemsCreatedCount++;
                }

                var transaction = _transactionRepo.GetQueryable()
                    .FirstOrDefault(x => x.OriginalDate == t.Date
                                      && x.ItemId == item.Id
                                      && x.TransactionTypeId == t.TransactionTypeId.Value
                                      && x.Debit == t.Debit
                                      && x.Credit == t.Credit
                                      && x.UserId == t.UserId.Value);

                if (transaction == null)
                {
                    transaction = _mapper.Map<FinanceTransaction>(t);
                    transaction.ItemId = item.Id;
                    transaction.OriginalDate = t.Date.Value;
                    transaction.InBalance = !(item.Merchant?.Ignore ?? false);
                    await _transactionRepo.Create(transaction);
                    transactionsCreatedCount++;
                }
            }
            Data = new { transactionsCreatedCount, itemsCreatedCount, itemsNeedToMapCount };
        }
    }
}
