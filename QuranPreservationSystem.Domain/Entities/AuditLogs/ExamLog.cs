using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuranPreservationSystem.Domain.Entities.AuditLogs;

/// <summary>
/// سجل عمليات الاختبارات
/// </summary>
[Table("ExamLogs")]
[Index(nameof(RecordId), nameof(Timestamp))]
[Index(nameof(UserId), nameof(Timestamp))]
[Index(nameof(ActionType), nameof(Timestamp))]
[Index(nameof(Timestamp))]
public class ExamLog : BaseAuditLog
{
    /// <summary>
    /// اسم الاختبار وقت العملية
    /// </summary>
    [StringLength(200)]
    public string? ExamName { get; set; }
}

