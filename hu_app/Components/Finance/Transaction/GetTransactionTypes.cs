using hu_app.Models.Lookups.Finance;
using hu_app.Shared;

namespace hu_app.Components.Finance.Transaction
{
    public class GetTransactionTypesRequest : IHuRequest { }

    public class GetTransactionTypesHandler : HuRequestHandler<GetTransactionTypesRequest, FinanceTransactionType>
    {
        public GetTransactionTypesHandler(HuRepository<FinanceTransactionType> repo) : base(repo)
        {
        }
    }
}
