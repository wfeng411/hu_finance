using System;

namespace hu_app.Components.Finance
{
    public class ItemDTO : BaseDTO
    {
        public string Name { get; set; }
        public string MerchantName { get; set; }
        public Guid? MerchantId { get; set; }
    }
}
