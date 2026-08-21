using Application.Commands;
namespace Application.InPorts;

public interface IOpportunityCommands
{
    Task<Guid> CreateAsync(CreateOpportunityCommand command, CancellationToken ct = default);
    Task UpdateStatusAsync(UpdateOpportunityStatusCommand command, CancellationToken ct = default);
    Task UpdateLossReasonAsync(UpdateOpportunityLossReasonCommand command, CancellationToken ct = default);
    Task DeleteAsync(DeleteOpportunityCommand command, CancellationToken ct = default);
    
    Task<Guid> AddItemAsync(AddOpportunityItemCommand command, CancellationToken ct = default);
    Task UpdateItemAsync(UpdateOpportunityItemCommand command, CancellationToken ct = default);
    Task RemoveItemAsync(RemoveOpportunityItemCommand command, CancellationToken ct = default);
}