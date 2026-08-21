using Application.Enums;
using Domain.Enums;
using Storage.Enums;

namespace Storage.Mappers;

public static class OpportunityStatusMapper
{
    public static OpportunityStatusDb ToDb(this OpportunityStatus status) => status switch
    {
        OpportunityStatus.Draft => OpportunityStatusDb.Draft,
        OpportunityStatus.Won => OpportunityStatusDb.Won,
        OpportunityStatus.Lost => OpportunityStatusDb.Lost,
        OpportunityStatus.Negotiation => OpportunityStatusDb.Negotiation,
        _ => throw new ArgumentOutOfRangeException(
            nameof(status), status, "Unknown opportunity status"),
    };
    public static OpportunityStatus ToDomain(this OpportunityStatusDb status) => status switch
    {
        OpportunityStatusDb.Draft => OpportunityStatus.Draft,
        OpportunityStatusDb.Won => OpportunityStatus.Won,
        OpportunityStatusDb.Lost => OpportunityStatus.Lost,
        OpportunityStatusDb.Negotiation => OpportunityStatus.Negotiation,
        _ => throw new ArgumentOutOfRangeException(
            nameof(status), status, "Unknown opportunity status"),
    };
    public static OpportunityStatusApp ToApp(this OpportunityStatusDb status) => status switch
    {
        OpportunityStatusDb.Draft => OpportunityStatusApp.Draft,
        OpportunityStatusDb.Won => OpportunityStatusApp.Won,
        OpportunityStatusDb.Lost => OpportunityStatusApp.Lost,
        OpportunityStatusDb.Negotiation => OpportunityStatusApp.Negotiation,
        _ => throw new ArgumentOutOfRangeException(
            nameof(status), status, "Unknown opportunity status"),
    };
}