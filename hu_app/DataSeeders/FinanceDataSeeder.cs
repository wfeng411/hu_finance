using hu_app.Models.Entities.Finance;
using hu_app.Models.Lookups.Finance;
using hu_app.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace hu_app.DataSeeders
{
    public class ExpenseTypeDataSeeder : IEntityTypeConfiguration<FinanceExpenseType>
    {
        public void Configure(EntityTypeBuilder<FinanceExpenseType> builder)
        {
            builder.HasData(new List<FinanceExpenseType> {
                new FinanceExpenseType { Order = 1, Id = HuConstants.Finance.ExpenseType.Essential, Name = "Essential" },
                new FinanceExpenseType { Order = 2, Id = HuConstants.Finance.ExpenseType.Communication, Name = "Communication" },
                new FinanceExpenseType { Order = 3, Id = HuConstants.Finance.ExpenseType.Food, Name = "Food" },
                new FinanceExpenseType { Order = 4, Id = HuConstants.Finance.ExpenseType.Shopping, Name = "Shopping" },
                new FinanceExpenseType { Order = 5, Id = HuConstants.Finance.ExpenseType.Bank, Name = "Bank" },
                new FinanceExpenseType { Order = 6, Id = HuConstants.Finance.ExpenseType.Other, Name = "Other" },
                new FinanceExpenseType { Order = 7, Id = HuConstants.Finance.ExpenseType.Income, Name = "Income" },
                new FinanceExpenseType { Order = 8, Id = HuConstants.Finance.ExpenseType.DK, Name = "I Don't Know" },
            });
        }
    }

    public class TransactionTypeDataSeeder : IEntityTypeConfiguration<FinanceTransactionType>
    {
        public void Configure(EntityTypeBuilder<FinanceTransactionType> builder)
        {
            builder.HasData(new List<FinanceTransactionType> {
                new FinanceTransactionType { Id = HuConstants.Finance.TransactionType.ChequingAccount, Name = "Chequing Account" },
                new FinanceTransactionType { Id = HuConstants.Finance.TransactionType.CreditCard, Name = "Credit Card" },
            });
        }
    }

    public class FinanceItemDataSeeder : IEntityTypeConfiguration<FinanceItem>
    {
        public void Configure(EntityTypeBuilder<FinanceItem> builder)
        {
            builder.HasData(new List<FinanceItem> {
                new FinanceItem { Id = Guid.Parse("7103b9b9-18a7-4e31-b278-a35b96751f4e"), Name = "Rent" }
            });
        }
    }
}
