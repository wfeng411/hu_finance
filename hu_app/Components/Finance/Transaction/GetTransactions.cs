using AutoMapper;
using hu_app.Models.Entities.Finance;
using hu_app.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace hu_app.Components.Finance.Transaction
{
    public class GetTransactionsRequest : IHuRequest
    {
        public Guid? TransactionTypeId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public Guid? UserId { get; set; }
    }

    public class GetTransactionsHandler : HuRequestHandler<GetTransactionsRequest>
    {
        private readonly HuRepository<FinanceTransaction> _repo;
        private readonly IMapper _mapper;

        public GetTransactionsHandler(HuRepository<FinanceTransaction> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public override async Task Load(GetTransactionsRequest request)
        {
            Data = await _repo.GetQueryable()
                .Include(x => x.Item.Merchant)
                .Include(x => x.OtherItem.Merchant)
                .Include(x => x.User)
                .Where(x => (!request.TransactionTypeId.HasValue || x.TransactionTypeId == request.TransactionTypeId.Value)
                         && (!request.UserId.HasValue || x.UserId == request.UserId.Value)
                         && (!request.Year.HasValue || (x.Date.Year == request.Year.Value && (!request.Month.HasValue || x.Date.Month == request.Month.Value))))
                .OrderByDescending(x => x.Date).ThenBy(x => x.UserId).ThenBy(x => x.Item.Merchant.Name).ThenBy(x => x.Item.Name)
                .Select(x => _mapper.Map<TransactionDTO>(x))
                .ToListAsync();
        }
    }
}
