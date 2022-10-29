using hu_app.Components.Finance;
using hu_app.Components.Finance.Income;
using hu_app.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hu_app.Pages.Finance
{
    public class IncomeModel : HuPageModel
    {
        public IncomeModel(HuServicesBag servicesBag) : base(servicesBag) { }

        [BindProperty(SupportsGet = true)]
        public int? Year { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Month { get; set; }

        public List<TransactionDTO> TransactionDTOs { get; set; }

        public async Task OnGet()
        {
            if (!Year.HasValue)
            {
                var today = DateTime.Now;
                Year = today.Year;
                Month = today.Month;
            }
            var response = await _mediator.Send(new GetIncomesRequest(Year, Month));
            if (response.Ok)
            {
                TransactionDTOs = response.Data as List<TransactionDTO>;
            }
        }
    }
}
