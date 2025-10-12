using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuranPreservationSystem.Application.DTOs;
using QuranPreservationSystem.Infrastructure.Identity;

namespace QuranPreservationSystem.Controllers
{
    /// <summary>
    /// Account Controller - يدير عمليات تسجيل الدخول والخروج
    /// </summary>
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// صفحة تسجيل الدخول - GET
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            // إذا كان المستخدم مسجل دخول بالفعل، توجيهه للصفحة الرئيسية
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        /// <summary>
        /// تسجيل الدخول - POST
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ViewData["ReturnUrl"] = model.ReturnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // محاولة تسجيل الدخول
            var result = await _signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                // تحديث آخر تسجيل دخول
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    user.LastLoginDate = DateTime.Now;
                    await _userManager.UpdateAsync(user);
                }

                _logger.LogInformation("User {UserName} logged in successfully.", model.UserName);

                // التوجيه
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return RedirectToAction("Index", "Dashboard");
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account {UserName} locked out.", model.UserName);
                ModelState.AddModelError(string.Empty, "الحساب محظور بسبب محاولات دخول فاشلة متكررة. حاول مرة أخرى بعد 5 دقائق.");
                return View(model);
            }

            if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "تسجيل الدخول غير مسموح. يرجى تأكيد البريد الإلكتروني.");
                return View(model);
            }

            // فشل تسجيل الدخول
            ModelState.AddModelError(string.Empty, "اسم المستخدم أو كلمة المرور غير صحيحة.");
            return View(model);
        }

        /// <summary>
        /// تسجيل الخروج
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// صفحة عدم السماح بالوصول
        /// </summary>
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        /// <summary>
        /// الحصول على معلومات المستخدم الحالي (للـ API)
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "المستخدم غير موجود" });
            }

            var roles = await _userManager.GetRolesAsync(user);

            return Json(new
            {
                success = true,
                user = new
                {
                    userName = user.UserName,
                    fullName = user.FullName,
                    email = user.Email,
                    roles = roles,
                    isActive = user.IsActive
                }
            });
        }
    }
}

