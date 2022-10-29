using hu_app.Components.Finance;
using hu_app.Components.Finance.ItemIndex;
using hu_app.Components.Finance.Merchant;
using hu_app.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hu_app.Pages.Finance
{
    public class ItemIndexModel : HuPageModel
    {
        public ItemIndexModel(HuServicesBag servicesBag) : base(servicesBag) { }

        public List<MerchantDTO> Merchants { get; set; } = new List<MerchantDTO>();
        public List<ItemIndexDTO> ItemIndexes { get; set; } = new List<ItemIndexDTO>();

        public async Task OnGet()
        {
            Merchants = await GetData<List<MerchantDTO>>(new GetMerchantsRequest());
            ItemIndexes = await GetData<List<ItemIndexDTO>>(new GetItemIndexesRequest());
        }

        public async Task<IActionResult> OnPostSaveItemIndex([FromBody] SaveItemIndexRequest request)
        {
            var response = await _mediator.Send(request);
            return HuResponse.Send(response);
        }

        public async Task<IActionResult> OnPostDeleteItemIndex([FromBody] DeleteItemIndexRequest request)
        {
            var response = await _mediator.Send(request);
            return HuResponse.Send(response);
        }
    }
}
