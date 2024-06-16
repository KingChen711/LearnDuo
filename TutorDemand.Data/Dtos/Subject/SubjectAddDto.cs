using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TutorDemand.Data.Dtos.Subject
{
    public class SubjectAddDto
    {
        public Guid SubjectId { get; set; }

        [Required(ErrorMessage = "Subject code is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Subject code length between (2-50)")]
        public string SubjectCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subject name is required")]
        [MaxLength(150, ErrorMessage = "Subject name not exceeds than 150 characters")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)] public string? Image { get; set; }

        [Required(ErrorMessage = "Please give some description for the subject")]
        [MaxLength] // No length threshold 
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subject Duration is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than 0")]
        public decimal? Duration { get; set; }

        [Required(ErrorMessage = "Subject Price is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal CostPrice { get; set; }
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Enrollment capacity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Total Enrollment Capacity")]
        public int? EnrolledCapacity { get; set; }

        public int? EnrolledStudents { get; set; } = 0;

        public List<string>? TutorIds { get; set; } = new List<string>();
        public string? SlotId { get; set; } 
    }
}