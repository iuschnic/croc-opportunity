using Application.Dtos;

namespace Application.InPorts;

public interface IOpportunityQueries
{
    Task<OpportunityDto> GetByIdAsync(Guid opportunityId, CancellationToken ct = default);
    // Мб стоит сделать еще метод для получения списка облегченных моделей (без вложенных items), но пока так
    Task<IReadOnlyList<OpportunityDto>> GetListAsync(CancellationToken ct = default);
}