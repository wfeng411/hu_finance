using FluentValidation;
using hu_app.Models.Entities.Finance;
using hu_app.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hu_app.Components.Finance.Merchant
{
    public class DeleteMerchantRequest : IHuRequest
    {
        public Guid? Id { get; set; }
    }

    public class DeleteMerchantValidator : AbstractValidator<DeleteMerchantRequest>
    {
        private readonly HuRepository<FinanceItem> _itemRepo;
        private readonly HuRepository<FinanceItemIndex> _itemIndexRepo;
        private readonly HuRepository<FinanceTransaction> _merchantRepo;
        private FinanceTransaction _merchant;

        public DeleteMerchantValidator(
            HuRepository<FinanceItem> itemRepo,
            HuRepository<FinanceItemIndex> itemIndexRepo,
            HuRepository<FinanceTransaction> merchantRepo)
        {
            _itemRepo = itemRepo;
            _itemIndexRepo = itemIndexRepo;
            _merchantRepo = merchantRepo;
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x).MustAsync(NoItems).WithMessage("Merchant with Items cannot be deleted.");
        }

        private async Task<bool> NoItems(DeleteMerchantRequest request, CancellationToken cancellationToken)
        {
            _merchant ??= await _merchantRepo.GetById(request.Id.Value);
            bool hasItems = await _itemRepo.GetQueryable().AnyAsync(x => x.MerchantId == request.Id.Value);
            if (hasItems)
            {
                return false;
            }
            bool hasItemIndexes = await _itemIndexRepo.GetQueryable().AnyAsync(x => x.MerchantId == request.Id.Value);
            if (hasItemIndexes)
            {
                return false;
            }
            return true;
        }
    }

    public class DeleteMerchantHandler : HuRequestHandler<DeleteMerchantRequest>
    {
        private readonly HuRepository<FinanceTransaction> _merchantRepo;

        public DeleteMerchantHandler(HuRepository<FinanceTransaction> merchantRepo)
        {
            _merchantRepo = merchantRepo;
        }

        public override async Task Process(DeleteMerchantRequest request)
        {
            var merchant = await _merchantRepo.GetById(request.Id.Value);
            await _merchantRepo.Delete(merchant);
        }
    }
}