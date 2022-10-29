using AutoMapper;
using hu_app.Models.Entities.Finance;
using hu_app.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hu_app.Components.Finance.Merchant
{
    public class GetMerchantDetailsRequest : IHuRequest { }

    public class GetMerchantDetailsHandler : HuRequestHandler<GetMerchantDetailsRequest>
    {
        private readonly HuRepository<FinanceMerchant> _merchantRepo;
        private readonly HuRepository<FinanceItem> _itemRepo;
        private readonly HuRepository<FinanceTransaction> _transactionRepo;
        private readonly IMapper _mapper;

        public GetMerchantDetailsHandler(
            HuRepository<FinanceMerchant> merchantRepo,
            HuRepository<FinanceItem> itemRepo,
            HuRepository<FinanceTransaction> transactionRepo,
            IMapper mapper)
        {
            _merchantRepo = merchantRepo;
            _itemRepo = itemRepo;
            _transactionRepo = transactionRepo;
            _mapper = mapper;
        }

        public override async Task Load(GetMerchantDetailsRequest request)
        {
            List<FinanceMerchant> merchants = await _merchantRepo.GetQueryable()
                .Include(x => x.ExpenseType)
                .Include(x => x.Items)
                .OrderBy(x => x.Ignore).ThenBy(x => x.ExpenseType.Order).ThenBy(x => x.Name)
                .ToListAsync();

            var merchantDTOs = new List<MerchantDetailDTO>();

            var firstDate = new DateTime(DateTime.Now.Year, 1, 1);
            foreach (var m in merchants)
            {
                var merchantDetailDTO = _mapper.Map<MerchantDetailDTO>(m);
                merchantDetailDTO.Items = merchantDetailDTO.Items.OrderBy(x => x.ItemName).ToList();
                merchantDetailDTO.IsOld = !await _transactionRepo.GetQueryable().AnyAsync(x => x.Item.MerchantId == m.Id && x.Date > firstDate);
                merchantDTOs.Add(merchantDetailDTO);
            }

            var newItems = await _itemRepo.GetQueryable()
                .Where(x => !x.MerchantId.HasValue)
                .ToListAsync();

            if (newItems.Any())
            {
                merchantDTOs.Add(new MerchantDetailDTO
                {
                    MerchantName = "NEW",
                    ExpenseTypeId = HuConstants.Finance.ExpenseType.DK,
                    Items = newItems.Select(x => _mapper.Map<MerchantDetailItemDTO>(x)).ToList()
                });
            }

            Data = merchantDTOs;
        }
    }
}