using System;
using System.Collections.Generic;
using System.Text;

namespace BankManagementSystem
{
    namespace BankManagementSystem
    {
        public class CustomerAccount
        {
            public int CustomerId { get; set; }
            public Customer Customer { get; set; }

            public string AccountNumber { get; set; }
            public Account Account { get; set; }

            public OwnershipRole OwnershipType { get; set; }
            public DateTime OwnershipStartDate { get; set; }
            public AccountStatus AccountStatus { get; set; }
        }
    }
}
