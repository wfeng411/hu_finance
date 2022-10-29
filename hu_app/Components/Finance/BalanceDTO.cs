using System;

namespace hu_app.Components.Finance
{
    public class BalanceDTO
    {
        public Guid MerchantId { get; set; }
        public string MerchantName { get; set; }
        public decimal Amount { get; set; }
    }
}
