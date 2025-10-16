using QuranPreservationSystem.Application.Commands.AuditLog;

namespace QuranPreservationSystem.Application.Interfaces;

/// <summary>
/// خدمة سجلات التدقيق
/// </summary>
public interface IAuditLogService
{
    // Student Logs
    Task LogStudentAsync(LogCommand command);
    Task<IEnumerable<Domain.Entities.AuditLogs.StudentLog>> GetStudentLogsAsync(int studentId);
    Task<IEnumerable<Domain.Entities.AuditLogs.StudentLog>> GetStudentLogsByUserAsync(string userId, int pageNumber = 1, int pageSize = 50);

    // Teacher Logs
    Task LogTeacherAsync(LogCommand command);
    Task<IEnumerable<Domain.Entities.AuditLogs.TeacherLog>> GetTeacherLogsAsync(int teacherId);
    Task<IEnumerable<Domain.Entities.AuditLogs.TeacherLog>> GetTeacherLogsByUserAsync(string userId, int pageNumber = 1, int pageSize = 50);

    // Center Logs
    Task LogCenterAsync(LogCommand command);
    Task<IEnumerable<Domain.Entities.AuditLogs.CenterLog>> GetCenterLogsAsync(int centerId);
    Task<IEnumerable<Domain.Entities.AuditLogs.CenterLog>> GetCenterLogsByUserAsync(string userId, int pageNumber = 1, int pageSize = 50);

    // Course Logs
    Task LogCourseAsync(LogCommand command);
    Task<IEnumerable<Domain.Entities.AuditLogs.CourseLog>> GetCourseLogsAsync(int courseId);
    Task<IEnumerable<Domain.Entities.AuditLogs.CourseLog>> GetCourseLogsByUserAsync(string userId, int pageNumber = 1, int pageSize = 50);

    // Exam Logs
    Task LogExamAsync(LogCommand command);
    Task<IEnumerable<Domain.Entities.AuditLogs.ExamLog>> GetExamLogsAsync(int examId);
    Task<IEnumerable<Domain.Entities.AuditLogs.ExamLog>> GetExamLogsByUserAsync(string userId, int pageNumber = 1, int pageSize = 50);

    // Enrollment Logs
    Task LogEnrollmentAsync(LogCommand command);
    Task<IEnumerable<Domain.Entities.AuditLogs.EnrollmentLog>> GetEnrollmentLogsAsync(int enrollmentId);
    Task<IEnumerable<Domain.Entities.AuditLogs.EnrollmentLog>> GetEnrollmentLogsByUserAsync(string userId, int pageNumber = 1, int pageSize = 50);

    // HafizRegistry Logs
    Task LogHafizRegistryAsync(LogCommand command);
    Task<IEnumerable<Domain.Entities.AuditLogs.HafizRegistryLog>> GetHafizRegistryLogsAsync(int hafizId);
    Task<IEnumerable<Domain.Entities.AuditLogs.HafizRegistryLog>> GetHafizRegistryLogsByUserAsync(string userId, int pageNumber = 1, int pageSize = 50);

    // User Logs
    Task LogUserAsync(LogCommand command);
    Task<IEnumerable<Domain.Entities.AuditLogs.UserLog>> GetUserLogsAsync(string targetUserId);
    Task<IEnumerable<Domain.Entities.AuditLogs.UserLog>> GetUserLogsByUserAsync(string userId, int pageNumber = 1, int pageSize = 50);
}

