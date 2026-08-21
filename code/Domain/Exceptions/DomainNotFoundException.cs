namespace Domain.Exceptions;

public class DomainNotFoundException : DomainException
{
    public DomainNotFoundException(string message) : base(message) { }
    public DomainNotFoundException() : base("Entity not found error") { }
}