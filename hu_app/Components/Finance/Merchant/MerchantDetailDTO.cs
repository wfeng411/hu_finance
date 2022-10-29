using System;
using System.Collections.Generic;

namespace hu_app.Components.Finance.Merchant
{
    public class MerchantDetailDTO : BaseDTO
    {
        public string MerchantName { get; set; }
        public string Note { get; set; }
        public Guid ExpenseTypeId { get; set; }
        public bool Ignore { get; set; }
        public bool IsOld { get; set; }
        public List<MerchantDetailItemDTO> Items { get; set; } = new List<MerchantDetailItemDTO>();
    }
}
