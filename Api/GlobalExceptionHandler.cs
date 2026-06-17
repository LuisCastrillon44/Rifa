using Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        IProblemDetailsService problemDetailsService,
        ILogger<GlobalExceptionHandler> logger)
    {
        _problemDetailsService = problemDetailsService;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        var problem = MapProblem(exception);

        if (problem is null)
        {
            _logger.LogError(exception, "Unhandled exception");
            return false; // deja que el handler por defecto responda 500
        }

        httpContext.Response.StatusCode = problem.Status!.Value;

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = problem
        });
    }

    private static ProblemDetails? MapProblem(Exception exception) => exception switch
    {
        ValidationException ve => new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Error de validacion",
            Detail = string.Join(" | ", ve.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"))
        },
        NotFoundException nfe => new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Recurso no encontrado",
            Detail = nfe.Message
        },
        ConflictException ce => new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Conflicto",
            Detail = ce.Message
        },
        UnauthorizedException ue => new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "No autorizado",
            Detail = ue.Message
        },
        _ => null
    };
}
