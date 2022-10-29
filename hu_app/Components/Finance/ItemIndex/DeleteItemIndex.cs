using FluentValidation;
using hu_app.Models.Entities.Finance;
using hu_app.Shared;
using System;
using System.Threading.Tasks;

namespace hu_app.Components.Finance.ItemIndex
{
    public class DeleteItemIndexRequest : IHuRequest
    {
        public Guid? Id { get; set; }
    }

    public class DeleteItemIndexValidator : AbstractValidator<DeleteItemIndexRequest>
    {
        public DeleteItemIndexValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }

    public class DeleteItemIndexHandler : HuRequestHandler<DeleteItemIndexRequest>
    {
        private readonly HuRepository<FinanceItemIndex> _itemRepo;

        public DeleteItemIndexHandler(HuRepository<FinanceItemIndex> itemRepo)
        {
            _itemRepo = itemRepo;
        }

        public override async Task Process(DeleteItemIndexRequest request)
        {
            var item = await _itemRepo.GetById(request.Id.Value);
            await _itemRepo.Delete(item);
        }
    }
}
