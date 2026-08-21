using Microsoft.AspNetCore.Mvc;
using API.Mappers;
using API.Models;
using Application.Commands;
using Application.InPorts;

namespace API.Controllers;

using ILogger = Serilog.ILogger;

[ApiController]
[Route("api/opportunities")]
public class OpportunityController: ControllerBase
{
    private readonly ILogger _logger;
    private readonly IOpportunityCommands _commandService;
    private readonly IOpportunityQueries _queryService;
    
    public OpportunityController(ILogger logger, 
        IOpportunityCommands commandService, IOpportunityQueries queryService)
    {
        _logger = logger;
        _commandService = commandService;
        _queryService = queryService;
    }

    /// <summary>
    /// Получение сделки по Id
    /// </summary>
    /// <param name="id">Id искомой сделки</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Информация о сделке</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetByIdResponse>> GetOpportunityById(
        Guid id, 
        CancellationToken ct)
    {
        var result = await _queryService.GetByIdAsync(id, ct);
        _logger.Information("Retrieved opportunity for id {id}", id);
        return Ok(new GetByIdResponse(){Opportunity = result});
    }
    
    /// <summary>
    /// Получение списка сделок
    /// </summary>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Информация о сделке</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<GetListResponse>> GetListAsync(
        CancellationToken ct)
    {
        var result = await _queryService.GetListAsync(ct);
        _logger.Information("Retrieved opportunities list");
        return Ok(new GetListResponse() {Opportunities = result});
    }

    /// <summary>
    /// Создать новую сделку
    /// </summary>
    /// <param name="request">Информация о сделке</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Созданный расход</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateOpportunity(
        [FromBody] CreateOpportunityRequest request, 
        CancellationToken ct)
    {
        var createdId = await _commandService.CreateAsync(request.ToCommand(), ct);
        _logger.Information("Created opportunity {opportunityId}", createdId);
        return CreatedAtAction(nameof(GetOpportunityById), 
            routeValues: new { id = createdId },
            value: new CreateResponse(){CreatedOpportunityId =  createdId});
    }

    /// <summary>
    /// Изменить статус сделки
    /// </summary>
    /// <param name="id">Идентификатор обновляемой сделки</param>
    /// <param name="request">Информация об обновлении сделки</param>
    /// <param name="ct">Токен отмены</param>
    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(
        Guid id,
        [FromBody] UpdateOpportunityStatusRequest request,
        CancellationToken ct)
    {
        await _commandService.UpdateStatusAsync(request.ToCommand(id), ct);
        _logger.Information("Updated opportunity {opportunityId} status to {status}", id, request.NewStatus);
        return NoContent();
    }

    /// <summary>
    /// Обновить причину проигрыша сделки(только для статуса Lost)
    /// </summary>
    /// <param name="id">Идентификатор обновляемой сделки</param>
    /// <param name="request">Информация об обновлении сделки</param>
    /// <param name="ct">Токен отмены</param>
    [HttpPatch("{id:guid}/loss-reason")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLossReason(
        Guid id,
        [FromBody] UpdateOpportunityLossReasonRequest request,
        CancellationToken ct)
    {
        await _commandService.UpdateLossReasonAsync(request.ToCommand(id), ct);
        _logger.Information("Updated opportunity {opportunityId} loss reason", id);
        return NoContent();
    }

    /// <summary>
    /// Удалить сделку
    /// </summary>
    /// <param name="id">Идентификатор удаляемой сделки</param>
    /// <param name="ct">Токен отмены</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id, 
        CancellationToken ct)
    {
        var cmd = new DeleteOpportunityCommand() { Id = id };
        await _commandService.DeleteAsync(cmd, ct);
        _logger.Information("Deleted opportunity {opportunityId}", id);
        return NoContent();
    }

    /// <summary>
    /// Добавить покупку (item) в сделку
    /// </summary>
    /// <param name="opportunityId">Идентификатор обновляемой сделки</param>
    /// <param name="request">Информация о новой покупке</param>
    /// <param name="ct">Токен отмены</param>
    [HttpPost("{opportunityId:guid}/items")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AddItemResponse>> AddItem(
        Guid opportunityId,
        [FromBody] AddOpportunityItemRequest request,
        CancellationToken ct)
    {
        var addedItemId = await _commandService.AddItemAsync(request.ToCommand(opportunityId), ct);
        _logger.Information("Added item {itemId} to opportunity {opportunityId}",
            addedItemId, opportunityId);
        return CreatedAtAction(
            nameof(GetOpportunityById),
            routeValues: new { id = opportunityId },
            value: new AddItemResponse { AddedItemId = addedItemId });
    }

    /// <summary>
    /// Обновить покупку (item) в сделке
    /// </summary>
    /// <param name="opportunityId">Идентификатор обновляемой сделки</param>
    /// <param name="opportunityItemId">Идентификатор обновляемой покупки в сделке</param>
    /// <param name="request">Информация об обновлении покупки</param>
    /// <param name="ct">Токен отмены</param>
    [HttpPut("{opportunityId:guid}/items/{opportunityItemId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateItem(
        Guid opportunityId,
        Guid opportunityItemId,
        [FromBody] UpdateOpportunityItemRequest request,
        CancellationToken ct)
    {
        await _commandService.UpdateItemAsync(request.ToCommand(opportunityId, opportunityItemId), ct);
        _logger.Information(
            "Updated item {itemId} in opportunity {opportunityId}",
            opportunityItemId, opportunityId);
        return NoContent();
    }

    /// <summary>
    /// Удалить покупку (item) из сделки
    /// </summary>
    /// <param name="opportunityId">Идентификатор обновляемой сделки</param>
    /// <param name="opportunityItemId">Идентификатор удаляемой покупки в сделке</param>
    /// <param name="ct">Токен отмены</param>
    [HttpDelete("{opportunityId:guid}/items/{opportunityItemId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveItem(
        Guid opportunityId, 
        Guid opportunityItemId, 
        CancellationToken ct)
    {
        var cmd = new RemoveOpportunityItemCommand() 
            { OpportunityId = opportunityId, OpportunityItemId = opportunityItemId }; 
        await _commandService.RemoveItemAsync(cmd, ct);
        _logger.Information(
            "Removed item {itemId} from opportunity {opportunityId}",
            opportunityItemId, opportunityId);
        return NoContent();
    }
}