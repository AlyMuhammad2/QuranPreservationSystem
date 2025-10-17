using Microsoft.AspNetCore.Identity;
using QuranPreservationSystem.Application.Commands.AuditLog;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Domain.Enums;
using QuranPreservationSystem.Infrastructure.Identity;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuranPreservationSystem.Helpers;

/// <summary>
/// Extension Methods للـ Audit Logging
/// </summary>
public static class AuditLogExtensions
{
    /// <summary>
    /// تسجيل عملية إنشاء
    /// </summary>
    public static async Task LogCreateAsync<T>(
        this IAuditLogService auditLogService,
        ClaimsPrincipal user,
        UserManager<ApplicationUser> userManager,
        HttpContext httpContext,
        string entityType,
        int recordId,
        T newData,
        string? entityName = null) where T : class
    {
        var currentUser = await userManager.GetUserAsync(user);
        var command = new LogCommand
        {
            RecordId = recordId,
            ActionType = ActionType.Create,
            UserId = currentUser?.Id,
            UserName = currentUser?.FullName,
            IpAddress = httpContext.Connection.RemoteIpAddress?.ToString(),
            NewData = JsonSerializer.Serialize(newData, GetJsonSerializerOptions()),
            EntityName = entityName
        };

        await ExecuteLogAsync(auditLogService, entityType, command);
    }

    /// <summary>
    /// تسجيل عملية تعديل
    /// </summary>
    public static async Task LogUpdateAsync<T>(
        this IAuditLogService auditLogService,
        ClaimsPrincipal user,
        UserManager<ApplicationUser> userManager,
        HttpContext httpContext,
        string entityType,
        int recordId,
        T oldData,
        T newData,
        string? entityName = null) where T : class
    {
        var currentUser = await userManager.GetUserAsync(user);
        var command = new LogCommand
        {
            RecordId = recordId,
            ActionType = ActionType.Update,
            UserId = currentUser?.Id,
            UserName = currentUser?.FullName,
            IpAddress = httpContext.Connection.RemoteIpAddress?.ToString(),
            OldData = JsonSerializer.Serialize(oldData, GetJsonSerializerOptions()),
            NewData = JsonSerializer.Serialize(newData, GetJsonSerializerOptions()),
            EntityName = entityName
        };

        await ExecuteLogAsync(auditLogService, entityType, command);
    }

    /// <summary>
    /// تسجيل عملية حذف
    /// </summary>
    public static async Task LogDeleteAsync<T>(
        this IAuditLogService auditLogService,
        ClaimsPrincipal user,
        UserManager<ApplicationUser> userManager,
        HttpContext httpContext,
        string entityType,
        int recordId,
        T oldData,
        string? entityName = null) where T : class
    {
        var currentUser = await userManager.GetUserAsync(user);
        var command = new LogCommand
        {
            RecordId = recordId,
            ActionType = ActionType.Delete,
            UserId = currentUser?.Id,
            UserName = currentUser?.FullName,
            IpAddress = httpContext.Connection.RemoteIpAddress?.ToString(),
            OldData = JsonSerializer.Serialize(oldData, GetJsonSerializerOptions()),
            EntityName = entityName
        };

        await ExecuteLogAsync(auditLogService, entityType, command);
    }

    /// <summary>
    /// تسجيل عملية عرض (اختياري - للعمليات الحساسة فقط)
    /// </summary>
    public static async Task LogViewAsync(
        this IAuditLogService auditLogService,
        ClaimsPrincipal user,
        UserManager<ApplicationUser> userManager,
        HttpContext httpContext,
        string entityType,
        int recordId,
        string? entityName = null)
    {
        var currentUser = await userManager.GetUserAsync(user);
        var command = new LogCommand
        {
            RecordId = recordId,
            ActionType = ActionType.View,
            UserId = currentUser?.Id,
            UserName = currentUser?.FullName,
            IpAddress = httpContext.Connection.RemoteIpAddress?.ToString(),
            EntityName = entityName
        };

        await ExecuteLogAsync(auditLogService, entityType, command);
    }

    /// <summary>
    /// تسجيل عملية تصدير
    /// </summary>
    public static async Task LogExportAsync(
        this IAuditLogService auditLogService,
        ClaimsPrincipal user,
        UserManager<ApplicationUser> userManager,
        HttpContext httpContext,
        string entityType,
        string? notes = null)
    {
        var currentUser = await userManager.GetUserAsync(user);
        var command = new LogCommand
        {
            RecordId = 0,
            ActionType = ActionType.Export,
            UserId = currentUser?.Id,
            UserName = currentUser?.FullName,
            IpAddress = httpContext.Connection.RemoteIpAddress?.ToString(),
            Notes = notes
        };

        await ExecuteLogAsync(auditLogService, entityType, command);
    }

    /// <summary>
    /// الحصول على إعدادات JSON Serializer مع معالجة المراجع الدائرية
    /// </summary>
    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            WriteIndented = false,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            MaxDepth = 32
        };
    }

    private static async Task ExecuteLogAsync(IAuditLogService auditLogService, string entityType, LogCommand command)
    {
        switch (entityType.ToLower())
        {
            case "student":
                await auditLogService.LogStudentAsync(command);
                break;
            case "teacher":
                await auditLogService.LogTeacherAsync(command);
                break;
            case "center":
                await auditLogService.LogCenterAsync(command);
                break;
            case "course":
                await auditLogService.LogCourseAsync(command);
                break;
            case "exam":
                await auditLogService.LogExamAsync(command);
                break;
            case "enrollment":
                await auditLogService.LogEnrollmentAsync(command);
                break;
            case "hafizregistry":
                await auditLogService.LogHafizRegistryAsync(command);
                break;
            case "user":
                await auditLogService.LogUserAsync(command);
                break;
        }
    }
}

