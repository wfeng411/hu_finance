using System;

namespace hu_app.Components.Finance
{
    public class ItemIndexDTO : BaseDTO
    {
        public string ItemName { get; set; }
        public string MerchantName { get; set; }
        public Guid? MerchantId { get; set; }
    }
}
