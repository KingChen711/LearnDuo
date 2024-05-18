using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TutorDemand.Data.Entities;

public class Company
{
    [Key]
    public int Id { get; set; }

    public Guid CompanyId { get; set; }

    [Required]
    [MaxLength(200, ErrorMessage = "Tên công ty tối đa 200 kí tự")]
    [DisplayName("Tên công ty")]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(200, ErrorMessage = "Địa chỉ tối đa 200 kí tự")]
    [DisplayName("Địa chỉ")]
    public string Address { get; set; } = null!;

    [MaxLength(3000, ErrorMessage = "Mô tả tối đa 3000 kí tự")]
    [DisplayName("Mô tả")]
    public string? Description { get; set; }

    [Required]
    [RegularExpression(@"^(\+84|0)[1-9]\d{8}$", ErrorMessage = "Số điện thoại không hợp lệ")]
    [DisplayName("Số điện thoại")]
    public string Phone { get; set; } = null!;
}