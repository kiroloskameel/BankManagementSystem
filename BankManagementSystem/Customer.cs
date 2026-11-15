using BankManagementSystem;
using BankManagementSystem.BankManagementSystem;
using System;
using System.Collections.Generic;
using System.Text;


namespace BankManagementSystem
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string NationalId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public CustomerType CustomerType { get; set; }

        // Navigation through Join Entity
        public ICollection<CustomerAccount> CustomerAccounts { get; set; } = new List<CustomerAccount>();
    }
}
