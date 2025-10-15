using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuranPreservationSystem.Authorization;

namespace QuranPreservationSystem.Controllers
{
    /// <summary>
    /// إعدادات النظام
    /// </summary>
    [Authorize(Roles = "Admin")]
    [PermissionAuthorize("Settings", "View")]
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

