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
        /// مدرس - يمكنه إدارة طلابه ودوراته فقط
        /// </summary>
        public const string Teacher = "Teacher";

   
        public static List<string> GetAllRoles()
        {
            return new List<string>
            {
                Admin,
                Teacher
            
            };
        }
    }
}

