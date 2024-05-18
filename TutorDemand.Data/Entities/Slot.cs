using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TutorDemand.Data.Entities;

public class Slot
{
    [Key]
    public int Id { get; set; }

    public Guid SlotId { get; set; }

    [Required]
    [DisplayName("Tên tiết học")]
    [MaxLength(50, ErrorMessage = "Tên tiết học tối đa 50 kí tự.")]
    public string SlotName { get; set; } = null!;

    [Required]
    [DisplayName("Thời lương (giờ)")]
    [Range(0, int.MaxValue, ErrorMessage = "Thời lượng phải lớn hơn không")]
    public float Duration { get; set; }

    [Required]
    [DisplayName("Giờ học bắt đầu")]
    public TimeOnly Time { get; set; }

    [DisplayName("Mô tả tiết học")]
    public string? SlotDesc { get; set; }

    //navigator
    public ICollection<TeachingSchedule> TeachingSchedules { get; set; } = [];
}