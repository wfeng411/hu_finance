using hu_app.Models.Lookups.Finance;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace hu_app.Models.Entities.Finance
{
    public class FinanceMerchant : BaseEntity
    {
        public string Name { get; set; }

        public string Note { get; set; }

        public FinanceExpenseType ExpenseType { get; set; }
        public Guid ExpenseTypeId { get; set; }

        public bool Ignore { get; set; }

        public int Order { get; set; }

        public List<FinanceItem> Items { get; set; } = new List<FinanceItem>();

        public List<FinanceItemIndex> ItemIndexes { get; set; } = new List<FinanceItemIndex>();

        [NotMapped]
        public string FullName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Note))
                {
                    return Name;
                }
                return $"{Name} ({Note})";
            }
        }
    }
}
