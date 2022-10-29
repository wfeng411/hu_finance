using AutoMapper;
using hu_app.Models.Entities.Finance;
using hu_app.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace hu_app.Components.Finance.Transaction
{
    public class GetItemsRequest : IHuRequest
    {
    }

    public class GetItemsHandler : HuRequestHandler<GetItemsRequest>
    {
        private readonly HuRepository<FinanceItem> _repo;
        private readonly IMapper _mapper;

        public GetItemsHandler(HuRepository<FinanceItem> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public override async Task Load(GetItemsRequest request)
        {
            var items = await _repo.GetQueryable()
                .OrderBy(x => x.Name)
                .ToListAsync();

            Data = items.Select(x => _mapper.Map<ItemDTO>(x)).ToList();
        }
    }
}
