using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    [Table("tblTaiLieu")]
    public class TaiLieu
    {
        [Key]
        [Column("sMaTaiLieu")]
        public required string MaTaiLieu { get; set; }

        [Required]
        [Column("sTenTaiLieu")]
        public required string TenTaiLieu { get; set; }

        [Required]
        [Column("sAnhURL")]
        public required string img { get; set; }

        [Column("sTenTacGia")]
        public required string TenTacGia { get; set; }

        [Column("sTheLoai")]
        public string? TheLoai { get; set; } 

        [Column("iNamXuatBan")]
        public int NamXuatBan { get; set; }

        [Column("sNXB")]
        public required string NXB { get; set; }

        [Column("iSoLuong")]
        public int SoLuong { get; set; }

        [Column("sTinhTrang")]
        public required string TinhTrang { get; set; }

        [Column("sViTri")]
        public required string ViTri { get; set; }
    }
}
