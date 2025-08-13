using System;
using System.Collections.Generic;

namespace FinanceManagementSystem
{
    public readonly record struct Transaction(int Id, DateTime Date, decimal Amount, string Category);

    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }

    public sealed class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[BankTransfer] Processed {transaction.Amount:C} for '{transaction.Category}' on {transaction.Date:d}.");
        }
    }

    public sealed class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[MobileMoney] Sent {transaction.Amount:C} towards '{transaction.Category}' on {transaction.Date:g}.");
        }
    }

    public sealed class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[CryptoWallet] Broadcast payment {transaction.Amount:C} for '{transaction.Category}' (tx simulated).");
        }
    }

    public class Account
    {
        public string AccountNumber { get; }
        public decimal Balance { get; protected set; }

        public Account(string accountNumber, decimal initialBalance)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("Account number is required.", nameof(accountNumber));
            if (initialBalance < 0)
                throw new ArgumentOutOfRangeException(nameof(initialBalance), "Initial balance cannot be negative.");

            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public virtual void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount <= 0)
            {
                Console.WriteLine("Transaction amount must be positive.");
                return;
            }

            Balance -= transaction.Amount;
            Console.WriteLine($"[Account {AccountNumber}] Applied '{transaction.Category}' of {transaction.Amount:C}. New balance: {Balance:C}");
        }
    }

    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance) : base(accountNumber, initialBalance) { }

        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount <= 0)
            {
                Console.WriteLine("Transaction amount must be positive.");
                return;
            }

            if (transaction.Amount > Balance)
            {
                Console.WriteLine("Insufficient funds");
                return;
            }

            Balance -= transaction.Amount;
            Console.WriteLine($"[Savings {AccountNumber}] Deducted {transaction.Amount:C} for '{transaction.Category}'. Updated balance: {Balance:C}");
        }
    }

    public class FinanceApp
    {
        private readonly List<Transaction> _transactions = new();

        public void Run()
        {
            var account = new SavingsAccount(accountNumber: "SA-001", initialBalance: 1000m);
            Console.WriteLine($"Opened Savings Account {account.AccountNumber} with balance {account.Balance:C}");
            Console.WriteLine(new string('-', 64));

            var t1 = new Transaction(Id: 1, Date: DateTime.Now, Amount: 150m, Category: "Groceries");
            var t2 = new Transaction(Id: 2, Date: DateTime.Now, Amount: 300m, Category: "Utilities");
            var t3 = new Transaction(Id: 3, Date: DateTime.Now, Amount: 800m, Category: "Entertainment"); // will trigger insufficient funds after t1 + t2

            // iii. Use processors
            ITransactionProcessor p1 = new MobileMoneyProcessor();
            ITransactionProcessor p2 = new BankTransferProcessor();
            ITransactionProcessor p3 = new CryptoWalletProcessor();

            p1.Process(t1);
            p2.Process(t2);
            p3.Process(t3);

            Console.WriteLine(new string('-', 64));

            account.ApplyTransaction(t1);
            account.ApplyTransaction(t2);
            account.ApplyTransaction(t3); 

            Console.WriteLine(new string('-', 64));

            _transactions.AddRange(new[] { t1, t2, t3 });

            // Simple printout to confirm storage
            Console.WriteLine("All tracked transactions:");
            foreach (var tx in _transactions)
            {
                Console.WriteLine($"  - #{tx.Id}: {tx.Category} | {tx.Amount:C} | {tx.Date:g}");
            }
        }
    }

    public static class Program
    {
        public static void Main()
        {
            var app = new FinanceApp();
            app.Run();

            Console.WriteLine();
            Console.WriteLine("Simulation complete. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
