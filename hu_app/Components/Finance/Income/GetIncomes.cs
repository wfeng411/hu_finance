using AutoMapper;
using hu_app.Models.Entities.Finance;
using hu_app.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace hu_app.Components.Finance.Income
{
    public class GetIncomesRequest : IHuRequest
    {
        public int? Year { get; set; }
        public int? Month { get; set; }

        public GetIncomesRequest(int? year = null, int? month = null)
        {
            Year = year;
            Month = month;
        }
    }

    public class GetIncomesHandler : HuRequestHandler<GetIncomesRequest>
    {
        private readonly HuRepository<FinanceTransaction> _repo;
        private readonly IMapper _mapper;

        public GetIncomesHandler(HuRepository<FinanceTransaction> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public override async Task Load(GetIncomesRequest request)
        {
            var transactions = await _repo.GetQueryable()
                .Include(x => x.Item).ThenInclude(x => x.Merchant)
                .Include(x => x.OtherItem).ThenInclude(x => x.Merchant)
                .Include(x => x.User)
                .Where(x => (x.Item.Merchant.ExpenseTypeId == HuConstants.Finance.ExpenseType.Income
                                || x.OtherItem.Merchant.ExpenseTypeId == HuConstants.Finance.ExpenseType.Income)
                            && (!request.Year.HasValue
                                || (x.Date.Year == request.Year.Value
                                    && (!request.Month.HasValue || x.Date.Month == request.Month.Value))))
                .OrderBy(x => x.User.UserName).ThenBy(x => x.Item.Merchant.Name).ThenByDescending(x => x.Date)
                .ToListAsync();
            Data = transactions.Select(x => _mapper.Map<TransactionDTO>(x)).ToList();
        }
    }
}
