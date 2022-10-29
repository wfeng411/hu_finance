using hu_app.Models.Entities.Finance;
using hu_app.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace hu_app.Components.Finance.Merchant
{
    public class GetMerchantFrequencyRequest : IHuRequest
    {
        public Guid MerchantId { get; set; }
        public int Year { get; set; }
    }

    public class GetMerchantFrequencyHandler : HuRequestHandler<GetMerchantFrequencyRequest>
    {
        private readonly HuRepository<FinanceTransaction> _repo;

        public GetMerchantFrequencyHandler(HuRepository<FinanceTransaction> repo)
        {
            _repo = repo;
        }

        public override async Task Load(GetMerchantFrequencyRequest request)
        {
            var c = await _repo.GetQueryable()
                .Where(x => ((x.OtherItemId.HasValue && x.OtherItem.MerchantId == request.MerchantId)
                                || x.Item.MerchantId == request.MerchantId)
                            && x.Date.Year == request.Year)
                .CountAsync();

            Data = c;
        }
    }
}
