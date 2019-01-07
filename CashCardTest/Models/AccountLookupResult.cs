namespace CashCardTest.Models
{
    public class AccountLookupResult
    {
        public bool Success { get; set; }

        public Account Account { get; set; }

        public string Message { get; set; }
    }
}
