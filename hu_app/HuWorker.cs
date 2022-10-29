using AutoMapper;
using hu_app.Hubs;
using hu_app.Models.Entities.Finance;
using hu_app.Shared;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hu_app
{
    public class HuWorker : BackgroundService
    {
        private readonly IHubContext<DashboardHub> _dashboardHub;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;

        public HuWorker(IHubContext<DashboardHub> dashboardHub,
            IServiceProvider serviceProvider,
            IMapper mapper)
        {
            _dashboardHub = dashboardHub;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var data = await LoadData();
                await _dashboardHub.Clients.All.SendAsync("UpdateData", data);
                await Task.Delay(3000);
            }
        }

        public async Task<object> LoadData()
        {
            var finance = await LoadFinanceData();

            return new { finance };
        }

        private async Task<object> LoadFinanceData()
        {
            var date = DateTime.Now;
            var start = new DateTime(date.Year, date.Month, 1);
            var end = new DateTime(date.Year, date.Month + 1, 1);

            using var scope = _serviceProvider.CreateScope();

            var _financeTransactionRepo = scope.ServiceProvider.GetRequiredService<HuRepository<FinanceTransaction>>();

            var transactions = await _financeTransactionRepo.GetQueryable()
                .Include(x => x.Item)
                .Include(x => x.User)
                .Where(x => x.Date >= start && x.Date < end
                            && x.TransactionTypeId == HuConstants.Finance.TransactionType.ChequingAccount
                            && !x.Item.Merchant.Ignore)
                .OrderByDescending(x => x.Date)
                .ToListAsync();

            var expense = transactions.Sum(x => x.Debit);
            var income = transactions.Sum(x => x.Credit);

            var finance = new
            {
                summary = new { expense, income },
                transactions = transactions.Select(x => new
                {
                    x.Date,
                    ItemName = x.Item.Name,
                    Amount = x.Debit ?? x.Credit,
                    IsCredit = x.Debit == null,
                    Who = x.User.Name
                })
            };

            return finance;
        }
    }
}
