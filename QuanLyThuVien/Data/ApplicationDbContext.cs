using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<TaiKhoan> TaiKhoan { get; set; }
        public DbSet<NguoiDung> NguoiDung { get; set; }
        public DbSet<TaiLieu> TaiLieu { get; set; }
        public DbSet<MuonTra> MuonTra { get; set; }
        public DbSet<ChiTietMuonTra> ChiTietMuonTra { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ánh xạ đến bảng có sẵn trong cơ sở dữ liệu
            modelBuilder.Entity<TaiKhoan>().ToTable("tblTaiKhoan");
            modelBuilder.Entity<NguoiDung>().ToTable("tblNguoiDung");
            modelBuilder.Entity<TaiLieu>().ToTable("tblTaiLieu");
            modelBuilder.Entity<MuonTra>().ToTable("tblMuonTra");
            modelBuilder.Entity<ChiTietMuonTra>().ToTable("tblChiTietMuonTra");
            // Định nghĩa TaiKhoan là một keyless entity
            modelBuilder.Entity<TaiKhoan>().HasNoKey();
            modelBuilder.Entity<MuonTra>()
                .HasOne(m => m.NguoiDung)
                .WithMany()
                .HasForeignKey(m => m.MaNguoiDung)
                .HasPrincipalKey(n => n.MaNguoiDung);

        }

    }
}
