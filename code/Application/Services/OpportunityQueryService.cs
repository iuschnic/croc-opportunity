using Application.Dtos;
using Application.InPorts;
using Application.OutPorts;

namespace Application.Services;

public class OpportunityQueryService(IOpportunityReadRepo opportunityReadRepo): IOpportunityQueries
{
    public async Task<OpportunityDto> GetByIdAsync(Guid opportunityId, CancellationToken ct = default)
    {
        return await opportunityReadRepo.GetById(opportunityId, ct) 
               ??  throw new ApplicationException($"Opportunity with id {opportunityId} not found");
    }

    public async Task<IReadOnlyList<OpportunityDto>> GetListAsync(CancellationToken ct = default)
    {
        return await opportunityReadRepo.GetList(ct);
    }
}