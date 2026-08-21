using Application.OutPorts;
using Application.Dtos;
using Microsoft.EntityFrameworkCore;
using Storage.Context;
using Storage.Mappers;

namespace Storage.Repositories;

public class OpportunityReadRepo : IOpportunityReadRepo
{
    private readonly AppDbContext _context;
    
    public OpportunityReadRepo(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<OpportunityDto?> GetById(Guid id, CancellationToken ct = default)
    {
        var opportunity = await _context.Opportunities
            .AsNoTracking()
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, ct);
        return opportunity?.ToDto();
    }

    public async Task<IReadOnlyList<OpportunityDto>> GetList(CancellationToken ct = default)
    {
        var opportunities = await _context.Opportunities
            .AsNoTracking()
            .Include(o => o.Items)
            .ToListAsync(ct);
        return opportunities.ToDto().ToList();
    }
}