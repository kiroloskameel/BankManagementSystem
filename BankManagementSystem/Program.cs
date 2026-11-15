using BankManagementSystem.BankManagementSystem;
using Microsoft.EntityFrameworkCore;

namespace BankManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            using var context = new BankDbContext();
            context.Database.EnsureCreated();

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("National Bank Management");
                Console.WriteLine("======================================");
                Console.WriteLine("1) Add a new Customer");
                Console.WriteLine("2) Open a new Account for a Customer");
                Console.WriteLine("3) Update Account Status (Active / Closed)");
                Console.WriteLine("4) Remove an Account from a Customer");
                Console.WriteLine("5) List all Customers (with accounts)");
                Console.WriteLine("0) Exit");
                Console.Write("\nEnter choice: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        AddNewCustomer(context);
                        break;
                    case "2":
                        OpenNewAccount(context);
                        break;
                    case "3":
                        UpdateAccountStatus(context);
                        break;
                    case "4":
                        RemoveAccountFromCustomer(context);
                        break;
                    case "5":
                        ListAllCustomers(context);
                        break;
                    case "0":
                        exit = true;
                        Console.WriteLine("Exiting application. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 0 and 5.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nPress any key to return to the menu...");
                    Console.ReadKey();
                }
            }
        }

        // --- Option 1: Add Customer ---
        private static void AddNewCustomer(BankDbContext context)
        {
            Console.WriteLine("Add New Customer");
            Console.Write("Full Name: ");
            string name = Console.ReadLine();

            Console.Write("National ID: ");
            string nationalId = Console.ReadLine();

            Console.Write("Date of Birth (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dob))
            {
                Console.WriteLine("Invalid date format. Operation cancelled.");
                return;
            }

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Phone: ");
            string phone = Console.ReadLine();

            Console.Write("Address: ");
            string address = Console.ReadLine();

            Console.WriteLine("Customer Type:\n1) Individual\n2) Business");
            Console.Write("Choice: ");
            if (!int.TryParse(Console.ReadLine(), out int typeChoice) || (typeChoice != 1 && typeChoice != 2))
            {
                Console.WriteLine("Invalid choice. Operation cancelled.");
                return;
            }

            var customer = new Customer
            {
                FullName = name,
                NationalId = nationalId,
                DateOfBirth = dob,
                Email = email,
                PhoneNumber = phone,
                Address = address,
                CustomerType = (CustomerType)typeChoice
            };

            context.Customers.Add(customer);
            context.SaveChanges();
            Console.WriteLine($"\nCustomer created successfully. CustomerId = {customer.Id}");
        }

        // --- Option 2: Open Account ---
        private static void OpenNewAccount(BankDbContext context)
        {
            Console.WriteLine("Open New Account");
            Console.Write("Account Number: ");
            string accNumber = Console.ReadLine();

            if (context.Accounts.Any(a => a.AccountNumber == accNumber))
            {
                Console.WriteLine("Account number already exists!");
                return;
            }

            Console.WriteLine("Account Type:\n1) Savings\n2) Current\n3) Business");
            Console.Write("Choice: ");
            if (!int.TryParse(Console.ReadLine(), out int typeChoice)) return;

            Console.Write("Branch Code: ");
            string branchCode = Console.ReadLine();

            Console.Write("Customer Id: ");
            if (!int.TryParse(Console.ReadLine(), out int customerId)) return;

            Console.WriteLine("Ownership Role:\n1) Primary\n2) CoHolder");
            Console.Write("Choice: ");
            if (!int.TryParse(Console.ReadLine(), out int roleChoice)) return;

            Console.WriteLine($"\nValidating branch '{branchCode}' and customer #{customerId}...");

            var branch = context.Branches.Find(branchCode);
            var customer = context.Customers.Find(customerId);

            if (branch == null || customer == null)
            {
                Console.WriteLine("Validation failed: Branch or Customer does not exist.");
                return;
            }

            // Check if account already exists globally, if not create it
            var account = context.Accounts.Find(accNumber);
            if (account == null)
            {
                account = new Account
                {
                    AccountNumber = accNumber,
                    AccountType = (AccountType)typeChoice,
                    CurrentBalance = 0,
                    OpeningDate = DateTime.Now,
                    BranchCode = branchCode
                };
                context.Accounts.Add(account);
            }

            // Create link in join table
            var customerAccount = new CustomerAccount
            {
                CustomerId = customerId,
                AccountNumber = accNumber,
                OwnershipType = (OwnershipRole)roleChoice,
                OwnershipStartDate = DateTime.Now,
                AccountStatus = AccountStatus.Active
            };

            context.CustomerAccounts.Add(customerAccount);
            context.SaveChanges();

            Console.WriteLine($"Account '{accNumber}' created and linked to customer {customerId} as {(OwnershipRole)roleChoice} owner.");
        }

        // --- Option 3: Update Account Status ---
        private static void UpdateAccountStatus(BankDbContext context)
        {
            Console.WriteLine("Update Account Status");
            Console.Write("Account Number: ");
            string accNumber = Console.ReadLine();

            Console.Write("Customer Id: ");
            if (!int.TryParse(Console.ReadLine(), out int customerId)) return;

            var link = context.CustomerAccounts
                .FirstOrDefault(ca => ca.AccountNumber == accNumber && ca.CustomerId == customerId);

            if (link == null)
            {
                Console.WriteLine("Account link not found for this customer.");
                return;
            }

            Console.WriteLine("New Status:\n1) Active\n2) Closed");
            Console.Write("Choice: ");
            if (!int.TryParse(Console.ReadLine(), out int statusChoice)) return;

            link.AccountStatus = (AccountStatus)statusChoice;
            context.SaveChanges();

            Console.WriteLine($"\nStatus updated to {link.AccountStatus}.");
        }

        // --- Option 4: Remove Account from Customer ---
        private static void RemoveAccountFromCustomer(BankDbContext context)
        {
            Console.WriteLine("Remove Account From Customer");
            Console.Write("Account Number: ");
            string accNumber = Console.ReadLine();

            Console.Write("Customer Id: ");
            if (!int.TryParse(Console.ReadLine(), out int customerId)) return;

            var link = context.CustomerAccounts
                .FirstOrDefault(ca => ca.AccountNumber == accNumber && ca.CustomerId == customerId);

            if (link == null)
            {
                Console.WriteLine("Account link not found.");
                return;
            }

            context.CustomerAccounts.Remove(link);
            context.SaveChanges();
            Console.WriteLine("Ownership link deleted.");

            // Check if account has any other owners left
            bool hasOtherOwners = context.CustomerAccounts.Any(ca => ca.AccountNumber == accNumber);
            if (!hasOtherOwners)
            {
                var account = context.Accounts.Find(accNumber);
                if (account != null)
                {
                    context.Accounts.Remove(account);
                    context.SaveChanges();
                    Console.WriteLine($"That was the last owner, account '{accNumber}' was also removed.");
                }
            }
        }

        // --- Option 5: List all Customers with accounts ---
        private static void ListAllCustomers(BankDbContext context)
        {
            Console.WriteLine("All Customers\n");

            var customers = context.Customers
                .Include(c => c.CustomerAccounts)
                    .ThenInclude(ca => ca.Account)
                        .ThenInclude(a => a.Branch)
                .ToList();

            foreach (var c in customers)
            {
                Console.WriteLine($"#{c.Id} {c.FullName} ({c.CustomerType})");

                if (!c.CustomerAccounts.Any())
                {
                    Console.WriteLine("  (no accounts)");
                }
                else
                {
                    foreach (var ca in c.CustomerAccounts)
                    {
                        var acc = ca.Account;
                        string branchName = acc.Branch?.Name ?? "Unknown Branch";
                        Console.WriteLine($"  {acc.AccountNumber} Current Balance: {acc.CurrentBalance:N2} {ca.OwnershipType} {ca.AccountStatus} @{branchName}");
                    }
                }
            }
        }
    }
}
