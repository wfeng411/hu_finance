using System.Collections.Generic;

namespace hu_app.Components.Finance.Balance
{
    public class BalanceDetailsDTO
    {
        public List<TransactionDTO> Transactions { get; set; }
        public List<BalanceDTO> Credits { get; set; }
        public List<BalanceDTO> Debits { get; set; }
    }
}
