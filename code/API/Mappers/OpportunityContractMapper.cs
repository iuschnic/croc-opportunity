using API.Models;
using Application.Commands;

namespace API.Mappers;

public static class OpportunityContractMapper
{
    public static CreateOpportunityCommand ToCommand(this CreateOpportunityRequest request)
    {
        return new CreateOpportunityCommand()
        {
            ContactId = request.ContactId,
            Currency = request.Currency
        };
    }
    
    public static UpdateOpportunityStatusCommand ToCommand(this UpdateOpportunityStatusRequest request, 
        Guid id)
    {
        return new UpdateOpportunityStatusCommand()
        {
            Id = id,
            NewStatus =  request.NewStatus,
            NewLossReason = request.NewLossReason
        };
    }
    
    public static UpdateOpportunityLossReasonCommand ToCommand(this UpdateOpportunityLossReasonRequest request,
        Guid id)
    {
        return new UpdateOpportunityLossReasonCommand()
        {
            Id = id,
            NewLossReason = request.NewLossReason
        };
    }
    
    public static AddOpportunityItemCommand ToCommand(this AddOpportunityItemRequest request, 
        Guid opportunityId)
    {
        return new AddOpportunityItemCommand()
        {
            OpportunityId = opportunityId,
            Name = request.Name,
            Quantity = request.Quantity,
            PricePerUnit = request.PricePerUnit,
            Discount = request.Discount
        };
    }
    
    public static UpdateOpportunityItemCommand ToCommand(this UpdateOpportunityItemRequest request, 
        Guid opportunityId, Guid  opportunityItemId)
    {
        return new UpdateOpportunityItemCommand()
        {
            OpportunityId = opportunityId,
            OpportunityItemId = opportunityItemId,
            NewName =  request.NewName,
            NewQuantity = request.NewQuantity,
            NewPricePerUnit = request.NewPricePerUnit,
            NewDiscount = request.NewDiscount
        };
    }
}