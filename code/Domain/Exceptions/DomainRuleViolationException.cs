namespace Domain.Exceptions;

public class DomainRuleViolationException : DomainException
{
    public DomainRuleViolationException(string message) : base(message) { }
    public DomainRuleViolationException() : base("Rule violation error") { }
}