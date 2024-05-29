using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorDemand.Data.Enums;

namespace TutorDemand.Data.Dtos.Customer
{
    public class CustomerUpdateDto
    {
        public Guid CustomerId { get; set; }

        [Required]
        [DisplayName("Tên")]
        [MaxLength(100, ErrorMessage = "Tên tối đa 100 kí tự.")]
        public string Fullname { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(100, ErrorMessage = "Email tối đa 100 kí tự.")]
        public string Email { get; set; } = null!; //UNIQUE, nếu có mua hàng lần 2 thì find by email trước

        [Required]
        [RegularExpression(@"^(\+84|0)[1-9]\d{8}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        [DisplayName("Số điện thoại")]
        [MaxLength(12, ErrorMessage = "Số điện thoại tối đa 100 kí tự.")]
        public string Phone { get; set; } = null!;

        [DisplayName("Địa chỉ")]
        [MaxLength(300, ErrorMessage = "Địa chỉ tối đa 300 kí tự.")]
        public string? Address { get; set; } = null!;

        [DisplayName("Giới tính")]
        [MaxLength(10)]
        [EnumDataType(typeof(Gender))]
        public string? Gender { get; set; }

        [DisplayName("Ngày sinh")] public DateOnly? Dob { get; set; }
    }
}