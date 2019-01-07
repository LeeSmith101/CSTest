using CashCardTest.Models;

namespace CashCardTest.Data
{
    public interface IAccountRepository
    {
        Account GetAccount(string cardNumber);
    }
}
