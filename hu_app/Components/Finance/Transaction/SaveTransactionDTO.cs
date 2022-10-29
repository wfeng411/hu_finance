using System;

namespace hu_app.Components.Finance.Transaction
{
    public class SaveTransactionDTO
    {
        public DateTime? Date { get; set; }
        public string ItemName { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public Guid? TransactionTypeId { get; set; }
        public Guid? UserId { get; set; }
    }
}
