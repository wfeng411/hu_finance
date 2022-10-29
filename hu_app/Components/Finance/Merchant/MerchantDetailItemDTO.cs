using System;

namespace hu_app.Components.Finance.Merchant
{
    public class MerchantDetailItemDTO : BaseDTO
    {
        public string ItemName { get; set; }
        public Guid? MerchantId { get; set; }
        public bool IsOther { get; set; }
    }
}
