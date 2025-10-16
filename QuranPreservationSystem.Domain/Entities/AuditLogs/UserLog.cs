using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuranPreservationSystem.Domain.Entities.AuditLogs;

/// <summary>
/// سجل عمليات المستخدمين
/// </summary>
[Table("UserLogs")]
[Index(nameof(RecordId), nameof(Timestamp))]
[Index(nameof(UserId), nameof(Timestamp))]
[Index(nameof(ActionType), nameof(Timestamp))]
[Index(nameof(Timestamp))]
public class UserLog : BaseAuditLog
{
    /// <summary>
    /// اسم المستخدم المستهدف (الذي تمت عليه العملية)
    /// </summary>
    [StringLength(200)]
    public string? TargetUserName { get; set; }
}

