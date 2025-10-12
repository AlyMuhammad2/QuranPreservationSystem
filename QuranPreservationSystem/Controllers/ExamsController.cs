using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuranPreservationSystem.Controllers
{
    /// <summary>
    /// الاختبارات - إدارة اختبارات التجويد
    /// </summary>
    [Authorize]
    public class ExamsController : Controller
    {
        private readonly ILogger<ExamsController> _logger;

        public ExamsController(ILogger<ExamsController> logger)
        {
            _logger = logger;
        }

        // GET: Exams
        public IActionResult Index()
        {
            return View();
        }
    }
}

