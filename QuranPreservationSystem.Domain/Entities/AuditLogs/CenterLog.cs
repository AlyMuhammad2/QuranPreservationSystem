using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuranPreservationSystem.Domain.Entities.AuditLogs;

/// <summary>
/// سجل عمليات المراكز
/// </summary>
[Table("CenterLogs")]
[Index(nameof(RecordId), nameof(Timestamp))]
[Index(nameof(UserId), nameof(Timestamp))]
[Index(nameof(ActionType), nameof(Timestamp))]
[Index(nameof(Timestamp))]
public class CenterLog : BaseAuditLog
{
    /// <summary>
    /// اسم المركز وقت العملية
    /// </summary>
    [StringLength(200)]
    public string? CenterName { get; set; }
}

