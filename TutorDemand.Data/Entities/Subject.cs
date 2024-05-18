using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TutorDemand.Data.Entities;

public class Subject
{
    [Key]
    public int Id { get; set; }

    public Guid SubjectId { get; set; }

    [Required]
    [DisplayName("Mã môn học")]
    [MaxLength(50, ErrorMessage = "Mã môn học tối đa 50 kí tự.")]
    public string SubjectCode { get; set; } = null!;

    [Required]
    [DisplayName("Tên môn học")]
    [MaxLength(100, ErrorMessage = "Tên môn học tối đa 100 kí tự.")]
    public string Name { get; set; } = null!;

    [DisplayName("Ảnh đại diện môn học")]
    [MaxLength(500)]
    public string Image { get; set; } = "/images/default-subject.jpg";


    //navigator
    public ICollection<TeachingSchedule> TeachingSchedules { get; set; } = [];
}