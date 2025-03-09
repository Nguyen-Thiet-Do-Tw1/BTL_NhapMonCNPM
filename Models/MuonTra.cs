using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    [Table("tblMuonTra")] 
    public class MuonTra
    {
        [Key]
        [Column("sMaMuonTra")]
        public string MaMuonTra { get; set; } = null!;

        [Required]
        [ForeignKey("sMaNguoiDung")]
        [Column("sMaNguoiDung")]
        public required string MaNguoiDung { get; set; }

        [Column("dNgayMuon")]
        public DateTime NgayMuon { get; set; }

        [Column("dNgayHenTra")]
        public DateTime NgayHenTra { get; set; }

        [Column("dNgayTraThucTe")]
        public DateTime? NgayTraThucTe { get; set; }

        [Required]
        [Column("sTinhTrang")]
        public required string TinhTrang { get; set; }

        public NguoiDung? NguoiDung { get; set; } 
    }
}
