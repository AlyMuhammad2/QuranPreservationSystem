using QuranPreservationSystem.Domain.Enums;

namespace QuranPreservationSystem.Application.DTOs;

public class LogFilterDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public string? SearchTerm { get; set; }
    public ActionType? ActionType { get; set; }
}

public class StudentLogDto
{
    public long Id { get; set; }
    public int RecordId { get; set; }
    public string? StudentName { get; set; }
    public ActionType ActionType { get; set; }
    public string ActionTypeDisplay { get; set; } = "";
    public string? UserName { get; set; }
    public DateTime Timestamp { get; set; }
    public string? IpAddress { get; set; }
    public string? OldData { get; set; }
    public string? NewData { get; set; }
    public string? Notes { get; set; }
}

public class TeacherLogDto
{
    public long Id { get; set; }
    public int RecordId { get; set; }
    public string? TeacherName { get; set; }
    public ActionType ActionType { get; set; }
    public string ActionTypeDisplay { get; set; } = "";
    public string? UserName { get; set; }
    public DateTime Timestamp { get; set; }
    public string? IpAddress { get; set; }
    public string? OldData { get; set; }
    public string? NewData { get; set; }
    public string? Notes { get; set; }
}

public class CenterLogDto
{
    public long Id { get; set; }
    public int RecordId { get; set; }
    public string? CenterName { get; set; }
    public ActionType ActionType { get; set; }
    public string ActionTypeDisplay { get; set; } = "";
    public string? UserName { get; set; }
    public DateTime Timestamp { get; set; }
    public string? IpAddress { get; set; }
    public string? OldData { get; set; }
    public string? NewData { get; set; }
    public string? Notes { get; set; }
}

public class CourseLogDto
{
    public long Id { get; set; }
    public int RecordId { get; set; }
    public string? CourseName { get; set; }
    public ActionType ActionType { get; set; }
    public string ActionTypeDisplay { get; set; } = "";
    public string? UserName { get; set; }
    public DateTime Timestamp { get; set; }
    public string? IpAddress { get; set; }
    public string? OldData { get; set; }
    public string? NewData { get; set; }
    public string? Notes { get; set; }
}

public class ExamLogDto
{
    public long Id { get; set; }
    public int RecordId { get; set; }
    public string? ExamName { get; set; }
    public ActionType ActionType { get; set; }
    public string ActionTypeDisplay { get; set; } = "";
    public string? UserName { get; set; }
    public DateTime Timestamp { get; set; }
    public string? IpAddress { get; set; }
    public string? OldData { get; set; }
    public string? NewData { get; set; }
    public string? Notes { get; set; }
}

public class EnrollmentLogDto
{
    public long Id { get; set; }
    public int RecordId { get; set; }
    public string? EnrollmentInfo { get; set; }
    public ActionType ActionType { get; set; }
    public string ActionTypeDisplay { get; set; } = "";
    public string? UserName { get; set; }
    public DateTime Timestamp { get; set; }
    public string? IpAddress { get; set; }
    public string? OldData { get; set; }
    public string? NewData { get; set; }
    public string? Notes { get; set; }
}

public class HafizRegistryLogDto
{
    public long Id { get; set; }
    public int RecordId { get; set; }
    public string? HafizName { get; set; }
    public ActionType ActionType { get; set; }
    public string ActionTypeDisplay { get; set; } = "";
    public string? UserName { get; set; }
    public DateTime Timestamp { get; set; }
    public string? IpAddress { get; set; }
    public string? OldData { get; set; }
    public string? NewData { get; set; }
    public string? Notes { get; set; }
}

public class UserLogDto
{
    public long Id { get; set; }
    public int RecordId { get; set; }
    public string? TargetUserName { get; set; }
    public ActionType ActionType { get; set; }
    public string ActionTypeDisplay { get; set; } = "";
    public string? UserName { get; set; }
    public DateTime Timestamp { get; set; }
    public string? IpAddress { get; set; }
    public string? OldData { get; set; }
    public string? NewData { get; set; }
    public string? Notes { get; set; }
}

