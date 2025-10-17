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

    // POST: Profile/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProfileDto model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "يرجى التأكد من صحة البيانات المدخلة";
            return RedirectToAction(nameof(Index));
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
                    TempData["Error"] = "اسم المستخدم مستخدم من قبل مستخدم آخر";
                    return RedirectToAction(nameof(Index));
                }
            }

            if (user.Email != model.Email)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    TempData["Error"] = "البريد الإلكتروني مستخدم من قبل مستخدم آخر";
                    return RedirectToAction(nameof(Index));
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
                try
                {
                    var newUserData = new
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        FullName = user.FullName,
                        PhoneNumber = user.PhoneNumber
                    };

                    // تسجيل في Audit Log
                    if (int.TryParse(user.Id, out int userId))
                    {
                        await _auditLogService.LogUpdateAsync(
                            User,
                            _userManager,
                            HttpContext,
                            "user",
                            userId,
                            oldUserData,
                            newUserData,
                            user.FullName
                        );
                    }
                }
                catch (Exception logEx)
                {
                    _logger.LogWarning(logEx, "فشل تسجيل تحديث الملف الشخصي في Audit Log");
                }

                // تحديث الـ Cookie إذا تم تغيير اسم المستخدم أو البريد
                await _signInManager.RefreshSignInAsync(user);

                TempData["Success"] = "تم تحديث الملف الشخصي بنجاح";
                _logger.LogInformation("تم تحديث الملف الشخصي للمستخدم {UserId}", user.Id);
                return RedirectToAction(nameof(Index));
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            TempData["Error"] = errors;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطأ أثناء تحديث الملف الشخصي");
            TempData["Error"] = "حدث خطأ أثناء تحديث الملف الشخصي";
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: Profile/ChangePassword
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "يرجى التأكد من صحة البيانات المدخلة";
            return RedirectToAction(nameof(Index));
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
                try
                {
                    // تسجيل في Audit Log
                    if (int.TryParse(user.Id, out int userId))
                    {
                        await _auditLogService.LogUpdateAsync(
                            User,
                            _userManager,
                            HttpContext,
                            "user",
                            userId,
                            new { Action = "تغيير كلمة المرور" },
                            new { Action = "تم تغيير كلمة المرور بنجاح" },
                            user.FullName
                        );
                    }
                }
                catch (Exception logEx)
                {
                    _logger.LogWarning(logEx, "فشل تسجيل تغيير كلمة المرور في Audit Log");
                }

                await _signInManager.RefreshSignInAsync(user);
                TempData["Success"] = "تم تغيير كلمة المرور بنجاح";
                _logger.LogInformation("تم تغيير كلمة المرور للمستخدم {UserId}", user.Id);
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                if (error.Code == "PasswordMismatch")
                {
                    TempData["Error"] = "كلمة المرور الحالية غير صحيحة";
                }
                else
                {
                    TempData["Error"] = error.Description;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطأ أثناء تغيير كلمة المرور");
            TempData["Error"] = "حدث خطأ أثناء تغيير كلمة المرور";
        }

        return RedirectToAction(nameof(Index));
    }
}

