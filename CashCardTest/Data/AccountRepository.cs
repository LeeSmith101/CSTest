using CashCardTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashCardTest.Data
{
    public class AccountRepository : IAccountRepository
    {
        private List<Account> _accounts = new List<Account>
        {
            new Account
            {
                AccountName = "Lee Smith",
                CardNumber = "4929-4657-3215-2434",
                Pin="2432",
                Transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        Description = "Opening Balance",
                        TransactionDate = DateTime.Now,
                        Amount = 5000
                    }
                },
                Balance = 5000
            },
            new Account
            {
                AccountName = "John Doe",
                CardNumber = "4929-3242-3435-9568",
                Pin = "6576",
                Transactions = new List<Transaction>
                {
                   new Transaction
                   {
                       Description = "Opening Balance",
                       TransactionDate = DateTime.Now,
                       Amount = 100
                   }
                },
                Balance = 100
            }
        };

        public Account GetAccount(string cardNumber)
        {
            return _accounts.FirstOrDefault(a => a.CardNumber == cardNumber);
        }
    }
}
