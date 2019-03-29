using System;

namespace Bank_Test
{
    class Account
    {

        public bool Deposit(decimal value)
        {
            this.Balance += value;
            Console.WriteLine("Deposit successfully");
            return true;
        }
        
        public bool Withdrawal(decimal value)
        {
            this.Balance -= value;
            Console.WriteLine("Withdraw successfully");
            return true;
        }

        public int CustomerId { get; set; }
        public int AccountNumber { get; set; }
        public decimal Balance { get; set; }
    }
}
