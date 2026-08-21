namespace API.Controllers;

using Application.Exceptions;
using Domain.Exceptions;
using System.Text.Json;

using ILogger = Serilog.ILogger;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var traceId = context.TraceIdentifier;
        var method = context.Request.Method;
        var path = context.Request.Path;

        var errorResponse = new ErrorResponse
        {
            TraceId = traceId
        };

        switch (exception)
        {
            case AppValidationException or DomainValidationException or DomainRuleViolationException:
                response.StatusCode = StatusCodes.Status400BadRequest;
                errorResponse.Error = exception.Message;
                errorResponse.Code = "VALIDATION_ERROR";
                _logger.Information("Validation error at {Path}: {Message}", path, exception.Message);
                break;
            case AppNotFoundException:
                response.StatusCode = StatusCodes.Status404NotFound;
                errorResponse.Error = exception.Message;
                errorResponse.Code = "NOT_FOUND";
                _logger.Information("Not found at {Path}: {Message}", path, exception.Message);
                break;
            case AppConflictException or DomainConflictException:
                response.StatusCode = StatusCodes.Status409Conflict;
                errorResponse.Error = exception.Message;
                errorResponse.Code = "CONFLICT";
                _logger.Information("Conflict at {Path}: {Message}", path, exception.Message);
                break;
            default:
                response.StatusCode = StatusCodes.Status500InternalServerError;
                errorResponse.Error = _env.IsDevelopment()
                    ? exception.Message
                    : "An unexpected error occurred";
                errorResponse.Code = "INTERNAL_ERROR";
                _logger.Error(exception,
                    "Unhandled exception at {Method} {Path}: {ExceptionType} - {Message}",
                    method, path, exception.GetType().Name, exception.Message);
                break;
        }
        if (_env.IsDevelopment())
        {
            errorResponse.StackTrace = exception.StackTrace;
            if (exception.InnerException != null)
                errorResponse.InnerError = exception.InnerException.Message;
        }
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = _env.IsDevelopment()
        };
        await response.WriteAsJsonAsync(errorResponse, jsonOptions);
    }
}

public class ErrorResponse
{
    public string Error { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string TraceId { get; set; } = string.Empty;
    public string? StackTrace { get; set; }
    public string? InnerError { get; set; }
}