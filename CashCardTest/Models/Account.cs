using System.Collections.Generic;

namespace CashCardTest.Models
{
    public class Account
    {
        public Account()
        {
            Transactions = new List<Transaction>();
        }

        public string AccountName { get; set; }

        public string CardNumber { get; set; }

        public string Pin { get; set; }

        public List<Transaction> Transactions { get; set; }

        public decimal Balance { get; set; }
    }
}
