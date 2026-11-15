using System;
using System.Collections.Generic;
using System.Text;

namespace BankManagementSystem
{
    public class Branch
    {
        public string Code { get; set; } // Primary Key (String from diagram)
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        // One-to-One with Manager
        public Manager Manager { get; set; }

        // One-to-Many with Accounts
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
