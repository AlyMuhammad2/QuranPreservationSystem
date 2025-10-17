using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuranPreservationSystem.Application.DTOs;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Helpers;
using QuranPreservationSystem.Infrastructure.Identity;

namespace QuranPreservationSystem.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UsersController> _logger;
    private readonly IAuditLogService _auditLogService;

    public UsersController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IUnitOfWork unitOfWork,
        ILogger<UsersController> logger,
        IAuditLogService auditLogService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _auditLogService = auditLogService;
    }

    // GET: Users
    public async Task<IActionResult> Index(string searchTerm = "")
    {
        var users = await _userManager.Users
            .Where(u => string.IsNullOrEmpty(searchTerm) ||
                       u.FullName.Contains(searchTerm) ||
                       u.UserName.Contains(searchTerm) ||
                       u.Email.Contains(searchTerm))
            .OrderBy(u => u.FullName)
            .ToListAsync();

        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "";

            userDtos.Add(new UserDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                PhoneNumber = user.PhoneNumber,
                Role = role,
                RoleDisplayName = GetRoleDisplayName(role),
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate,
                LastLoginDate = user.LastLoginDate
            });
        }

        ViewBag.SearchTerm = searchTerm;
        ViewBag.TotalUsers = userDtos.Count;
        ViewBag.ActiveUsers = userDtos.Count(u => u.IsActive);
        ViewBag.InactiveUsers = userDtos.Count(u => !u.IsActive);
        ViewBag.AdminUsers = userDtos.Count(u => u.Role == "Admin");
        ViewBag.TeacherUsers = userDtos.Count(u => u.Role == "Teacher");

        return View(userDtos);
    }

    // GET: Users/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (string.IsNullOrEmpty(id))
            return NotFound();

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "";

        var userDto = new UserDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            UserName = user.UserName ?? "",
            Email = user.Email ?? "",
            PhoneNumber = user.PhoneNumber,
            Role = role,
            RoleDisplayName = GetRoleDisplayName(role),
            IsActive = user.IsActive,
            CreatedDate = user.CreatedDate,
            LastLoginDate = user.LastLoginDate
        };

        return View(userDto);
    }

    // GET: Users/Create
    public IActionResult Create()
    {
        ViewBag.Roles = GetRolesList();
        return View();
    }

    // POST: Users/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserDto model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Roles = GetRolesList();
            return View(model);
        }

        // التحقق من عدم وجود المستخدم مسبقاً
        var existingUser = await _userManager.FindByNameAsync(model.UserName);
        if (existingUser != null)
        {
            ModelState.AddModelError("UserName", "اسم المستخدم موجود مسبقاً");
            ViewBag.Roles = GetRolesList();
            return View(model);
        }

        var existingEmail = await _userManager.FindByEmailAsync(model.Email);
        if (existingEmail != null)
        {
            ModelState.AddModelError("Email", "البريد الإلكتروني موجود مسبقاً");
            ViewBag.Roles = GetRolesList();
            return View(model);
        }

        var user = new ApplicationUser
        {
            FullName = model.FullName,
            UserName = model.UserName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            IsActive = model.IsActive,
            CreatedDate = DateTime.Now
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            // إضافة الدور للمستخدم
            await _userManager.AddToRoleAsync(user, model.Role);

            await _auditLogService.LogCreateAsync(User, _userManager, HttpContext, "user", 0, user, user.FullName);

            TempData["Success"] = $"تم إضافة المستخدم '{user.FullName}' بنجاح";
            _logger.LogInformation($"User {user.UserName} created successfully with role {model.Role}");
            return RedirectToAction(nameof(Index));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        ViewBag.Roles = GetRolesList();
        return View(model);
    }

    // GET: Users/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (string.IsNullOrEmpty(id))
            return NotFound();

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "";

        var model = new EditUserDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            UserName = user.UserName ?? "",
            Email = user.Email ?? "",
            PhoneNumber = user.PhoneNumber,
            Role = role,
            IsActive = user.IsActive
        };

        ViewBag.Roles = GetRolesList();
        return View(model);
    }

    // POST: Users/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditUserDto model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Roles = GetRolesList();
            return View(model);
        }

        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null)
            return NotFound();

        var oldUser = new ApplicationUser { Id = user.Id, FullName = user.FullName, UserName = user.UserName, Email = user.Email, IsActive = user.IsActive };

        // التحقق من عدم تكرار اسم المستخدم
        var existingUser = await _userManager.Users
            .Where(u => u.UserName == model.UserName && u.Id != model.UserId)
            .FirstOrDefaultAsync();

        if (existingUser != null)
        {
            ModelState.AddModelError("UserName", "اسم المستخدم موجود مسبقاً");
            ViewBag.Roles = GetRolesList();
            return View(model);
        }

        // التحقق من عدم تكرار البريد
        var existingEmail = await _userManager.Users
            .Where(u => u.Email == model.Email && u.Id != model.UserId)
            .FirstOrDefaultAsync();

        if (existingEmail != null)
        {
            ModelState.AddModelError("Email", "البريد الإلكتروني موجود مسبقاً");
            ViewBag.Roles = GetRolesList();
            return View(model);
        }

        // تحديث البيانات
        user.FullName = model.FullName;
        user.UserName = model.UserName;
        user.Email = model.Email;
        user.PhoneNumber = model.PhoneNumber;
        user.IsActive = model.IsActive;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            // تحديث الدور
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Count > 0 && currentRoles.First() != model.Role)
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, model.Role);
            }
            else if (currentRoles.Count == 0)
            {
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            // تحديث كلمة المرور إذا تم إدخال واحدة جديدة
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                    {
                        ModelState.AddModelError("NewPassword", error.Description);
                    }
                    ViewBag.Roles = GetRolesList();
                    return View(model);
                }
            }

            await _auditLogService.LogUpdateAsync(User, _userManager, HttpContext, "user", 0, oldUser, user, user.FullName);

            TempData["Success"] = $"تم تحديث بيانات المستخدم '{user.FullName}' بنجاح";
            _logger.LogInformation($"User {user.UserName} updated successfully");
            return RedirectToAction(nameof(Index));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        ViewBag.Roles = GetRolesList();
        return View(model);
    }

    // GET: Users/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrEmpty(id))
            return NotFound();

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        // منع حذف الأدمن الافتراضي
        if (user.UserName == "admin")
        {
            TempData["Error"] = "لا يمكن حذف الحساب الإداري الافتراضي";
            return RedirectToAction(nameof(Index));
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "";

        var userDto = new UserDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            UserName = user.UserName ?? "",
            Email = user.Email ?? "",
            PhoneNumber = user.PhoneNumber,
            Role = role,
            RoleDisplayName = GetRoleDisplayName(role),
            IsActive = user.IsActive,
            CreatedDate = user.CreatedDate,
            LastLoginDate = user.LastLoginDate
        };

        return View(userDto);
    }

    // POST: Users/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        // منع حذف الأدمن الافتراضي
        if (user.UserName == "admin")
        {
            TempData["Error"] = "لا يمكن حذف الحساب الإداري الافتراضي";
            return RedirectToAction(nameof(Index));
        }

        await _auditLogService.LogDeleteAsync(User, _userManager, HttpContext, "user", 0, user, user.FullName);
        
        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded)
        {
            TempData["Success"] = $"تم حذف المستخدم '{user.FullName}' بنجاح";
            _logger.LogInformation($"User {user.UserName} deleted successfully");
            return RedirectToAction(nameof(Index));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return RedirectToAction(nameof(Delete), new { id });
    }

    // Helper Methods
    private List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> GetRolesList()
    {
        return new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
        {
            new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "Admin", Text = "مدير النظام" },
            new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "Teacher", Text = "مدرس" }
        };
    }

    private string GetRoleDisplayName(string role)
    {
        return role switch
        {
            "Admin" => "مدير النظام",
            "Teacher" => "مدرس",
            _ => role
        };
    }
}
