using System;

namespace hu_app.Shared
{
    public class HuFilterOptions
    {
        public Guid? TransactionType { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public Guid? Who { get; set; }
    }
}
