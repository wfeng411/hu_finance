using AutoMapper;
using hu_app.Components.Finance;
using hu_app.Components.Finance.ItemIndex;
using hu_app.Components.Finance.Merchant;
using hu_app.Components.Finance.Transaction;
using hu_app.Models.Entities.Finance;

namespace hu_app.MapperProfiles
{
    public class FinanceMapperProfile : Profile
    {
        public FinanceMapperProfile()
        {
            // Transaction
            CreateMap<FinanceTransaction, TransactionDTO>()
                .ForMember(x => x.ItemName, opt => opt.MapFrom(x => x.Item.Name))
                .ForMember(x => x.OtherItemName, opt => opt.MapFrom(x => x.OtherItem.Name))
                .ForMember(x => x.MerchantName, opt => opt.MapFrom(x => GetMerchantName(x)))
                .ForMember(x => x.Who, opt => opt.MapFrom(x => x.User.UserName))
                .ForMember(x => x.Who_CN, opt => opt.MapFrom(x => x.User.Name))
                .ForMember(x => x.Ignore, opt => opt.MapFrom(x => x.Item.Merchant.Ignore));

            CreateMap<SaveTransactionDTO, FinanceTransaction>();

            // Item
            CreateMap<FinanceItem, ItemDTO>()
                .ForMember(x => x.MerchantName, opt => opt.MapFrom(x => x.Merchant.Name));

            CreateMap<FinanceItemIndex, ItemIndexDTO>()
                .ForMember(x => x.ItemName, opt => opt.MapFrom(x => x.Name))
                .ForMember(x => x.MerchantName, opt => opt.MapFrom(x => x.Merchant.Name));

            CreateMap<SaveItemIndexRequest, FinanceItemIndex>()
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.ItemName));

            // Merchant
            CreateMap<FinanceMerchant, MerchantDTO>();

            CreateMap<SaveMerchantRequest, FinanceMerchant>()
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.MerchantName.Trim().Replace("\t", "").Replace("\n", "")))
                .ForMember(x => x.Note, opt => opt.MapFrom(x => string.IsNullOrWhiteSpace(x.Note) ? null : x.Note.Trim().Replace("\t", "").Replace("\n", "")));

            CreateMap<FinanceMerchant, MerchantDetailDTO>()
                .ForMember(x => x.MerchantName, opt => opt.MapFrom(x => x.Name));

            CreateMap<MerchantDetailDTO, MerchantDTO>()
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.MerchantName));

            CreateMap<FinanceItem, MerchantDetailItemDTO>()
                .ForMember(x => x.ItemName, opt => opt.MapFrom(x => x.Name));

            CreateMap<FinanceItemIndex, MerchantDetailItemDTO>()
                .ForMember(x => x.ItemName, opt => opt.MapFrom(x => x.Name));

            // Balance
            CreateMap<FinanceTransaction, BalanceDTO>()
                .ForMember(x => x.MerchantName, opt => opt.MapFrom(x => x.Item.Merchant.Name))
                .ForMember(x => x.Amount, opt => opt.MapFrom(x => x.Debit ?? x.Credit));
        }

        private string GetMerchantName(FinanceTransaction t)
        {
            var merchant = t.OtherItem?.Merchant ?? t.Item.Merchant;
            var merchantName = merchant?.Name;
            if (merchant?.Note != null)
            {
                merchantName += " (" + merchant.Note + ")";
            }
            return merchantName;
        }
    }
}
