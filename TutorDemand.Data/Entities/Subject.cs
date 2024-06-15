using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TutorDemand.Data.Entities;

public class Subject
{
    [Key] public int Id { get; set; }

    public Guid SubjectId { get; set; }

    [Required]
    [DisplayName("Code")]
    [MaxLength(50, ErrorMessage = "Subject code not exceeds than 50 characters")]
    public string SubjectCode { get; set; } = null!;

    [Required]
    [DisplayName(nameof(Name))]
    [MaxLength(100, ErrorMessage = "Subject name not exceeds than 100 characters")]
    public string Name { get; set; } = null!;

    [DisplayName(nameof(Image))]
    [MaxLength(500)]
    [AllowNull]
    public string? Image { get; set; } = string.Empty;

    [DisplayName(nameof(Description))]
    public string? Description { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Duration must greater than 0")]
    [DisplayName(nameof(Duration))]
    public decimal? Duration { get; set; }

    [DisplayName("Price")]
    [Range(1, int.MaxValue, ErrorMessage = "Cost price must greater than 0")]
    public decimal CostPrice { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? StartDate { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? EndDate { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Enrollment capacity must be a positive number")]
    [DisplayName("Enrollment Capacity")]
    public int? EnrolledCapacity { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Enrolled students must be a positive number")]
    [DisplayName("Enrolled Students")]
    public int? EnrolledStudents { get; set; } = 0;

    //navigator
    public ICollection<TeachingSchedule> TeachingSchedules { get; set; } = [];
}