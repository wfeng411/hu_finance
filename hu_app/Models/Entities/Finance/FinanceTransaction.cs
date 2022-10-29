using hu_app.Models.Lookups.Finance;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace hu_app.Models.Entities.Finance
{
    public class FinanceTransaction : BaseEntity
    {
        public DateTime Date { get; set; }

        public DateTime OriginalDate { get; set; }

        public Guid ItemId { get; set; }
        public FinanceItem Item { get; set; }

        public Guid? OtherItemId { get; set; }
        public FinanceItem OtherItem { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal? Debit { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal? Credit { get; set; }

        public Guid TransactionTypeId { get; set; }
        public FinanceTransactionType TransactionType { get; set; }

        public Guid UserId { get; set; }
        public HuAppUser User { get; set; }

        public bool InBalance { get; set; }
    }
}
