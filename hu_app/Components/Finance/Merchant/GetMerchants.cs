using AutoMapper;
using hu_app.Models.Entities.Finance;
using hu_app.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace hu_app.Components.Finance.Merchant
{
    public class GetMerchantsRequest : IHuRequest { }

    public class GetMerchantsHandler : HuRequestHandler<GetMerchantsRequest>
    {
        private readonly HuRepository<FinanceMerchant> _repo;
        private readonly IMapper _mapper;

        public GetMerchantsHandler(HuRepository<FinanceMerchant> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public override async Task Load(GetMerchantsRequest request)
        {
            Data = await _repo.GetQueryable()
                .OrderBy(x => x.Name)
                .Select(x => _mapper.Map<MerchantDTO>(x))
                .ToListAsync();
        }
    }
}
