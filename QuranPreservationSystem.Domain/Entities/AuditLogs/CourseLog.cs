using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuranPreservationSystem.Domain.Entities.AuditLogs;

/// <summary>
/// سجل عمليات الدورات
/// </summary>
[Table("CourseLogs")]
[Index(nameof(RecordId), nameof(Timestamp))]
[Index(nameof(UserId), nameof(Timestamp))]
[Index(nameof(ActionType), nameof(Timestamp))]
[Index(nameof(Timestamp))]
public class CourseLog : BaseAuditLog
{
    /// <summary>
    /// اسم الدورة وقت العملية
    /// </summary>
    [StringLength(200)]
    public string? CourseName { get; set; }
}

