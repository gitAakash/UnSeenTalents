using System.ComponentModel;

namespace UnseentalentsApp.Enums
{
    public enum TransactionType
    {
        [Description("Credit")] Credit = 1,
        [Description("Debit")] Debit = 2
    }
}