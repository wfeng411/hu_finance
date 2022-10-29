using AutoMapper;
using FluentValidation;
using hu_app.Models.Entities.Finance;
using hu_app.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hu_app.Components.Finance.ItemIndex
{
    public class SaveItemIndexRequest : IHuRequest
    {
        public Guid? Id { get; set; }
        public string ItemName { get; set; }
        public Guid? MerchantId { get; set; }
    }

    public class SaveItemIndexValidator : AbstractValidator<SaveItemIndexRequest>
    {
        private readonly HuRepository<FinanceItem> _itemRepo;

        public SaveItemIndexValidator(HuRepository<FinanceItem> itemRepo)
        {
            _itemRepo = itemRepo;
            RuleFor(x => x.ItemName).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MustAsync(ItemNotExist).When(x => !x.Id.HasValue, ApplyConditionTo.CurrentValidator).WithMessage("Item already exists.");
            RuleFor(x => x.MerchantId).NotEmpty();
        }

        private async Task<bool> ItemNotExist(string name, CancellationToken cancellationToken)
        {
            var exist = await _itemRepo.GetQueryable()
                .AnyAsync(x => x.Name.ToUpper() == name.Trim().Replace("\t", "").Replace("\n", "").ToUpper());
            return !exist;
        }
    }

    public class SaveItemIndexHandler : HuRequestHandler<SaveItemIndexRequest>
    {
        private readonly HuRepository<FinanceItemIndex> _itemRepo;
        private readonly IMapper _mapper;

        public SaveItemIndexHandler(HuRepository<FinanceItemIndex> itemRepo, IMapper mapper)
        {
            _itemRepo = itemRepo;
            _mapper = mapper;
        }

        public override async Task Process(SaveItemIndexRequest request)
        {
            request.ItemName = request.ItemName.Trim().ToUpper();
            var item = _mapper.Map<FinanceItemIndex>(request);
            if (request.Id.HasValue)
            {
                await _itemRepo.Update(item);
            }
            else
            {
                await _itemRepo.Create(item);
            }
        }
    }
}
