using hu_app.Models.Entities.Finance;
using hu_app.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace hu_app.Components.Finance.Transaction
{
    public class CheckTransactionRequest : IHuRequest
    {
        public SaveTransactionDTO Transaction { get; set; }
    }

    public class CheckTransactionHandler : HuRequestHandler<CheckTransactionRequest>
    {
        private readonly HuRepository<FinanceTransaction> _transactionRepo;
        private readonly HuRepository<FinanceItem> _itemRepo;
        private readonly HuRepository<FinanceItemIndex> _itemIndexRepo;

        public CheckTransactionHandler(
            HuRepository<FinanceTransaction> transactionRepo,
            HuRepository<FinanceItem> itemRepo,
            HuRepository<FinanceItemIndex> itemIndexRepo)
        {
            _transactionRepo = transactionRepo;
            _itemRepo = itemRepo;
            _itemIndexRepo = itemIndexRepo;
        }

        public override async Task Load(CheckTransactionRequest request)
        {
            var t = request.Transaction;

            if (!t.Date.HasValue ||
                string.IsNullOrWhiteSpace(t.ItemName) ||
                !t.TransactionTypeId.HasValue ||
                (!t.Debit.HasValue && !t.Credit.HasValue) ||
                !t.UserId.HasValue)
            {
                Data = HuConstants.Finance.TransactionResult.Error;
                return;
            }

            var itemName = t.ItemName.Trim();

            var transaction = await _transactionRepo.GetQueryable()
                .Include(x => x.Item).ThenInclude(x => x.Merchant)
                .FirstOrDefaultAsync(x => x.OriginalDate == t.Date
                                          && x.Item.Name == itemName
                                          && x.TransactionTypeId == t.TransactionTypeId
                                          && x.Debit == t.Debit
                                          && x.Credit == t.Credit
                                          && x.UserId == t.UserId);

            if (transaction != null)
            {
                Data = new CheckTransactionResultDTO
                {
                    Result = HuConstants.Finance.TransactionResult.Exist,
                    MerchantName = transaction.Item?.Merchant?.Name
                };
            }
            else
            {
                string merchantName = null;
                var item = await _itemRepo.GetQueryable()
                    .Include(x => x.Merchant)
                    .FirstOrDefaultAsync(x => x.Name == itemName);
                if (item != null)
                {
                    merchantName = item.Merchant?.Name;
                }
                else
                {
                    var indexes = await _itemIndexRepo.GetQueryable()
                        .Include(x => x.Merchant)
                        .Where(x => itemName.ToUpper().Contains(x.Name))
                        .ToListAsync();
                    if (indexes.Count == 1)
                    {
                        merchantName = indexes[0].Merchant?.Name;
                    }
                }
                Data = new CheckTransactionResultDTO
                {
                    Result = HuConstants.Finance.TransactionResult.New,
                    MerchantName = merchantName
                };
            }
        }
    }
}
