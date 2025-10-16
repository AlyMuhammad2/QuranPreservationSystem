using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuranPreservationSystem.Domain.Entities.AuditLogs;

/// <summary>
/// سجل عمليات المدرسين
/// </summary>
[Table("TeacherLogs")]
[Index(nameof(RecordId), nameof(Timestamp))]
[Index(nameof(UserId), nameof(Timestamp))]
[Index(nameof(ActionType), nameof(Timestamp))]
[Index(nameof(Timestamp))]
public class TeacherLog : BaseAuditLog
{
    /// <summary>
    /// اسم المدرس وقت العملية
    /// </summary>
    [StringLength(200)]
    public string? TeacherName { get; set; }
}

