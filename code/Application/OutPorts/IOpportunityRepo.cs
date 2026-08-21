using Domain.Models;

namespace Application.OutPorts;

public interface IOpportunityRepo
{
    Task<Opportunity?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task CreateAsync(Opportunity opportunity, CancellationToken ct = default);
    // изменение полей самого агрегата, добавление/удаление item, изменение item
    Task UpdateAsync(Opportunity opportunity, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}