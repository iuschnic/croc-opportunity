namespace Application.Exceptions;

public class AppNotFoundException : AppException
{
    public AppNotFoundException(string message) : base(message) { }
    public AppNotFoundException() : base("Entity not found error") { }
}