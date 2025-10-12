using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuranPreservationSystem.Controllers
{
    /// <summary>
    /// سجل النشاطات
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class LogsController : Controller
    {
        private readonly ILogger<LogsController> _logger;

        public LogsController(ILogger<LogsController> logger)
        {
            _logger = logger;
        }

        // GET: Logs
        public IActionResult Index()
        {
            return View();
        }
    }
}

