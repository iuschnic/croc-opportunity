using Application.Commands;
using Application.Enums;
using Application.Mappers;
using Application.Exceptions;
using Application.InPorts;
using Application.OutPorts;
using Domain.Models;

namespace Application.Services;

public class OpportunityCommandService(IOpportunityRepo opportunityRepo): IOpportunityCommands
{
    public async Task<Guid> CreateAsync(CreateOpportunityCommand command, CancellationToken ct = default)
    {
        var opportunity = Opportunity.Create(command.ContactId, command.Currency.ToDomain());
        await opportunityRepo.CreateAsync(opportunity, ct);
        return opportunity.Id;
    }

    public async Task UpdateStatusAsync(UpdateOpportunityStatusCommand command, CancellationToken ct = default)
    {
        var opportunity = await opportunityRepo.GetByIdAsync(command.Id, ct);
        if  (opportunity == null)
            throw new AppNotFoundException($"Opportunity {command.Id} not found");
        switch (command.NewStatus)
        {
            case OpportunityStatusApp.Draft:
                opportunity.MarkAsDraft();
                break;
            case OpportunityStatusApp.Negotiation:
                opportunity.MarkAsNegotiation();
                break;
            case OpportunityStatusApp.Lost:
                if (command.NewLossReason == null)
                    throw new AppValidationException("LossReason should be specified when applying Lost status");
                opportunity.MarkAsLost(command.NewLossReason);
                break;
            case OpportunityStatusApp.Won:
                opportunity.MarkAsWon();
                break;
            default:
                throw new AppValidationException($"Unknown opportunity status {nameof(command.NewStatus)}");
        }
        await opportunityRepo.UpdateAsync(opportunity, ct);
    }

    public async Task UpdateLossReasonAsync(UpdateOpportunityLossReasonCommand command, CancellationToken ct = default)
    {
        var opportunity = await opportunityRepo.GetByIdAsync(command.Id, ct);
        if  (opportunity == null)
            throw new AppNotFoundException($"Opportunity {command.Id} not found");
        opportunity.UpdateLossReason(command.NewLossReason);
    }

    public async Task DeleteAsync(DeleteOpportunityCommand command, CancellationToken ct = default)
    {
        await opportunityRepo.DeleteAsync(command.Id, ct);
    }

    public async Task<Guid> AddItemAsync(AddOpportunityItemCommand command, CancellationToken ct = default)
    {
        var opportunity = await opportunityRepo.GetByIdAsync(command.OpportunityId, ct);
        if  (opportunity == null)
            throw new AppNotFoundException($"Opportunity {command.OpportunityId} not found");
        var newItem = OpportunityItem.Create(command.Name, command.Quantity, 
            command.PricePerUnit, command.Discount);
        opportunity.AddItem(newItem);
        await opportunityRepo.UpdateAsync(opportunity, ct);
        return newItem.Id;
    }

    public async Task UpdateItemAsync(UpdateOpportunityItemCommand command, CancellationToken ct = default)
    {
        var opportunity = await opportunityRepo.GetByIdAsync(command.OpportunityId, ct);
        if  (opportunity == null)
            throw new AppNotFoundException($"Opportunity {command.OpportunityId} not found");
        var newItem = OpportunityItem.Restore(command.OpportunityItemId, command.NewName, 
            command.NewQuantity, command.NewPricePerUnit, command.NewDiscount);
        opportunity.UpdateItem(newItem);
        await opportunityRepo.UpdateAsync(opportunity, ct);
    }

    public async Task RemoveItemAsync(RemoveOpportunityItemCommand command, CancellationToken ct = default)
    {
        var opportunity = await opportunityRepo.GetByIdAsync(command.OpportunityId, ct);
        if  (opportunity == null)
            throw new AppNotFoundException($"Opportunity {command.OpportunityId} not found");
        opportunity.RemoveItem(command.OpportunityItemId);
        await opportunityRepo.UpdateAsync(opportunity, ct);
    }
}