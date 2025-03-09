using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    [Table("tblChiTietMuonTra")]
    public class ChiTietMuonTra
    {
        [Key]
        [Column("sMaChiTiet")]
        public string MaChiTiet { get; set; } = null!;

        [Required]
        [Column("sMaMuonTra")]
        public required string MaMuonTra { get; set; }

        [Required]
        [Column("sMaTaiLieu")]
        public required string MaTaiLieu { get; set; }

        [Required]
        [Column("iSoluongMuon")]
        public int SoluongMuon { get; set; }

        [Required]
        [Column("sTinhTrangMuon")]
        public required string TinhTrangSachMuon { get; set; }

        [Column("iSoLuongTra")]
        public int? SoLuongTra { get; set; } // Chấp nhận NULL

        [Column("sTinhTrangTra")]
        public string? TinhTrangTra { get; set; } // Chấp nhận NULL

        [Column("fPhiPhat")]
        public double? PhiPhat { get; set; } // Chấp nhận NULL

        [ForeignKey("MaTaiLieu")]
        public TaiLieu? TaiLieu { get; set; } // Thêm quan hệ với bảng TaiLieu



    }
}
