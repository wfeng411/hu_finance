using hu_app.Components.Finance;
using hu_app.Components.Finance.Transaction;
using hu_app.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hu_app.Pages.Finance
{
    public class TransactionsModel : HuPageModel
    {
        public TransactionsModel(HuServicesBag servicesBag) : base(servicesBag) { }

        [BindProperty(SupportsGet = true)]
        public Guid? Type { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid? Who { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Year { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Month { get; set; }

        public IEnumerable<TransactionDTO> Transactions { get; set; }
        public IEnumerable<ItemDTO> Items { get; set; }

        public async Task OnGet()
        {
            var filter = GetUserPreference().TransactionsFilters;
            if (filter != null)
            {
                Year = filter.Year;
                Month = filter.Month;
                Type = filter.TransactionType;
                Who = filter.Who;
            }

            if (Year == null)
            {
                Year = DateTime.Now.Year;
                Month = DateTime.Now.Month;
            }

            Items = await GetData<IEnumerable<ItemDTO>>(new GetItemsRequest());
        }

        public async Task<IActionResult> OnGetTransactions()
        {
            var preference = GetUserPreference();
            preference.TransactionsFilters = new HuFilterOptions
            {
                TransactionType = Type,
                Year = Year,
                Month = Month,
                Who = Who
            };
            SetUserPreference(preference);

            var response = await _mediator.Send(new GetTransactionsRequest { TransactionTypeId = Type, Year = Year, Month = Month, UserId = Who });
            return HuResponse.Send(response);
        }

        public async Task<IActionResult> OnPostUpdateTransaction([FromBody] UpdateTransactionRequest request)
        {
            var response = await _mediator.Send(request);
            return HuResponse.Send(response);
        }
    }
}
