namespace Domain.Exceptions;

public class DomainConflictException : DomainException
{
    public DomainConflictException(string message) : base(message) { }
    public DomainConflictException() : base("Entity already exists error") { }
}