using AutoMapper;
using hu_app.Models.Entities.Finance;
using hu_app.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace hu_app.Components.Finance.ItemIndex
{
    public class GetItemIndexesRequest : IHuRequest
    {
    }

    public class GetItemIndexesHandler : HuRequestHandler<GetItemIndexesRequest>
    {
        private readonly HuRepository<FinanceItemIndex> _repo;
        private readonly IMapper _mapper;

        public GetItemIndexesHandler(HuRepository<FinanceItemIndex> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public override async Task Load(GetItemIndexesRequest request)
        {
            var itemIndexes = await _repo.GetQueryable()
                .OrderBy(x => x.Merchant.Name).ThenBy(x => x.Name)
                .ToListAsync();

            Data = itemIndexes.Select(x => _mapper.Map<ItemIndexDTO>(x)).ToList();
        }
    }
}
