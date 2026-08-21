using Application.Enums;
using Domain.Enums;

namespace Application.Mappers;

public static class OpportunityStatusMapper
{
    public static OpportunityStatusApp ToApp(this OpportunityStatus status) => status switch
    {
        OpportunityStatus.Draft => OpportunityStatusApp.Draft,
        OpportunityStatus.Won => OpportunityStatusApp.Won,
        OpportunityStatus.Lost => OpportunityStatusApp.Lost,
        OpportunityStatus.Negotiation => OpportunityStatusApp.Negotiation,
        _ => throw new ArgumentOutOfRangeException(
            nameof(status), status, "Unknown opportunity status"),
    };
    public static OpportunityStatus ToDomain(this OpportunityStatusApp status) => status switch
    {
        OpportunityStatusApp.Draft => OpportunityStatus.Draft,
        OpportunityStatusApp.Won => OpportunityStatus.Won,
        OpportunityStatusApp.Lost => OpportunityStatus.Lost,
        OpportunityStatusApp.Negotiation => OpportunityStatus.Negotiation,
        _ => throw new ArgumentOutOfRangeException(
            nameof(status), status, "Unknown opportunity status"),
    };
}