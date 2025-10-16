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

        #region Helper Methods
        /// <summary>
        /// الحصول على التواريخ الافتراضية (من أول الشهر الحالي إلى اليوم)
        /// </summary>
        private (DateTime startDate, DateTime endDate) GetDefaultDates()
        {
            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            return (firstDayOfMonth, today);
        }

        /// <summary>
        /// تحضير ViewBag مع التواريخ
        /// </summary>
        private void PrepareViewBagDates(DateTime? startDate, DateTime? endDate, int pageNumber)
        {
            var (defaultStart, defaultEnd) = GetDefaultDates();
            
            ViewBag.StartDate = (startDate ?? defaultStart).ToString("yyyy-MM-dd");
            ViewBag.EndDate = (endDate ?? defaultEnd).ToString("yyyy-MM-dd");
            ViewBag.PageNumber = pageNumber;
        }
        #endregion

        #region Student Logs
        // GET: Logs/Students
        public async Task<IActionResult> Students(DateTime? startDate, DateTime? endDate, int pageNumber = 1)
        {
            try
            {
                // استخدام التواريخ الافتراضية إذا لم يتم تحديدها
                var (defaultStart, defaultEnd) = GetDefaultDates();
                var effectiveStartDate = startDate ?? defaultStart;
                var effectiveEndDate = endDate ?? defaultEnd;

                var logs = await _auditLogService.GetAllStudentLogsAsync(effectiveStartDate, effectiveEndDate, pageNumber, 50);
                
                var studentLogs = logs.Select(l => new StudentLogDto
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
                
                PrepareViewBagDates(effectiveStartDate, effectiveEndDate, pageNumber);
                
                return View(studentLogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع سجلات الطلاب");
                TempData["Error"] = "حدث خطأ أثناء استرجاع السجلات";
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion

        #region Teacher Logs
        // GET: Logs/Teachers
        public async Task<IActionResult> Teachers(DateTime? startDate, DateTime? endDate, int pageNumber = 1)
        {
            try
            {
                var (defaultStart, defaultEnd) = GetDefaultDates();
                var effectiveStartDate = startDate ?? defaultStart;
                var effectiveEndDate = endDate ?? defaultEnd;

                var logs = await _auditLogService.GetAllTeacherLogsAsync(effectiveStartDate, effectiveEndDate, pageNumber, 50);
                
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
                
                PrepareViewBagDates(effectiveStartDate, effectiveEndDate, pageNumber);
                
                return View(teacherLogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع سجلات المدرسين");
                TempData["Error"] = "حدث خطأ أثناء استرجاع السجلات";
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion

        #region Center Logs
        // GET: Logs/Centers
        public async Task<IActionResult> Centers(DateTime? startDate, DateTime? endDate, int pageNumber = 1)
        {
            try
            {
                var (defaultStart, defaultEnd) = GetDefaultDates();
                var effectiveStartDate = startDate ?? defaultStart;
                var effectiveEndDate = endDate ?? defaultEnd;

                var logs = await _auditLogService.GetAllCenterLogsAsync(effectiveStartDate, effectiveEndDate, pageNumber, 50);
                
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
                
                PrepareViewBagDates(effectiveStartDate, effectiveEndDate, pageNumber);
                
                return View(centerLogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع سجلات المراكز");
                TempData["Error"] = "حدث خطأ أثناء استرجاع السجلات";
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion

        #region Course Logs
        // GET: Logs/Courses
        public async Task<IActionResult> Courses(DateTime? startDate, DateTime? endDate, int pageNumber = 1)
        {
            try
            {
                var (defaultStart, defaultEnd) = GetDefaultDates();
                var effectiveStartDate = startDate ?? defaultStart;
                var effectiveEndDate = endDate ?? defaultEnd;

                var logs = await _auditLogService.GetAllCourseLogsAsync(effectiveStartDate, effectiveEndDate, pageNumber, 50);
                
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
                
                PrepareViewBagDates(effectiveStartDate, effectiveEndDate, pageNumber);
                
                return View(courseLogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع سجلات الدورات");
                TempData["Error"] = "حدث خطأ أثناء استرجاع السجلات";
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion

        #region Exam Logs
        // GET: Logs/Exams
        public async Task<IActionResult> Exams(DateTime? startDate, DateTime? endDate, int pageNumber = 1)
        {
            try
            {
                var (defaultStart, defaultEnd) = GetDefaultDates();
                var effectiveStartDate = startDate ?? defaultStart;
                var effectiveEndDate = endDate ?? defaultEnd;

                var logs = await _auditLogService.GetAllExamLogsAsync(effectiveStartDate, effectiveEndDate, pageNumber, 50);
                
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
                
                PrepareViewBagDates(effectiveStartDate, effectiveEndDate, pageNumber);
                
                return View(examLogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع سجلات الاختبارات");
                TempData["Error"] = "حدث خطأ أثناء استرجاع السجلات";
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion

        #region Enrollment Logs
        // GET: Logs/Enrollments
        public async Task<IActionResult> Enrollments(DateTime? startDate, DateTime? endDate, int pageNumber = 1)
        {
            try
            {
                var (defaultStart, defaultEnd) = GetDefaultDates();
                var effectiveStartDate = startDate ?? defaultStart;
                var effectiveEndDate = endDate ?? defaultEnd;

                var logs = await _auditLogService.GetAllEnrollmentLogsAsync(effectiveStartDate, effectiveEndDate, pageNumber, 50);
                
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
                
                PrepareViewBagDates(effectiveStartDate, effectiveEndDate, pageNumber);
                
                return View(enrollmentLogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع سجلات التسجيلات");
                TempData["Error"] = "حدث خطأ أثناء استرجاع السجلات";
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion

        #region HafizRegistry Logs
        // GET: Logs/HafizRegistry
        public async Task<IActionResult> HafizRegistry(DateTime? startDate, DateTime? endDate, int pageNumber = 1)
        {
            try
            {
                var (defaultStart, defaultEnd) = GetDefaultDates();
                var effectiveStartDate = startDate ?? defaultStart;
                var effectiveEndDate = endDate ?? defaultEnd;

                var logs = await _auditLogService.GetAllHafizRegistryLogsAsync(effectiveStartDate, effectiveEndDate, pageNumber, 50);
                
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
                
                PrepareViewBagDates(effectiveStartDate, effectiveEndDate, pageNumber);
                
                return View(hafizLogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع سجلات ديوان الحفاظ");
                TempData["Error"] = "حدث خطأ أثناء استرجاع السجلات";
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion

        #region User Logs
        // GET: Logs/Users
        public async Task<IActionResult> Users(DateTime? startDate, DateTime? endDate, int pageNumber = 1)
        {
            try
            {
                var (defaultStart, defaultEnd) = GetDefaultDates();
                var effectiveStartDate = startDate ?? defaultStart;
                var effectiveEndDate = endDate ?? defaultEnd;

                var logs = await _auditLogService.GetAllUserLogsAsync(effectiveStartDate, effectiveEndDate, pageNumber, 50);
                
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
                
                PrepareViewBagDates(effectiveStartDate, effectiveEndDate, pageNumber);
                
                return View(userLogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع سجلات المستخدمين");
                TempData["Error"] = "حدث خطأ أثناء استرجاع السجلات";
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion
    }
}
