using AutoMapper;
using hu_app.Models.Entities.Finance;
using hu_app.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace hu_app.Components.Finance.Merchant
{
    public class GetTransactionsByItemIdRequest : IHuRequest
    {
        public Guid ItemId { get; set; }
    }

    public class GetTransactionsByItemIdHandler : HuRequestHandler<GetTransactionsByItemIdRequest>
    {
        private readonly HuRepository<FinanceTransaction> _repo;
        private readonly IMapper _mapper;

        public GetTransactionsByItemIdHandler(HuRepository<FinanceTransaction> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public override async Task Load(GetTransactionsByItemIdRequest request)
        {
            Data = await _repo.GetQueryable()
                .Include(x => x.Item).ThenInclude(x => x.Merchant)
                .Include(x => x.User)
                .Where(x => x.ItemId == request.ItemId)
                .OrderByDescending(x => x.Date)
                .Select(x => _mapper.Map<TransactionDTO>(x))
                .ToListAsync();
        }
    }
}
