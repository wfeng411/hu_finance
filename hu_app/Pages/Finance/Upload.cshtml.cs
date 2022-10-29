using hu_app.Components.Finance.Merchant;
using hu_app.Components.Finance.Transaction;
using hu_app.Components.User;
using hu_app.Models;
using hu_app.Models.Lookups.Finance;
using hu_app.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hu_app.Pages.Finance
{
    public class UploadModel : HuPageModel
    {
        public UploadModel(HuServicesBag servicesBag) : base(servicesBag) { }

        public List<SelectListItem> Users { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> TransactionTypes { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ExpenseTypeOptions { get; set; }

        public async Task OnGet()
        {
            var usersResponse = await _mediator.Send(new GetUsersRequest());
            if (usersResponse.Ok)
            {
                Users = (usersResponse.Data as List<HuAppUser>)
                    .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                    .ToList();
            }

            var transactionTypesResponse = await _mediator.Send(new GetTransactionTypesRequest());
            if (transactionTypesResponse.Ok)
            {
                TransactionTypes = (transactionTypesResponse.Data as List<FinanceTransactionType>)
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
                    .ToList();
            }

            var expenseTypes = await GetData<IEnumerable<FinanceExpenseType>>(new GetExpenseTypesRequest());
            ExpenseTypeOptions = expenseTypes.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
        }

        public async Task<IActionResult> OnPostCheckTransaction([FromBody] CheckTransactionRequest request)
        {
            HuResponse response = await _mediator.Send(request);
            return HuResponse.Send(response);
        }

        public async Task<IActionResult> OnPostSaveTransactions([FromBody] SaveTransactionsRequest request)
        {
            HuResponse response = await _mediator.Send(request);
            return HuResponse.Send(response);
        }
    }
}
