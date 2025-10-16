using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuranPreservationSystem.Domain.Enums;

namespace QuranPreservationSystem.Domain.Entities.AuditLogs;

/// <summary>
/// Base class لجميع سجلات التدقيق
/// </summary>
public abstract class BaseAuditLog
{
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// معرف السجل الذي تمت عليه العملية
    /// </summary>
    [Required]
    public int RecordId { get; set; }

    /// <summary>
    /// نوع العملية
    /// </summary>
    [Required]
    public ActionType ActionType { get; set; }

    /// <summary>
    /// معرف المستخدم
    /// </summary>
    [StringLength(450)]
    public string? UserId { get; set; }

    /// <summary>
    /// اسم المستخدم
    /// </summary>
    [StringLength(200)]
    public string? UserName { get; set; }

    /// <summary>
    /// وقت العملية
    /// </summary>
    [Required]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// IP Address
    /// </summary>
    [StringLength(45)]
    public string? IpAddress { get; set; }

    /// <summary>
    /// البيانات القديمة (JSON)
    /// </summary>
    [Column(TypeName = "nvarchar(max)")]
    public string? OldData { get; set; }

    /// <summary>
    /// البيانات الجديدة (JSON)
    /// </summary>
    [Column(TypeName = "nvarchar(max)")]
    public string? NewData { get; set; }

    /// <summary>
    /// ملاحظات إضافية
    /// </summary>
    [StringLength(500)]
    public string? Notes { get; set; }
}

