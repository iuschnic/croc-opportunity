using Application.Dtos;

namespace Application.OutPorts;

public interface IOpportunityReadRepo
{
    Task<OpportunityDto?> GetById(Guid opportunityId, CancellationToken ct = default);
    Task<IReadOnlyList<OpportunityDto>> GetList(CancellationToken ct = default);
}