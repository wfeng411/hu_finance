using AutoMapper;
using FluentValidation;
using hu_app.Models.Entities.Finance;
using hu_app.Shared;
using System;
using System.Threading.Tasks;

namespace hu_app.Components.Finance.Merchant
{
    public class UpdateItemMerchantIdRequest : IHuRequest
    {
        public Guid? Id { get; set; }

        public Guid? MerchantId { get; set; }
    }

    public class UpdateItemMerchantIdValidatior : AbstractValidator<UpdateItemMerchantIdRequest>
    {
        public UpdateItemMerchantIdValidatior()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.MerchantId).NotEmpty();
        }
    }

    public class UpdateItemMerchantIdHandler : HuRequestHandler<UpdateItemMerchantIdRequest>
    {
        private readonly IMapper _mapper;
        private readonly HuRepository<FinanceItem> _itemRepo;

        public UpdateItemMerchantIdHandler(IMapper mapper, HuRepository<FinanceItem> itemRepo)
        {
            _mapper = mapper;
            _itemRepo = itemRepo;
        }

        public override async Task Process(UpdateItemMerchantIdRequest request)
        {
            var item = await _itemRepo.GetById(request.Id.Value);
            item.MerchantId = request.MerchantId.Value;
            await _itemRepo.Update(item);
        }
    }
}
