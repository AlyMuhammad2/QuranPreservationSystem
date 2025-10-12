namespace QuranPreservationSystem.Application.DTOs
{
    /// <summary>
    /// DTO للمركز - لنقل البيانات بين الطبقات
    /// </summary>
    public class CenterDto
    {
        public int CenterId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        
        // معلومات إحصائية
        public int TeachersCount { get; set; }
        public int StudentsCount { get; set; }
        public int CoursesCount { get; set; }
    }

    public class CreateCenterDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateCenterDto
    {
        public int CenterId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}

