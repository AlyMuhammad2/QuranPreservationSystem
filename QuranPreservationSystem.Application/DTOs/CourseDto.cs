namespace QuranPreservationSystem.Application.DTOs
{
    /// <summary>
    /// DTO للدورة - لنقل البيانات بين الطبقات
    /// </summary>
    public class CourseDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CourseType { get; set; }
        public string? Level { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Schedule { get; set; }
        public int? MaxStudents { get; set; }
        public int? DurationHours { get; set; }
        public bool IsActive { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        
        // معلومات المركز
        public int CenterId { get; set; }
        public string? CenterName { get; set; }
        
        // معلومات المدرس
        public int TeacherId { get; set; }
        public string? TeacherName { get; set; }
        
        // معلومات إحصائية
        public int EnrolledStudentsCount { get; set; }
        public int AvailableSeats { get; set; }
    }

    public class CreateCourseDto
    {
        public string CourseName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CourseType { get; set; }
        public string? Level { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Schedule { get; set; }
        public int? MaxStudents { get; set; }
        public int? DurationHours { get; set; }
        public int CenterId { get; set; }
        public int TeacherId { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateCourseDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CourseType { get; set; }
        public string? Level { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Schedule { get; set; }
        public int? MaxStudents { get; set; }
        public int? DurationHours { get; set; }
        public int CenterId { get; set; }
        public int TeacherId { get; set; }
        public bool IsActive { get; set; }
        public string? Notes { get; set; }
    }
}

