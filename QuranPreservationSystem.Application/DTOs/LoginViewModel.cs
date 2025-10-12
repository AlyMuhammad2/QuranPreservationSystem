using System.ComponentModel.DataAnnotations;

namespace QuranPreservationSystem.Application.DTOs
{
    /// <summary>
    /// نموذج تسجيل الدخول
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [Display(Name = "اسم المستخدم")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "تذكرني")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}

