using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TutorDemand.Data.Enums;

namespace TutorDemand.Data.Entities;

public class Tutor
{
    [Key]
    public int Id { get; set; }

    public Guid TutorId { get; set; }

    [Required]
    [DisplayName("Tên giảng viên")]
    [MaxLength(100, ErrorMessage = "Tên giảng viên tối đa 100 kí tự.")]
    public string Fullname { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [RegularExpression(@"^(\+84|0)[1-9]\d{8}$", ErrorMessage = "Số điện thoại không hợp lệ")]
    [DisplayName("Số điện thoại")]
    public string Phone { get; set; } = null!;

    [DisplayName("Địa chỉ")]
    [MaxLength(300, ErrorMessage = "Địa chỉ tối đa 300 kí tự.")]
    public string? Address { get; set; }

    [DisplayName("Giới tính")]
    [MaxLength(10)]
    [EnumDataType(typeof(Gender))]
    public string? Gender { get; set; }

    [DisplayName("Ngày sinh")]
    public DateOnly? Dob { get; set; }

    [MaxLength(500)]
    public string? Avatar { get; set; }

    [MaxLength(500)]
    public string? CertificateImage { get; set; }

    [MaxLength(500)]
    public string? IdentityCard { get; set; }

    //navigator
    public ICollection<TeachingSchedule> TeachingSchedules { get; set; } = [];
}