using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace QuanLyThuVien.Models
{
    [Table("tblNguoiDung")]
    public class NguoiDung
    {
        [Key]
        [Column("sMaNguoiDung")]
        [BindNever] // Không bind giá trị này từ form
        public string? MaNguoiDung { get; set; }  

        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [Column("sHoTen")]
        public string HoTen { get; set; }

        [Column("sSDT")]
        public string SDT { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [Column("sMatKhau")]
        public string MatKhau { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Column("sEmail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Loại người dùng là bắt buộc")]
        [Column("sLoaiNguoiDung")]
        public string LoaiNguoiDung { get; set; }
    }
}
