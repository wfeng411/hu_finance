using FluentValidation;
using hu_app.Models.Entities.Finance;
using hu_app.Shared;
using System;
using System.Threading.Tasks;

namespace hu_app.Components.Finance.Balance
{
    public class ToggleBalanceRequest : IHuRequest
    {
        public Guid? TransactionId { get; set; }
    }

    public class ToggleBalanceValidatior : AbstractValidator<ToggleBalanceRequest>
    {
        public ToggleBalanceValidatior()
        {
            RuleFor(x => x.TransactionId).NotEmpty();
        }
    }

    public class ToggleBalanceHandler : HuRequestHandler<ToggleBalanceRequest>
    {
        private readonly HuRepository<FinanceTransaction> _transactionRepo;

        public ToggleBalanceHandler(HuRepository<FinanceTransaction> transactionRepo)
        {
            _transactionRepo = transactionRepo;
        }

        public override async Task Process(ToggleBalanceRequest request)
        {
            var transaction = await _transactionRepo.GetById(request.TransactionId.Value);
            transaction.InBalance = !transaction.InBalance;
            await _transactionRepo.Update(transaction);
        }
    }
}
