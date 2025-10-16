using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuranPreservationSystem.Application.DTOs;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Authorization;
using QuranPreservationSystem.Domain.Enums;
using QuranPreservationSystem.Helpers;

namespace QuranPreservationSystem.Controllers
{
    /// <summary>
    /// سجل النشاطات
    /// </summary>
    [Authorize(Roles = "Admin")]
    [PermissionAuthorize("Logs", "View")]
    public class LogsController : Controller
    {
        private readonly IAuditLogService _auditLogService;
        private readonly ILogger<LogsController> _logger;

        public LogsController(
            IAuditLogService auditLogService,
            ILogger<LogsController> logger)
        {
            _auditLogService = auditLogService;
            _logger = logger;
        }

        // GET: Logs
        public IActionResult Index()
        {
            return View();
        }

        #region Student Logs
        // GET: Logs/Students
        public async Task<IActionResult> Students(DateTime? startDate, DateTime? endDate, int pageNumber = 1)
        {
            var logs = await GetStudentLogsAsync(startDate, endDate, pageNumber);
            
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = 10; // سيتم حسابها لاحقاً
            
            return View(logs);
        }

        private async Task<List<StudentLogDto>> GetStudentLogsAsync(DateTime? startDate, DateTime? endDate, int pageNumber)
        {
            // هنا سنضيف فلترة بالتاريخ لاحقاً
            var logs = await _auditLogService.GetStudentLogsByUserAsync("", pageNumber, 50);
            
            return logs.Select(l => new StudentLogDto
            {
                Id = l.Id,
                RecordId = l.RecordId,
                StudentName = l.StudentName,
                ActionType = l.ActionType,
                ActionTypeDisplay = l.ActionType.GetDisplayName(),
                UserName = l.UserName,
                Timestamp = l.Timestamp,
                IpAddress = l.IpAddress,
                OldData = l.OldData,
                NewData = l.NewData,
                Notes = l.Notes
            }).ToList();
        }
        #endregion

        #region Teacher Logs
        // GET: Logs/Teachers
        public async Task<IActionResult> Teachers(DateTime? startDate, DateTime? endDate, int pageNumber = 1)
        {
            var logs = await _auditLogService.GetTeacherLogsByUserAsync("", pageNumber, 50);
            
            var teacherLogs = logs.Select(l => new TeacherLogDto
            {
                Id = l.Id,
                RecordId = l.RecordId,
                TeacherName = l.TeacherName,
                ActionType = l.ActionType,
                ActionTypeDisplay = l.ActionType.GetDisplayName(),
                UserName = l.UserName,
                Timestamp = l.Timestamp,
                IpAddress = l.IpAddress,
                OldData = l.OldData,
                NewData = l.NewData,
                Notes = l.Notes
            }).ToList();
            
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.PageNumber = pageNumber;
            
            return View(teacherLogs);
        }
        #endregion

        #region Center Logs
        // GET: Logs/Centers
        public async Task<IActionResult> Centers(DateTime? startDate, DateTime? endDate, int pageNumber = 1)
        {
            var logs = await _auditLogService.GetCenterLogsByUserAsync("", pageNumber, 50);
            
            var centerLogs = logs.Select(l => new CenterLogDto
            {
                Id = l.Id,
                RecordId = l.RecordId,
                CenterName = l.CenterName,
                ActionType = l.ActionType,
                ActionTypeDisplay = l.ActionType.GetDisplayName(),
                UserName = l.UserName,
                Timestamp = l.Timestamp,
                IpAddress = l.IpAddress,
                OldData = l.OldData,
                NewData = l.NewData,
                Notes = l.Notes
            }).ToList();
            
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.PageNumber = pageNumber;
            
            return View(centerLogs);
        }
        #endregion

        #region Course Logs
        // GET: Logs/Courses
        public async Task<IActionResult> Courses(DateTime? startDate, DateTime? endDate, int pageNumber = 1)
        {
            var logs = await _auditLogService.GetCourseLogsByUserAsync("", pageNumber, 50);
            
            var courseLogs = logs.Select(l => new CourseLogDto
            {
                Id = l.Id,
                RecordId = l.RecordId,
                CourseName = l.CourseName,
                ActionType = l.ActionType,
                ActionTypeDisplay = l.ActionType.GetDisplayName(),
                UserName = l.UserName,
                Timestamp = l.Timestamp,
                IpAddress = l.IpAddress,
                OldData = l.OldData,
                NewData = l.NewData,
                Notes = l.Notes
            }).ToList();
            
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.PageNumber = pageNumber;
            
            return View(courseLogs);
        }
        #endregion

        #region Exam Logs
        // GET: Logs/Exams
        public async Task<IActionResult> Exams(DateTime? startDate, DateTime? endDate, int pageNumber = 1)
        {
            var logs = await _auditLogService.GetExamLogsByUserAsync("", pageNumber, 50);
            
            var examLogs = logs.Select(l => new ExamLogDto
            {
                Id = l.Id,
                RecordId = l.RecordId,
                ExamName = l.ExamName,
                ActionType = l.ActionType,
                ActionTypeDisplay = l.ActionType.GetDisplayName(),
                UserName = l.UserName,
                Timestamp = l.Timestamp,
                IpAddress = l.IpAddress,
                OldData = l.OldData,
                NewData = l.NewData,
                Notes = l.Notes
            }).ToList();
            
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.PageNumber = pageNumber;
            
            return View(examLogs);
        }
        #endregion

        #region Enrollment Logs
        // GET: Logs/Enrollments
        public async Task<IActionResult> Enrollments(DateTime? startDate, DateTime? endDate, int pageNumber = 1)
        {
            var logs = await _auditLogService.GetEnrollmentLogsByUserAsync("", pageNumber, 50);
            
            var enrollmentLogs = logs.Select(l => new EnrollmentLogDto
            {
                Id = l.Id,
                RecordId = l.RecordId,
                EnrollmentInfo = l.EnrollmentInfo,
                ActionType = l.ActionType,
                ActionTypeDisplay = l.ActionType.GetDisplayName(),
                UserName = l.UserName,
                Timestamp = l.Timestamp,
                IpAddress = l.IpAddress,
                OldData = l.OldData,
                NewData = l.NewData,
                Notes = l.Notes
            }).ToList();
            
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.PageNumber = pageNumber;
            
            return View(enrollmentLogs);
        }
        #endregion

        #region HafizRegistry Logs
        // GET: Logs/HafizRegistry
        public async Task<IActionResult> HafizRegistry(DateTime? startDate, DateTime? endDate, int pageNumber = 1)
        {
            var logs = await _auditLogService.GetHafizRegistryLogsByUserAsync("", pageNumber, 50);
            
            var hafizLogs = logs.Select(l => new HafizRegistryLogDto
            {
                Id = l.Id,
                RecordId = l.RecordId,
                HafizName = l.HafizName,
                ActionType = l.ActionType,
                ActionTypeDisplay = l.ActionType.GetDisplayName(),
                UserName = l.UserName,
                Timestamp = l.Timestamp,
                IpAddress = l.IpAddress,
                OldData = l.OldData,
                NewData = l.NewData,
                Notes = l.Notes
            }).ToList();
            
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.PageNumber = pageNumber;
            
            return View(hafizLogs);
        }
        #endregion

        #region User Logs
        // GET: Logs/Users
        public async Task<IActionResult> Users(DateTime? startDate, DateTime? endDate, int pageNumber = 1)
        {
            var logs = await _auditLogService.GetUserLogsByUserAsync("", pageNumber, 50);
            
            var userLogs = logs.Select(l => new UserLogDto
            {
                Id = l.Id,
                RecordId = l.RecordId,
                TargetUserName = l.TargetUserName,
                ActionType = l.ActionType,
                ActionTypeDisplay = l.ActionType.GetDisplayName(),
                UserName = l.UserName,
                Timestamp = l.Timestamp,
                IpAddress = l.IpAddress,
                OldData = l.OldData,
                NewData = l.NewData,
                Notes = l.Notes
            }).ToList();
            
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.PageNumber = pageNumber;
            
            return View(userLogs);
        }
        #endregion
    }
}

