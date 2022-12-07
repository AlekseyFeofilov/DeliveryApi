namespace DeliveryAppAPI.Configurations;

public static class ErrorMessage
{
    public const string TooEarlyDate = "Date can't be earlier than {0}";
    public const string TooLateDate = "Date can't be later than {0}";
    public const string EmptyCart = "There are no dish baskets in the cart";
    public const string RegisteredEmail = "Email is already in registered";
    public const string InvalidUserNameOrPassword = "Invalid username or password";
    public const string UnexpectedError = "Unecpected error";
}