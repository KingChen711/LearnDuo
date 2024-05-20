using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TutorDemand.Data.Dtos.TeachingSchedule
{
    public record TeachingScheduleCreationDto
    {
        [Required] public Guid TutorId { get; set; }

        [Required] public Guid SubjectId { get; set; }

        [Required] public Guid SlotId { get; set; }

        [Required(ErrorMessage = "Mã phòng Google Meet bắt buộc")]
        [MaxLength(50, ErrorMessage = "Mã phòng Google Meet tối đa 50 kí tự")]
        [DisplayName("Mã phòng Google Meet*")]
        public string MeetRoomCode { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu phòng Google Meet bắt buộc")]
        [MaxLength(50, ErrorMessage = "Mật khẩu phòng Google Meet tối đa 50 kí tự")]
        [DisplayName("Mật khẩu phòng Google Meet*")]
        public string RoomPassword { get; set; } = null!;
        
        [DisplayName("Các ngày học trong tuần*")]
        public string? LearnDays { get; set; } //Example: "Monday,Wednesday,Friday"

        [Required(ErrorMessage = "Giá khóa học bắt buộc")]
        [Range(0, int.MaxValue, ErrorMessage = "Giá khóa học phải lớn hơn 0")]
        [DisplayName("Giá khóa học(VNĐ)*")]
        public int PaidPrice { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu bắt buộc")]
        [DisplayName("Ngày bắt đầu*")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc bắt buộc")]
        [DisplayName("Ngày kết thúc*")]
        public DateOnly EndDate { get; set; }
    }
}