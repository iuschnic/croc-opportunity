using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Storage.Models;

public class OpportunityItemDb
{
    internal OpportunityItemDb() { }
    
    public OpportunityItemDb(Guid id, Guid opportunityId, string name, int quantity, 
        decimal pricePerUnit, decimal discount)
    {
        Id = id;
        OpportunityId = opportunityId;
        Name = name;
        Quantity = quantity;
        PricePerUnit = pricePerUnit;
        Discount = discount;
    }
    
    public Guid Id { get; set; }
    public Guid OpportunityId { get; set; }
    [StringLength(OpportunityItem.NameMaxLength,  MinimumLength = OpportunityItem.NameMinLength)]
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal PricePerUnit { get; set; }
    public decimal Discount { get; set; }
    
    public OpportunityDb? Opportunity { get; set; }
}