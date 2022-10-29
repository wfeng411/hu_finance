using hu_app.Components.Finance;
using hu_app.Components.Finance.Merchant;
using hu_app.Models.Lookups.Finance;
using hu_app.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hu_app.Pages.Finance
{
    public class MerchantsModel : HuPageModel
    {
        public MerchantsModel(HuServicesBag servicesBag) : base(servicesBag) { }

        [BindProperty(SupportsGet = true)]
        public Guid ItemId { get; set; }

        public IEnumerable<SelectListItem> ExpenseTypeOptions { get; set; }
        public Dictionary<Guid, string> ExpenseTypesDict { get; set; } = new Dictionary<Guid, string>();
        public List<MerchantDetailDTO> MerchantDetailDTOs { get; set; }
        public List<MerchantDTO> MerchantDTOs { get; set; }

        public async Task OnGet()
        {
            var expenseTypes = await GetData<IEnumerable<FinanceExpenseType>>(new GetExpenseTypesRequest());
            ExpenseTypeOptions = expenseTypes.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ExpenseTypesDict = expenseTypes.ToDictionary(x => x.Id, x => x.Name);
            MerchantDetailDTOs = await GetData<List<MerchantDetailDTO>>(new GetMerchantDetailsRequest());
            MerchantDTOs = MerchantDetailDTOs.Select(x => _mapper.Map<MerchantDTO>(x)).OrderBy(x => x.Name).ToList();
        }

        public async Task<IActionResult> OnPostSaveMerchant([FromBody] SaveMerchantRequest request)
        {
            var response = await _mediator.Send(request);
            return HuResponse.Send(response);
        }

        public async Task<IActionResult> OnPostDeleteMerchant([FromBody] DeleteMerchantRequest request)
        {
            var response = await _mediator.Send(request);
            return HuResponse.Send(response);
        }

        public async Task<IActionResult> OnPostSaveItem([FromBody] UpdateItemMerchantIdRequest request)
        {
            var response = await _mediator.Send(request);
            return HuResponse.Send(response);
        }

        public async Task<IActionResult> OnGetTransactions()
        {
            var response = await _mediator.Send(new GetTransactionsByItemIdRequest { ItemId = ItemId });
            return HuResponse.Send(response);
        }

        public async Task<IActionResult> OnGetMerchantFrequency(Guid id, int year)
        {
            var response = await _mediator.Send(new GetMerchantFrequencyRequest { MerchantId = id, Year = year });
            return HuResponse.Send(response);
        }
    }
}
