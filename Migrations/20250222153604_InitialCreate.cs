using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyThuVien.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblNguoiDung",
                columns: table => new
                {
                    sMaNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    sHoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sSDT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sMatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sLoaiNguoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblNguoiDung", x => x.sMaNguoiDung);
                });

            migrationBuilder.CreateTable(
                name: "tblTaiKhoan",
                columns: table => new
                {
                    sMaTaiKhoan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    sMaNguoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sTenDangNhap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sMatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTaiKhoan", x => x.sMaTaiKhoan);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblNguoiDung");

            migrationBuilder.DropTable(
                name: "tblTaiKhoan");
        }
    }
}
