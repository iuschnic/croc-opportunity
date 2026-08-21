using Microsoft.EntityFrameworkCore;
using Npgsql;
using Application.Exceptions;
using Application.OutPorts;
using Storage.Context;
using Storage.Models;
using Domain.Models;
using Storage.Mappers;

namespace Storage.Repositories;

public class OpportunityRepo: IOpportunityRepo
{
    private readonly AppDbContext _context;
    
    public OpportunityRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Opportunity?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var opportunity = await _context.Opportunities
            .AsNoTracking()
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, ct);
        return opportunity?.ToDomain();
    }

    public async Task CreateAsync(Opportunity opportunity, CancellationToken ct = default)
    {
        var opportunityDb = opportunity.ToDb();
        _context.Opportunities.Add(opportunityDb);
        try
        {
            await _context.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new AppConflictException($"Opportunity {opportunity.Id} already exists");
        }
    }
    
    public async Task UpdateAsync(Opportunity opportunity, CancellationToken ct = default)
    {
        var opportunityDb = await _context.Opportunities
            .Include(o => o.Items)
            .FirstOrDefaultAsync(el => el.Id == opportunity.Id, ct);
        if (opportunityDb == null)
            throw new AppNotFoundException($"Opportunity {opportunity.Id} not found");

        opportunityDb.ContactId = opportunity.ContactId;
        opportunityDb.Status = opportunity.Status.ToDb();
        opportunityDb.LossReason = opportunity.LossReason;
        opportunityDb.Currency = opportunity.Currency.ToDb();

        var itemsDbById = opportunityDb.Items.ToDictionary(i => i.Id);
        var itemsById = opportunity.Items.ToDictionary(i => i.Id);

        var oldItems = itemsDbById.Keys.ToHashSet();
        var newItems = itemsById.Keys.ToHashSet();
        var itemsToAdd = newItems.Except(oldItems).ToList();
        var itemsToRemove = oldItems.Except(newItems).ToList();
        var itemsToUpdate = oldItems.Intersect(newItems).ToList();

        foreach (var id in itemsToRemove)
            opportunityDb.Items.Remove(itemsDbById[id]);

        foreach (var id in itemsToUpdate)
        {
            var itemDb = itemsDbById[id];
            var item = itemsById[id];
            itemDb.Name = item.Name;
            itemDb.Quantity = item.Quantity;
            itemDb.PricePerUnit = item.PricePerUnit;
            itemDb.Discount = item.Discount;
        }

        foreach (var id in itemsToAdd)
        {
            var item = itemsById[id];
            opportunityDb.Items.Add(new OpportunityItemDb(
                item.Id,
                opportunityDb.Id,
                item.Name,
                item.Quantity,
                item.PricePerUnit,
                item.Discount));
        }

        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var deleted = await _context.Opportunities
            .Where(o => o.Id == id)
            .ExecuteDeleteAsync(ct);
        if (deleted == 0)
            throw new AppNotFoundException($"Opportunity {id} not found");
    }

    private static bool IsUniqueViolation(DbUpdateException ex)
    {
        return ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation };
    }
}