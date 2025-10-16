using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuranPreservationSystem.Domain.Entities.AuditLogs;

/// <summary>
/// سجل عمليات الطلاب
/// </summary>
[Table("StudentLogs")]
[Index(nameof(RecordId), nameof(Timestamp))]
[Index(nameof(UserId), nameof(Timestamp))]
[Index(nameof(ActionType), nameof(Timestamp))]
[Index(nameof(Timestamp))]
public class StudentLog : BaseAuditLog
{
    /// <summary>
    /// اسم الطالب وقت العملية
    /// </summary>
    [StringLength(200)]
    public string? StudentName { get; set; }
}

