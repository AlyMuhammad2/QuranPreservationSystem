using Microsoft.EntityFrameworkCore;
using QuranPreservationSystem.Application.Commands.AuditLog;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Domain.Entities.AuditLogs;
using QuranPreservationSystem.Infrastructure.Data;

namespace QuranPreservationSystem.Infrastructure.Services;

public class AuditLogService : IAuditLogService
{
    private readonly AppDbContext _context;

    public AuditLogService(AppDbContext context)
    {
        _context = context;
    }

    #region Student Logs
    public async Task LogStudentAsync(LogCommand command)
    {
        var log = new StudentLog
        {
            RecordId = command.RecordId,
            ActionType = command.ActionType,
            UserId = command.UserId,
            UserName = command.UserName,
            IpAddress = command.IpAddress,
            OldData = command.OldData,
            NewData = command.NewData,
            Notes = command.Notes,
            StudentName = command.EntityName,
            Timestamp = DateTime.UtcNow
        };

        await _context.StudentLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<StudentLog>> GetStudentLogsAsync(int studentId)
    {
        return await _context.StudentLogs
            .Where(l => l.RecordId == studentId)
            .OrderByDescending(l => l.Timestamp)
            .Take(100)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<StudentLog>> GetStudentLogsByUserAsync(string userId, int pageNumber = 1, int pageSize = 50)
    {
        return await _context.StudentLogs
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }
    #endregion

    #region Teacher Logs
    public async Task LogTeacherAsync(LogCommand command)
    {
        var log = new TeacherLog
        {
            RecordId = command.RecordId,
            ActionType = command.ActionType,
            UserId = command.UserId,
            UserName = command.UserName,
            IpAddress = command.IpAddress,
            OldData = command.OldData,
            NewData = command.NewData,
            Notes = command.Notes,
            TeacherName = command.EntityName,
            Timestamp = DateTime.UtcNow
        };

        await _context.TeacherLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TeacherLog>> GetTeacherLogsAsync(int teacherId)
    {
        return await _context.TeacherLogs
            .Where(l => l.RecordId == teacherId)
            .OrderByDescending(l => l.Timestamp)
            .Take(100)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<TeacherLog>> GetTeacherLogsByUserAsync(string userId, int pageNumber = 1, int pageSize = 50)
    {
        return await _context.TeacherLogs
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }
    #endregion

    #region Center Logs
    public async Task LogCenterAsync(LogCommand command)
    {
        var log = new CenterLog
        {
            RecordId = command.RecordId,
            ActionType = command.ActionType,
            UserId = command.UserId,
            UserName = command.UserName,
            IpAddress = command.IpAddress,
            OldData = command.OldData,
            NewData = command.NewData,
            Notes = command.Notes,
            CenterName = command.EntityName,
            Timestamp = DateTime.UtcNow
        };

        await _context.CenterLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CenterLog>> GetCenterLogsAsync(int centerId)
    {
        return await _context.CenterLogs
            .Where(l => l.RecordId == centerId)
            .OrderByDescending(l => l.Timestamp)
            .Take(100)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<CenterLog>> GetCenterLogsByUserAsync(string userId, int pageNumber = 1, int pageSize = 50)
    {
        return await _context.CenterLogs
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }
    #endregion

    #region Course Logs
    public async Task LogCourseAsync(LogCommand command)
    {
        var log = new CourseLog
        {
            RecordId = command.RecordId,
            ActionType = command.ActionType,
            UserId = command.UserId,
            UserName = command.UserName,
            IpAddress = command.IpAddress,
            OldData = command.OldData,
            NewData = command.NewData,
            Notes = command.Notes,
            CourseName = command.EntityName,
            Timestamp = DateTime.UtcNow
        };

        await _context.CourseLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CourseLog>> GetCourseLogsAsync(int courseId)
    {
        return await _context.CourseLogs
            .Where(l => l.RecordId == courseId)
            .OrderByDescending(l => l.Timestamp)
            .Take(100)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<CourseLog>> GetCourseLogsByUserAsync(string userId, int pageNumber = 1, int pageSize = 50)
    {
        return await _context.CourseLogs
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }
    #endregion

    #region Exam Logs
    public async Task LogExamAsync(LogCommand command)
    {
        var log = new ExamLog
        {
            RecordId = command.RecordId,
            ActionType = command.ActionType,
            UserId = command.UserId,
            UserName = command.UserName,
            IpAddress = command.IpAddress,
            OldData = command.OldData,
            NewData = command.NewData,
            Notes = command.Notes,
            ExamName = command.EntityName,
            Timestamp = DateTime.UtcNow
        };

        await _context.ExamLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ExamLog>> GetExamLogsAsync(int examId)
    {
        return await _context.ExamLogs
            .Where(l => l.RecordId == examId)
            .OrderByDescending(l => l.Timestamp)
            .Take(100)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<ExamLog>> GetExamLogsByUserAsync(string userId, int pageNumber = 1, int pageSize = 50)
    {
        return await _context.ExamLogs
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }
    #endregion

    #region Enrollment Logs
    public async Task LogEnrollmentAsync(LogCommand command)
    {
        var log = new EnrollmentLog
        {
            RecordId = command.RecordId,
            ActionType = command.ActionType,
            UserId = command.UserId,
            UserName = command.UserName,
            IpAddress = command.IpAddress,
            OldData = command.OldData,
            NewData = command.NewData,
            Notes = command.Notes,
            EnrollmentInfo = command.EntityName,
            Timestamp = DateTime.UtcNow
        };

        await _context.EnrollmentLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<EnrollmentLog>> GetEnrollmentLogsAsync(int enrollmentId)
    {
        return await _context.EnrollmentLogs
            .Where(l => l.RecordId == enrollmentId)
            .OrderByDescending(l => l.Timestamp)
            .Take(100)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<EnrollmentLog>> GetEnrollmentLogsByUserAsync(string userId, int pageNumber = 1, int pageSize = 50)
    {
        return await _context.EnrollmentLogs
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }
    #endregion

    #region HafizRegistry Logs
    public async Task LogHafizRegistryAsync(LogCommand command)
    {
        var log = new HafizRegistryLog
        {
            RecordId = command.RecordId,
            ActionType = command.ActionType,
            UserId = command.UserId,
            UserName = command.UserName,
            IpAddress = command.IpAddress,
            OldData = command.OldData,
            NewData = command.NewData,
            Notes = command.Notes,
            HafizName = command.EntityName,
            Timestamp = DateTime.UtcNow
        };

        await _context.HafizRegistryLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<HafizRegistryLog>> GetHafizRegistryLogsAsync(int hafizId)
    {
        return await _context.HafizRegistryLogs
            .Where(l => l.RecordId == hafizId)
            .OrderByDescending(l => l.Timestamp)
            .Take(100)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<HafizRegistryLog>> GetHafizRegistryLogsByUserAsync(string userId, int pageNumber = 1, int pageSize = 50)
    {
        return await _context.HafizRegistryLogs
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }
    #endregion

    #region User Logs
    public async Task LogUserAsync(LogCommand command)
    {
        var log = new UserLog
        {
            RecordId = command.RecordId,
            ActionType = command.ActionType,
            UserId = command.UserId,
            UserName = command.UserName,
            IpAddress = command.IpAddress,
            OldData = command.OldData,
            NewData = command.NewData,
            Notes = command.Notes,
            TargetUserName = command.EntityName,
            Timestamp = DateTime.UtcNow
        };

        await _context.UserLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserLog>> GetUserLogsAsync(string targetUserId)
    {
        if (int.TryParse(targetUserId, out int id))
        {
            return await _context.UserLogs
                .Where(l => l.RecordId == id)
                .OrderByDescending(l => l.Timestamp)
                .Take(100)
                .AsNoTracking()
                .ToListAsync();
        }
        
        return Enumerable.Empty<UserLog>();
    }

    public async Task<IEnumerable<UserLog>> GetUserLogsByUserAsync(string userId, int pageNumber = 1, int pageSize = 50)
    {
        return await _context.UserLogs
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }
    #endregion
}

