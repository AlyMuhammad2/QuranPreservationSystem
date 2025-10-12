namespace QuranPreservationSystem.Domain.Enums
{
    /// <summary>
    /// أدوار المستخدمين في النظام
    /// </summary>
    public static class UserRole
    {
        /// <summary>
        /// مدير النظام - صلاحيات كاملة
        /// </summary>
        public const string Admin = "Admin";

        /// <summary>
        /// مشرف - صلاحيات متوسطة
        /// </summary>
        public const string Supervisor = "Supervisor";

        /// <summary>
        /// مدرس - يمكنه إدارة طلابه ودوراته فقط
        /// </summary>
        public const string Teacher = "Teacher";

        /// <summary>
        /// مدير مركز - يدير مركز واحد فقط
        /// </summary>
        public const string CenterManager = "CenterManager";

        /// <summary>
        /// مستخدم عادي - صلاحيات محدودة (عرض فقط)
        /// </summary>
        public const string User = "User";

        /// <summary>
        /// الحصول على جميع الأدوار
        /// </summary>
        public static List<string> GetAllRoles()
        {
            return new List<string>
            {
                Admin,
                Supervisor,
                Teacher,
                CenterManager,
                User
            };
        }
    }
}

