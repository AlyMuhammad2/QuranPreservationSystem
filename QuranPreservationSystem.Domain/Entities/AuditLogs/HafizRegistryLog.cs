using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuranPreservationSystem.Domain.Entities.AuditLogs;

/// <summary>
/// سجل عمليات ديوان الحفاظ
/// </summary>
[Table("HafizRegistryLogs")]
[Microsoft.EntityFrameworkCore.Index(nameof(RecordId), nameof(Timestamp))]
[Microsoft.EntityFrameworkCore.Index(nameof(UserId), nameof(Timestamp))]
[Microsoft.EntityFrameworkCore.Index(nameof(ActionType), nameof(Timestamp))]
[Microsoft.EntityFrameworkCore.Index(nameof(Timestamp))]
public class HafizRegistryLog : BaseAuditLog
{
    /// <summary>
    /// اسم الحافظ وقت العملية
    /// </summary>
    [StringLength(200)]
    public string? HafizName { get; set; }
}

