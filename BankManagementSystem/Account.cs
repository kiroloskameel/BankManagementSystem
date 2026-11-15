using BankManagementSystem;
using BankManagementSystem.BankManagementSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankManagementSystem
{
    public class Account
    {
        public string AccountNumber { get; set; } // Primary Key (String)
        public decimal CurrentBalance { get; set; }
        public AccountType AccountType { get; set; }
        public DateTime OpeningDate { get; set; }

        // Foreign Key to Branch
        public string BranchCode { get; set; }
        public Branch Branch { get; set; }

        // Navigation through Join Entity
        public ICollection<CustomerAccount> CustomerAccounts { get; set; } = new List<CustomerAccount>();

        // One-to-Many with Transactions
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
