using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuranPreservationSystem.Application.DTOs;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Infrastructure.Identity;
using QuranPreservationSystem.Helpers;

namespace QuranPreservationSystem.Controllers;

/// <summary>
/// الملف الشخصي للمستخدم
/// </summary>
[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IAuditLogService auditLogService,
        ILogger<ProfileController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    // GET: Profile
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var roles = await _userManager.GetRolesAsync(user);
        var profile = new ProfileDto
        {
            Id = user.Id,
            UserName = user.UserName ?? "",
            Email = user.Email ?? "",
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            Role = roles.FirstOrDefault() ?? "لا يوجد",
            CreatedAt = user.CreatedDate
        };

        return View(profile);
    }

    // GET: Profile/Edit
    public async Task<IActionResult> Edit()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var profile = new ProfileDto
        {
            Id = user.Id,
            UserName = user.UserName ?? "",
            Email = user.Email ?? "",
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber
        };

        return View(profile);
    }

    // POST: Profile/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProfileDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // التحقق من أن اسم المستخدم والبريد الإلكتروني غير مستخدمين من قبل مستخدم آخر
            if (user.UserName != model.UserName)
            {
                var existingUser = await _userManager.FindByNameAsync(model.UserName);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    ModelState.AddModelError("UserName", "اسم المستخدم مستخدم من قبل مستخدم آخر");
                    return View(model);
                }
            }

            if (user.Email != model.Email)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    ModelState.AddModelError("Email", "البريد الإلكتروني مستخدم من قبل مستخدم آخر");
                    return View(model);
                }
            }

            // حفظ البيانات القديمة للـ Audit Log
            var oldUserData = new
            {
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber
            };

            // تحديث البيانات
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                var newUserData = new
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber
                };

                // تسجيل في Audit Log
                await _auditLogService.LogUpdateAsync(
                    User,
                    _userManager,
                    HttpContext,
                    "user",
                    int.Parse(user.Id),
                    oldUserData,
                    newUserData,
                    user.FullName
                );

                // تحديث الـ Cookie إذا تم تغيير اسم المستخدم أو البريد
                await _signInManager.RefreshSignInAsync(user);

                TempData["Success"] = "تم تحديث الملف الشخصي بنجاح";
                _logger.LogInformation("تم تحديث الملف الشخصي للمستخدم {UserId}", user.Id);
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطأ أثناء تحديث الملف الشخصي");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء تحديث الملف الشخصي");
        }

        return View(model);
    }

    // GET: Profile/ChangePassword
    public IActionResult ChangePassword()
    {
        return View();
    }

    // POST: Profile/ChangePassword
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                // تسجيل في Audit Log
                await _auditLogService.LogUpdateAsync(
                    User,
                    _userManager,
                    HttpContext,
                    "user",
                    int.Parse(user.Id),
                    new { Action = "تغيير كلمة المرور" },
                    new { Action = "تم تغيير كلمة المرور بنجاح" },
                    user.FullName
                );

                await _signInManager.RefreshSignInAsync(user);
                TempData["Success"] = "تم تغيير كلمة المرور بنجاح";
                _logger.LogInformation("تم تغيير كلمة المرور للمستخدم {UserId}", user.Id);
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                if (error.Code == "PasswordMismatch")
                {
                    ModelState.AddModelError("CurrentPassword", "كلمة المرور الحالية غير صحيحة");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطأ أثناء تغيير كلمة المرور");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء تغيير كلمة المرور");
        }

        return View(model);
    }
}

