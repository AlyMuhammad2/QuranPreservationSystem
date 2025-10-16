using QuranPreservationSystem.Domain.Enums;

namespace QuranPreservationSystem.Application.Commands.AuditLog;

/// <summary>
/// Command لتسجيل عملية في السجل
/// </summary>
public class LogCommand
{
    public int RecordId { get; set; }
    public ActionType ActionType { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? IpAddress { get; set; }
    public string? OldData { get; set; }
    public string? NewData { get; set; }
    public string? Notes { get; set; }
    public string? EntityName { get; set; } // اسم الكيان (Student, Teacher, etc.)
}

