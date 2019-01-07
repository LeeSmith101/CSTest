using System;

namespace CashCardTest.Models
{
    public class Transaction
    {
        public DateTime TransactionDate { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }
    }
}
