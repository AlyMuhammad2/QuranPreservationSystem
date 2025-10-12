namespace QuranPreservationSystem.Application.DTOs
{
    /// <summary>
    /// DTO للمدرس - لنقل البيانات بين الطبقات
    /// </summary>
    public class TeacherDto
    {
        public int TeacherId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Qualification { get; set; }
        public string? Specialization { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
        public string? Notes { get; set; }
        
        // معلومات المركز
        public int CenterId { get; set; }
        public string? CenterName { get; set; }
        
        // معلومات إحصائية
        public int CoursesCount { get; set; }
    }

    public class CreateTeacherDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Qualification { get; set; }
        public string? Specialization { get; set; }
        public int CenterId { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateTeacherDto
    {
        public int TeacherId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Qualification { get; set; }
        public string? Specialization { get; set; }
        public int CenterId { get; set; }
        public bool IsActive { get; set; }
        public string? Notes { get; set; }
    }
}

