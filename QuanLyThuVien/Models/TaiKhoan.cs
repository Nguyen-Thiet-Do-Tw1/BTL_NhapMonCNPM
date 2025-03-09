using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    [Table("tblTaiKhoan")] 
    public class TaiKhoan
    {
        [Key]
        [Column("sMaTaiKhoan")] 
        public required string MaTaiKhoan { get; set; }

        [Column("sMaNguoiDung")]
        public required string MaNguoiDung { get; set; }
        
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [Column("sTenDangNhap")]
        public required string TenDangNhap { get; set; }

        [Required]
        [Column("sMatKhau")]
        public required string MatKhau { get; set; }
    }
}
