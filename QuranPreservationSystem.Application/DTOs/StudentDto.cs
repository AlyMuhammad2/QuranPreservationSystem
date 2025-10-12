namespace QuranPreservationSystem.Application.DTOs
{
    /// <summary>
    /// DTO للطالب - لنقل البيانات بين الطبقات
    /// </summary>
    public class StudentDto
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string? Gender { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianPhone { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public bool IsActive { get; set; }
        public string? EducationLevel { get; set; }
        public string? Notes { get; set; }
        
        // معلومات المركز
        public int CenterId { get; set; }
        public string? CenterName { get; set; }
        
        // معلومات إحصائية
        public int EnrolledCoursesCount { get; set; }
    }

    public class CreateStudentDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianPhone { get; set; }
        public int CenterId { get; set; }
        public string? EducationLevel { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateStudentDto
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianPhone { get; set; }
        public int CenterId { get; set; }
        public bool IsActive { get; set; }
        public string? EducationLevel { get; set; }
        public string? Notes { get; set; }
    }
}

