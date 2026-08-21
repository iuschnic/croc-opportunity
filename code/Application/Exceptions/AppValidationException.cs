namespace Application.Exceptions;

public class AppValidationException: AppException
{
    public AppValidationException(string message) : base(message) { }
    public AppValidationException() : base("App validation error") { }
}