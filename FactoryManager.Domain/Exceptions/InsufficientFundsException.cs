public class InsufficientFundsException : Exception
{
    public InsufficientFundsException()
        : base("User does not have enough money.")
    {
    }
}