using hu_app.Components.Finance;
using hu_app.Components.Finance.Balance;
using hu_app.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hu_app.Pages.Finance
{
    public class BalanceModel : HuPageModel
    {
        public BalanceModel(HuServicesBag servicesBag) : base(servicesBag) { }

        [BindProperty(SupportsGet = true)]
        public Guid? Who { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Year { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Month { get; set; }

        public List<BalanceDTO> Balance { get; set; } = new List<BalanceDTO>();

        public void OnGet()
        {
            Year = DateTime.Now.Year;
            Month = DateTime.Now.Month;
            Who = HuConstants.HuApp.User.Hu;
        }

        public async Task<IActionResult> OnGetBalances()
        {
            var response = await _mediator.Send(new GetBalancesRequest { Year = Year, Month = Month, UserId = Who });
            return HuResponse.Send(response);
        }

        public async Task<IActionResult> OnPostToggleBalance([FromBody] ToggleBalanceRequest request)
        {
            var response = await _mediator.Send(request);
            return HuResponse.Send(response);
        }

        public async Task<IActionResult> OnPostResetBalances([FromBody] ResetBalancesRequest request)
        {
            var response = await _mediator.Send(request);
            return HuResponse.Send(response);
        }
    }
}
