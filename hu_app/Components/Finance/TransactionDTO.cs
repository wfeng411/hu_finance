using System;

namespace hu_app.Components.Finance
{
    public class TransactionDTO : BaseDTO
    {
        public DateTime Date { get; set; }
        public DateTime OriginalDate { get; set; }
        public string ItemName { get; set; }
        public string OtherItemId { get; set; }
        public string OtherItemName { get; set; }
        public string MerchantName { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public Guid TransactionTypeId { get; set; }
        public string Who { get; set; }
        public string Who_CN { get; set; }
        public bool Ignore { get; set; }
        public bool InBalance { get; set; }
        public bool InBalanceChanged { get; set; }
    }
}
