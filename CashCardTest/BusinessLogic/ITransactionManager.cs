using CashCardTest.Models;

namespace CashCardTest.BusinessLogic
{
    public interface ITransactionManager
    {
        TransactionResult CreditAccount(Card card, string pin, decimal amount);

        TransactionResult DebitAccount(Card card, string pin, decimal amount);

        TransactionResult GetAccountBalance(Card card, string pin);
    }
}
