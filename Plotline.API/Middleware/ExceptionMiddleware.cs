using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Plotline.Application.Exceptions;

namespace Plotline.API.Middleware;

/// <summary>
/// Middleware для глобальной обработки исключений в приложении.
/// Перехват, логирование и возврат структурированных HTTP-ответов.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ExceptionMiddleware"/>.
    /// </summary>
    /// <param name="next">Следующий делегат в конвейере запросов.</param>
    /// <param name="logger">Логгер для записи информации об исключениях.</param>
    /// <param name="env">Среда выполнения приложения (Development/Production).</param>
    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    /// <summary>
    /// Обрабатывает HTTP-запрос и перехватывает исключения.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса.</param>
    /// <returns>Задача, представляющая асинхронную операцию обработки запроса.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UnauthorizedException ex)
        {
            _logger.LogWarning(ex, "Unauthorized: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Not Found: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
        catch (ConflictException ex)
        {
            _logger.LogWarning(ex, "Conflict: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation failed: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
        catch (BadHttpRequestException ex)
        {
            _logger.LogWarning(ex, "Bad Request: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            
            _logger.LogError(ex, "Unhandled exception");
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Обрабатывает исключение и формирует HTTP-ответ.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса.</param>
    /// <param name="exception">Исключение для обработки.</param>
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = GetStatusCode(exception);
        var problemType = GetProblemType(exception);
        var problemTitle = GetProblemTitle(exception);
        var validationErrors = GetValidationErrors(exception);

        // Создаем объект ProblemDetails согласно RFC 7807
        var problem = new ProblemDetails
        {
            Type = problemType,
            Title = problemTitle,
            Status = statusCode,
            Detail = exception.Message,
            Instance = context.Request.Path,
            Extensions = { ["traceId"] = context.TraceIdentifier }
        };

        // Добавляем ошибки валидации, если они есть
        if (validationErrors != null)
        {
            problem.Extensions["errors"] = validationErrors;
        }

        // В среде разработки добавляем stack trace для отладки
        if (_env.IsDevelopment())
        {
            problem.Extensions["stackTrace"] = exception.StackTrace;
        }

        context.Response.StatusCode = statusCode;

        var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = _env.IsDevelopment()
        });

        await context.Response.WriteAsync(json);
    }

    /// <summary>
    /// Определяет HTTP-статус код на основе типа исключения.
    /// </summary>
    /// <param name="exception">Исключение для анализа.</param>
    /// <returns>Соответствующий HTTP-статус код.</returns>
    private static int GetStatusCode(Exception exception) => exception switch
    {
        UnauthorizedException => StatusCodes.Status401Unauthorized,
        ConflictException => StatusCodes.Status409Conflict,
        NotFoundException => StatusCodes.Status404NotFound,
        ValidationException => StatusCodes.Status400BadRequest,
        BadHttpRequestException => StatusCodes.Status400BadRequest,
        _ => StatusCodes.Status500InternalServerError
    };

    /// <summary>
    /// Определяет тип проблемы (RFC ссылка) на основе исключения.
    /// </summary>
    /// <param name="exception">Исключение для анализа.</param>
    /// <returns>Ссылка на спецификацию RFC для типа проблемы.</returns>
    private static string GetProblemType(Exception exception) => exception switch
    {
        UnauthorizedException => "https://tools.ietf.org/html/rfc7235#section-3.1",
        ConflictException => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
        NotFoundException => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
        ValidationException => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        BadHttpRequestException => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
    };

    /// <summary>
    /// Определяет заголовок проблемы на основе типа исключения.
    /// </summary>
    /// <param name="exception">Исключение для анализа.</param>
    /// <returns>Человекочитаемый заголовок проблемы.</returns>
    private static string GetProblemTitle(Exception exception) => exception switch
    {
        UnauthorizedException => "Unauthorized",
        ConflictException => "Conflict",
        NotFoundException => "Not Found",
        ValidationException => "Validation Failed",
        BadHttpRequestException => "Bad Request",
        _ => "Internal Server Error"
    };

    /// <summary>
    /// Извлекает ошибки валидации из ValidationException.
    /// </summary>
    /// <param name="exception">Исключение для анализа.</param>
    /// <returns>Объект с ошибками валидации или null, если исключение не является ValidationException.</returns>
    private static object GetValidationErrors(Exception exception)
    {
        if (exception is ValidationException validationException)
        {
            return validationException.Errors;
        }

        return null;
    }
}