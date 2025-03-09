using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;
using System.Data;
using System.Linq;

namespace QuanLyThuVien.Controllers
{
    public class AccountController : Controller
    {
        // private readonly string _connectionString;

        // public AccountController(IConfiguration configuration)
        // {
        //     _connectionString = configuration.GetConnectionString("DefaultConnection") 
        //                         ?? throw new InvalidOperationException("Connection string is not configured.");
        // }

        private readonly ApplicationDbContext _context;



        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(TaiKhoan model)
        {
            ModelState.Remove("MaTaiKhoan");
            ModelState.Remove("MaNguoiDung");

            if (ModelState.IsValid)
            {
                var user = _context.TaiKhoan
                    .Where(u => u.TenDangNhap == model.TenDangNhap && u.MatKhau == model.MatKhau)
                    .Join(_context.NguoiDung,
                        tk => tk.MaNguoiDung,
                        nd => nd.MaNguoiDung,
                        (tk, nd) => new { TaiKhoan = tk, NguoiDung = nd })
                    .FirstOrDefault();

                if (user != null)
                {
                    HttpContext.Session.SetString("TenDangNhap", user.TaiKhoan.TenDangNhap);
                    HttpContext.Session.SetString("LoaiNguoiDung", user.NguoiDung.LoaiNguoiDung);

                    if (user.NguoiDung.LoaiNguoiDung == "Quản trị viên")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "TaiLieu");
                    }
                }

                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            }

            return View(model);
        }
    }
}
