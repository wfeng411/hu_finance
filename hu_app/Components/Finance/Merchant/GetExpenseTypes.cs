using hu_app.Models.Lookups.Finance;
using hu_app.Shared;

namespace hu_app.Components.Finance.Merchant
{
    public class GetExpenseTypesRequest : IHuRequest { }

    public class GetExpenseTypesHandler : HuRequestHandler<GetExpenseTypesRequest, FinanceExpenseType>
    {
        public GetExpenseTypesHandler(HuRepository<FinanceExpenseType> repo) : base(repo)
        {
        }
    }
}
