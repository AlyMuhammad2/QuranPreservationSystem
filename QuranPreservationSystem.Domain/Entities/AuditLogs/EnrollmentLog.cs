using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuranPreservationSystem.Domain.Entities.AuditLogs;

/// <summary>
/// سجل عمليات التسجيلات
/// </summary>
[Table("EnrollmentLogs")]
[Index(nameof(RecordId), nameof(Timestamp))]
[Index(nameof(UserId), nameof(Timestamp))]
[Index(nameof(ActionType), nameof(Timestamp))]
[Index(nameof(Timestamp))]
public class EnrollmentLog : BaseAuditLog
{
    /// <summary>
    /// معلومات التسجيل (اسم الطالب والدورة)
    /// </summary>
    [StringLength(400)]
    public string? EnrollmentInfo { get; set; }
}

