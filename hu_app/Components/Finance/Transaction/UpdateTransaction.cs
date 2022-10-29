using FluentValidation;
using hu_app.Models.Entities.Finance;
using hu_app.Shared;
using System;
using System.Threading.Tasks;

namespace hu_app.Components.Finance.Transaction
{
    public class UpdateTransactionRequest : IHuRequest
    {
        public Guid? Id { get; set; }
        public DateTime? Date { get; set; }
        public Guid? OtherItemId { get; set; }
    }

    public class UpdateTransactionValidatior : AbstractValidator<UpdateTransactionRequest>
    {
        public UpdateTransactionValidatior()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class UpdateTransactionHandler : HuRequestHandler<UpdateTransactionRequest>
    {
        private readonly HuRepository<FinanceTransaction> _transactionRepo;

        public UpdateTransactionHandler(HuRepository<FinanceTransaction> transactionRepo)
        {
            _transactionRepo = transactionRepo;
        }

        public override async Task Process(UpdateTransactionRequest request)
        {
            var transaction = await _transactionRepo.GetById(request.Id.Value);
            if (request.Date.HasValue)
            {
                transaction.Date = request.Date.Value.GetCleanDate();
            }
            transaction.OtherItemId = request.OtherItemId;
            await _transactionRepo.Update(transaction);
        }
    }
}
