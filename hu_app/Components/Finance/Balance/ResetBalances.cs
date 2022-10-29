using FluentValidation;
using hu_app.Models.Entities.Finance;
using hu_app.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace hu_app.Components.Finance.Balance
{
    public class ResetBalancesRequest : IHuRequest
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public Guid? UserId { get; set; }
    }

    public class ResetBalancesValidatior : AbstractValidator<ResetBalancesRequest>
    {
        public ResetBalancesValidatior()
        {
            RuleFor(x => x.Year).NotEmpty().GreaterThanOrEqualTo(2020).LessThanOrEqualTo(2099);
            RuleFor(x => x.Month).NotEmpty().GreaterThanOrEqualTo(1).LessThanOrEqualTo(12);
            RuleFor(x => x.UserId).NotEmpty();
        }
    }

    public class ResetBalancesHandler : HuRequestHandler<ResetBalancesRequest>
    {
        private readonly HuRepository<FinanceTransaction> _transactionRepo;

        public ResetBalancesHandler(HuRepository<FinanceTransaction> transactionRepo)
        {
            _transactionRepo = transactionRepo;
        }

        public override async Task Process(ResetBalancesRequest request)
        {
            var startDate = new DateTime(request.Year.Value, request.Month.Value, 1);
            var endDate = new DateTime(request.Year.Value, request.Month.Value, DateTime.DaysInMonth(request.Year.Value, request.Month.Value));

            var transactions = await _transactionRepo.GetQueryable()
                .Include(x => x.Item.Merchant)
                .Where(x => x.UserId == request.UserId.Value
                         && x.Date.Year == request.Year.Value
                         && x.Date.Month == request.Month.Value)
                .ToListAsync();

            foreach (var t in transactions)
            {
                t.InBalance = !(t.Item.Merchant?.Ignore ?? false);
                await _transactionRepo.Update(t);
            }
        }
    }
}
