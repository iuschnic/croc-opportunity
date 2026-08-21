using Domain.Exceptions;

namespace Domain.Models;

public class OpportunityItem
{
    public const int NameMinLength = 1;
    public const int NameMaxLength = 50;
    public Guid Id { get; }
    public string Name { get; private set; }
    public int Quantity { get; private set; }
    public decimal PricePerUnit { get; private set; }
    public decimal Discount { get; private set; }
    
    private OpportunityItem(Guid id, string name, int quantity, decimal pricePerUnit, decimal discount)
    {
        CheckName(name);
        CheckQuantity(quantity);
        CheckPricePerUnit(pricePerUnit);
        CheckDiscount(discount);
        Id = id;
        Name = name;
        Quantity = quantity;
        PricePerUnit = pricePerUnit;
        Discount = discount;
    }

    public static OpportunityItem Create(string name, int quantity, 
        decimal pricePerUnit, decimal discount)
    {
        return new OpportunityItem(Guid.NewGuid(), name, quantity, pricePerUnit, discount);
    }
    
    public static OpportunityItem Restore(Guid id, string name, int quantity, 
        decimal pricePerUnit, decimal discount)
    {
        return new OpportunityItem(id, name, quantity, pricePerUnit, discount);
    }

    public void UpdateName(string newName)
    {
        CheckName(newName);
        Name = newName;
    }

    public void UpdateQuantity(int newQuantity)
    {
        CheckQuantity(newQuantity);
        Quantity = newQuantity;
    }

    public void UpdatePricePerUnit(decimal newPricePerUnit)
    {
        CheckPricePerUnit(newPricePerUnit);
        PricePerUnit = newPricePerUnit;
    }

    public void UpdateDiscount(decimal newDiscount)
    {
        CheckDiscount(newDiscount);
        Discount = newDiscount;
    }

    private void CheckName(string name)
    {
        if (name.Length < NameMinLength || name.Length > NameMaxLength)
            throw new DomainValidationException("Name length should be between " +
                                                "" + NameMinLength + " and " + NameMaxLength);
    }
    
    private void CheckQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new DomainValidationException("Quantity should be greater than zero");
    }

    private void CheckPricePerUnit(decimal price)
    {
        if (price <= 0)
            throw new DomainValidationException("Price should be greater than zero");
    }

    private void CheckDiscount(decimal discount)
    {
        if (discount < 0.0m || discount > 1.0m)
            throw new DomainValidationException("Discount should be between 0.0 and 1.0");
    }
}