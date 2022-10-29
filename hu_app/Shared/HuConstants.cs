using System;

namespace hu_app.Shared
{
    public static class HuConstants
    {
        public class HuApp
        {
            public class User
            {
                public static Guid Hu = Guid.Parse("f5fc4bd4-055b-42a2-0c7e-08d8ddbb0999");
                public static Guid Tu = Guid.Parse("64883f11-7a8f-4381-0c7f-08d8ddbb0999");
            }
        }

        public class Finance
        {
            public class TransactionType
            {
                public static Guid ChequingAccount = Guid.Parse("04bf43c2-88ed-455f-9da2-4928727d44a1");
                public static Guid CreditCard = Guid.Parse("dbf5d467-ec7d-4f16-a6a6-cc043be2efac");
            }

            public class TransactionResult
            {
                public static string New = "New";
                public static string Exist = "Exist";
                public static string Error = "Error";
            }

            public class ExpenseType
            {
                public static Guid Shopping = Guid.Parse("77c34529-e858-4045-90d0-a710df28d691");
                public static Guid Food = Guid.Parse("6e1e65b8-4996-4c4c-9f1a-7be175f08032");
                public static Guid Essential = Guid.Parse("013c4beb-40f4-4439-a442-5c3c6b5f173e");
                public static Guid Communication = Guid.Parse("63492dcb-f117-4d77-bfea-9b29ba61fb89");
                public static Guid Bank = Guid.Parse("85f69835-4fe8-4200-abad-ff801bdc9bd3");
                public static Guid Income = Guid.Parse("5c7a93db-9cdd-4723-9fcf-c6f354144f50");
                public static Guid Other = Guid.Parse("c2ee31b7-0e48-4886-9c08-7ed55c81d7d4");
                public static Guid DK = Guid.Parse("4a599a4f-1fe2-4a5d-8a90-bd29be609519");
            }
        }

        public class LogSource
        {
            public static string Info = "Info";
            public static string Exception = "Exception";
            public static string Mediator = "Mediator";
            public static string MediatorValidator = "Mediator.Validator";
        }
    }
}
