using System;
using System.Collections.Generic;

namespace hu_app.Models.Entities.Finance
{
    public class FinanceItem : BaseEntity
    {
        public string Name { get; set; }

        public Guid? MerchantId { get; set; }
        public FinanceMerchant Merchant { get; set; }

        public List<FinanceTransaction> Transactions { get; set; } = new List<FinanceTransaction>();
    }
}
