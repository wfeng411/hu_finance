using AutoMapper;
using FluentValidation;
using hu_app.Models.Entities.Finance;
using hu_app.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hu_app.Components.Finance.Merchant
{
    public class SaveMerchantRequest : IHuRequest
    {
        public Guid? Id { get; set; }
        public string MerchantName { get; set; }
        public string Note { get; set; }
        public Guid? ExpenseTypeId { get; set; }
        public bool Ignore { get; set; }
        public int? Order { get; set; }
    }

    public class SaveMerchantValidator : AbstractValidator<SaveMerchantRequest>
    {
        private readonly HuRepository<FinanceMerchant> _merchantRepo;

        public SaveMerchantValidator(HuRepository<FinanceMerchant> merchantRepo)
        {
            _merchantRepo = merchantRepo;
            RuleFor(x => x.MerchantName).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MustAsync(MerchantNotExist).When(x => !x.Id.HasValue, ApplyConditionTo.CurrentValidator).WithMessage("Merchant already exists.");
            RuleFor(x => x.ExpenseTypeId).NotEmpty();
        }

        private async Task<bool> MerchantNotExist(string name, CancellationToken cancellationToken)
        {
            var exist = await _merchantRepo.GetQueryable()
                .AnyAsync(x => x.Name.ToUpper() == name.Trim().Replace("\t", "").Replace("\n", "").ToUpper());
            return !exist;
        }
    }

    public class SaveMerchantHandler : HuRequestHandler<SaveMerchantRequest>
    {
        private readonly HuRepository<FinanceTransaction> _merchantRepo;
        private readonly IMapper _mapper;

        public SaveMerchantHandler(HuRepository<FinanceTransaction> merchantRepo, IMapper mapper)
        {
            _merchantRepo = merchantRepo;
            _mapper = mapper;
        }

        public override async Task Process(SaveMerchantRequest request)
        {
            if (request.Id.HasValue)
            {
                var merchant = await _merchantRepo.GetById(request.Id.Value);
                _mapper.Map(request, merchant);
                await _merchantRepo.Update(merchant);
            }
            else
            {
                var merchant = _mapper.Map<FinanceTransaction>(request);
                await _merchantRepo.Create(merchant);
            }
        }
    }
}
