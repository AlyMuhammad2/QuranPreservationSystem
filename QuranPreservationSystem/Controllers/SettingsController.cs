using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuranPreservationSystem.Controllers
{
    /// <summary>
    /// إعدادات النظام
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class SettingsController : Controller
    {
        private readonly ILogger<SettingsController> _logger;

        public SettingsController(ILogger<SettingsController> logger)
        {
            _logger = logger;
        }

        // GET: Settings
        public IActionResult Index()
        {
            return View();
        }
    }
}

