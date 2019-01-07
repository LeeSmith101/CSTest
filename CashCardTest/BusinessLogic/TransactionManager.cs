using CashCardTest.Data;
using CashCardTest.Models;
using System;
using System.Threading;

namespace CashCardTest.BusinessLogic
{
    public class TransactionManager : ITransactionManager
    {
        private IAccountRepository _accountRepository;
        private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public TransactionManager(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public TransactionResult CreditAccount(Card card, string pin, decimal amount)
        {
            _lock.EnterUpgradeableReadLock();

            var lookupResult = GetAccount(card, pin);    

            if (!lookupResult.Success)
            {
                _lock.ExitUpgradeableReadLock();
                return new TransactionResult { Success = false, Message = lookupResult.Message };
            }

            _lock.EnterWriteLock();

            lookupResult.Account.Balance += amount;
            lookupResult.Account.Transactions.Add(new Transaction { TransactionDate = DateTime.Now, Description = "Credit", Amount = amount });

            _lock.ExitWriteLock();

            _lock.ExitUpgradeableReadLock();

            return new TransactionResult { Success = true, Message = string.Format("Account successfully credited £{0}", amount) };
        }

        public TransactionResult DebitAccount(Card card, string pin, decimal amount)
        {
            _lock.EnterUpgradeableReadLock();

            var lookupResult = GetAccount(card, pin);

            if (!lookupResult.Success)
            {
                _lock.ExitUpgradeableReadLock();
                return new TransactionResult { Success = false, Message = lookupResult.Message };
            }

            if (lookupResult.Account.Balance <= amount)
            {
                _lock.ExitUpgradeableReadLock();
                return new TransactionResult { Success = false, Message = "Insufficient Funds" };
            }

            _lock.EnterWriteLock();

            lookupResult.Account.Balance -= amount;
            lookupResult.Account.Transactions.Add(new Transaction { TransactionDate = DateTime.Now, Description = "Debit", Amount = amount });

            _lock.ExitWriteLock();

            _lock.ExitUpgradeableReadLock();

            return new TransactionResult { Success = true, Message = string.Format("Account successfully debited £{0}", amount) };
        }

        public TransactionResult GetAccountBalance(Card card, string pin)
        {
            _lock.EnterReadLock();

            var lookupResult = GetAccount(card, pin);

            if (!lookupResult.Success)
            {
                _lock.ExitReadLock();
                return new TransactionResult { Success = false, Message = lookupResult.Message };
            }

            _lock.ExitReadLock();

            return new TransactionResult { Success = true, Message = string.Format("Account Balance is £{0}", lookupResult.Account.Balance) };
        }

        private AccountLookupResult GetAccount(Card card, string pin)
        {
            var account = _accountRepository.GetAccount(card.CardNumber);

            if (account == null)
            {
                return new AccountLookupResult { Account = null, Message = "Account not found", Success = false };
            }

            if (!ValidatePin(account, pin))
            {
                return new AccountLookupResult { Account = null, Message = "Incorrect Pin", Success = false };
            }

            return new AccountLookupResult { Account = account, Message = "Success", Success = true };
        }


        private bool ValidatePin(Account account, string pin)
        {
            return account != null && account.Pin == pin;
        }
    }
}
