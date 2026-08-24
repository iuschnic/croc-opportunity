using Domain.Exceptions;
using Domain.Enums;

namespace Domain.Models;

public class Opportunity
{
    public const int LossReasonMinLength = 1;
    public const int LossReasonMaxLength = 300;

    public Guid Id { get; }
    public Guid ContactId { get; }
    public OpportunityStatus Status { get; private set; }
    public DateTime CreatedAt { get; }
    public string? LossReason { get; private set; }
    public Currency Currency { get; }

    public Money TotalAmount => CalculateTotal(Currency, _items);
    public IReadOnlyList<OpportunityItem> Items => _items.AsReadOnly();

    private readonly List<OpportunityItem> _items;

    private Opportunity(Guid id, Guid contactId, OpportunityStatus status, Currency currency,
        DateTime createdAt, List<OpportunityItem> items, string? lossReason)
    {
        ArgumentNullException.ThrowIfNull(items);
        CheckCurrency(currency);
        CheckItemsAreUnique(items);
        CheckLossReasonMatchesStatus(status, lossReason);
        CheckIfWonThanNonZeroTotal(status, CalculateTotal(currency, items));

        Id = id;
        ContactId = contactId;
        Status = status;
        Currency = currency;
        CreatedAt = createdAt;
        LossReason = lossReason;
        _items = [..items];
    }

    public static Opportunity Create(Guid contactId, Currency currency)
    {
        return new Opportunity(Guid.NewGuid(), contactId, OpportunityStatus.Draft, 
            currency, DateTime.UtcNow, [], null);
    }

    public static Opportunity Restore(Guid id, Guid contactId, OpportunityStatus status, Currency currency,
        DateTime createdAt, IEnumerable<OpportunityItem> items, string? lossReason)
    {
        return new Opportunity(id, contactId, status, currency, createdAt, items.ToList(), lossReason);
    }

    public void MarkAsDraft()
    {
        Status = OpportunityStatus.Draft;
        LossReason = null;
    }

    public void MarkAsNegotiation()
    {
        Status = OpportunityStatus.Negotiation;
        LossReason = null;
    }

    public void MarkAsWon()
    {
        CheckIfWonThanNonZeroTotal(OpportunityStatus.Won, TotalAmount);
        Status = OpportunityStatus.Won;
        LossReason = null;
    }

    public void MarkAsLost(string newLossReason)
    {
        CheckLossReasonMatchesStatus(OpportunityStatus.Lost, newLossReason);
        Status = OpportunityStatus.Lost;
        LossReason = newLossReason;
    }

    public void UpdateLossReason(string newLossReason)
    {
        if (Status != OpportunityStatus.Lost)
            throw new DomainRuleViolationException("Status should be Lost in order to change Loss Reason");
        CheckLossReasonMatchesStatus(Status, newLossReason);
        LossReason = newLossReason;
    }

    public void AddItem(OpportunityItem toAddItem)
    {
        ArgumentNullException.ThrowIfNull(toAddItem);
        CheckIfCanEditItems();
        if (_items.Any(item => item.Id == toAddItem.Id))
            throw new DomainConflictException($"Item {toAddItem.Id} is already added in Opportunity {Id}");
        _items.Add(toAddItem);
    }

    public void RemoveItem(Guid toRemoveItemId)
    {
        CheckIfCanEditItems();
        var toRemoveItem = _items.FirstOrDefault(item => item.Id == toRemoveItemId)
            ?? throw new DomainNotFoundException($"Item {toRemoveItemId} not found in Opportunity {Id}");
        _items.Remove(toRemoveItem);
    }

    // Так как покупка в сделке неотделима от агрегата сделки, обновление покупки происходит через агрегат
    public void UpdateItem(OpportunityItem newItem)
    {
        ArgumentNullException.ThrowIfNull(newItem);
        CheckIfCanEditItems();
        var oldItem = _items.FirstOrDefault(item => item.Id == newItem.Id)
            ?? throw new DomainNotFoundException($"Item {newItem.Id} not found in Opportunity {Id}");

        oldItem.UpdateDiscount(newItem.Discount);
        oldItem.UpdateName(newItem.Name);
        oldItem.UpdateQuantity(newItem.Quantity);
        oldItem.UpdatePricePerUnit(newItem.PricePerUnit);
    }

    private void CheckIfCanEditItems()
    {
        if (Status is OpportunityStatus.Lost or OpportunityStatus.Won)
            throw new DomainRuleViolationException("Cannot edit items when Status is Lost or Won");
    }

    private static void CheckCurrency(Currency currency)
    {
        if (!Enum.IsDefined(currency))
            throw new DomainValidationException("Currency is not specified");
    }

    private static void CheckLossReasonMatchesStatus(OpportunityStatus status, string? lossReason)
    {
        if (status == OpportunityStatus.Lost)
        {
            if (string.IsNullOrWhiteSpace(lossReason)
                || lossReason.Length < LossReasonMinLength
                || lossReason.Length > LossReasonMaxLength)
                throw new DomainValidationException(
                    $"Loss reason should not be empty or whitespace and " +
                    $"have length between {LossReasonMinLength} and {LossReasonMaxLength} characters");
        }
        else if (lossReason is not null)
        {
            throw new DomainRuleViolationException("Loss reason is allowed only when Status is Lost");
        }
    }

    private static void CheckIfWonThanNonZeroTotal(OpportunityStatus status, Money totalAmount)
    {
        if (status == OpportunityStatus.Won && totalAmount.Amount == 0)
            throw new DomainRuleViolationException("Cannot assign Won status when Total Amount is 0");
    }

    private static void CheckItemsAreUnique(List<OpportunityItem> items)
    {
        if (items.GroupBy(item => item.Id).Any(group => group.Count() > 1))
            throw new DomainValidationException("Opportunity items must have unique ids");
    }

    private static Money CalculateTotal(Currency currency, IEnumerable<OpportunityItem> items)
    {
        var total = Money.Zero(currency);
        total += items.Sum(item => item.PricePerUnit * item.Quantity * (1.0m - item.Discount));
        return total;
    }
}