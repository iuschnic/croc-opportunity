using Storage.Enums;

namespace Storage.Models;

public class OpportunityDb
{
    internal OpportunityDb() { }

    public OpportunityDb(Guid id, Guid contactId, OpportunityStatusDb status,
        DateTime createdAt, string? lossReason, CurrencyDb currency)
    {
        Id = id;
        ContactId = contactId;
        Status = status;
        CreatedAt = createdAt;
        LossReason = lossReason;
        Currency = currency;
    }

    public Guid Id { get; set; }
    public Guid ContactId { get; set; }
    public OpportunityStatusDb Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? LossReason { get; set; }
    public CurrencyDb Currency { get; set; }
    
    public ICollection<OpportunityItemDb> Items { get; set; } = [];
}