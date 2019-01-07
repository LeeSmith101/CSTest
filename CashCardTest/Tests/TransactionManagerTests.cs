using CashCardTest.BusinessLogic;
using CashCardTest.Data;
using CashCardTest.Models;
using NUnit.Framework;

namespace CashCardTest.Tests
{
    [TestFixture]
    public class TransactionManagerTests
    {
        private ITransactionManager _transactionManager;
        private Card _goodCard = new Card { CardNumber = "4929-4657-3215-2434" };
        private string _goodPin = "2432";
        private Card _badCard = new Card { CardNumber = "1111-1111-1111-1111" };
        private string _badPin = "1111";

        [SetUp]
        public void Setup()
        {
            _transactionManager = new TransactionManager(new AccountRepository());
        }

        [Test]
        public void TestDebitAccountWithIncorrectCard()
        {
            var result = _transactionManager.DebitAccount(_badCard, _goodPin, 100);

            Assert.IsFalse(result.Success);

            var balanceCheck = _transactionManager.GetAccountBalance(_goodCard, _goodPin);
            Assert.AreEqual(balanceCheck.Message, "Account Balance is £5000");
        }

        [Test]
        public void TestDebitAccountWithIncorrectPin()
        {
            var result = _transactionManager.DebitAccount(_goodCard, _badPin, 100);

            Assert.IsFalse(result.Success);

            var balanceCheck = _transactionManager.GetAccountBalance(_goodCard, _goodPin);
            Assert.AreEqual(balanceCheck.Message, "Account Balance is £5000");
        }

        [Test]
        public void TestDebitAccountWithInsufficientFunds()
        {
            var result = _transactionManager.DebitAccount(_goodCard, _goodPin, 1000000000);

            Assert.IsFalse(result.Success);

            var balanceCheck = _transactionManager.GetAccountBalance(_goodCard, _goodPin);
            Assert.AreEqual(balanceCheck.Message, "Account Balance is £5000");
        }

        [Test]
        public void TestDebitAccountWithCorrectCardAndPin()
        {
            var result = _transactionManager.DebitAccount(_goodCard, _goodPin, 100);

            Assert.IsTrue(result.Success);

            var balanceCheck = _transactionManager.GetAccountBalance(_goodCard, _goodPin);
            Assert.AreEqual(balanceCheck.Message, "Account Balance is £4900");
        }

        [Test]
        public void TestCreditAccountWithIncorrectCard()
        {
            var result = _transactionManager.CreditAccount(_badCard, _goodPin, 100);

            Assert.IsFalse(result.Success);

            var balanceCheck = _transactionManager.GetAccountBalance(_goodCard, _goodPin);
            Assert.AreEqual(balanceCheck.Message, "Account Balance is £5000");
        }

        [Test]
        public void TestCredutAccountWithIncorrectPin()
        {
            var result = _transactionManager.CreditAccount(_goodCard, _badPin, 100);

            Assert.IsFalse(result.Success);

            var balanceCheck = _transactionManager.GetAccountBalance(_goodCard, _goodPin);
            Assert.AreEqual(balanceCheck.Message, "Account Balance is £5000");
        }

        [Test]
        public void TestCreditAccountWithCorrectCardAndPin()
        {
            var result = _transactionManager.CreditAccount(_goodCard, _goodPin, 100);

            Assert.IsTrue(result.Success);

            var balanceCheck = _transactionManager.GetAccountBalance(_goodCard, _goodPin);
            Assert.AreEqual(balanceCheck.Message, "Account Balance is £5100");
        }

        [Test]
        public void TestBalanceWithIncorrectCard()
        {
            var result = _transactionManager.GetAccountBalance(_badCard, _goodPin);

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void TestBalanceWithIncorrectPin()
        {
            var result = _transactionManager.GetAccountBalance(_goodCard, _badPin);

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void TestBalanceWithCorrectCardAndPin()
        {
            var result = _transactionManager.GetAccountBalance(_goodCard, _goodPin);

            Assert.IsTrue(result.Success);
        }
    }
}
