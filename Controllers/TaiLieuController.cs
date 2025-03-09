using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;


namespace QuanLyThuVien.Controllers
{
    public class TaiLieuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaiLieuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách thể loại và danh sách tài liệu
        public IActionResult Index(string? theLoai = null)
        {
            var loaiNguoiDung = HttpContext.Session.GetString("LoaiNguoiDung");

            if (string.IsNullOrEmpty(loaiNguoiDung))
            {
                return RedirectToAction("Login", "Account");
            }
            // Lấy danh sách thể loại, loại bỏ null và chuỗi rỗng
            var theLoaiList = _context.TaiLieu
                .Select(t => t.TheLoai ?? string.Empty) // Thay thế null bằng chuỗi trống
                .Where(t => !string.IsNullOrEmpty(t))  // Loại bỏ chuỗi rỗng nếu cần
                .Distinct()
                .ToList();

            // Lấy danh sách tài liệu theo thể loại
            var taiLieuList = string.IsNullOrEmpty(theLoai)
                ? _context.TaiLieu.ToList()
                : _context.TaiLieu
                    .Where(t => t.TheLoai != null && t.TheLoai.Trim().ToLower() == theLoai.Trim().ToLower())
                    .ToList();

            // Truyền dữ liệu vào View
            ViewBag.TheLoaiList = theLoaiList;
            ViewBag.SelectedTheLoai = theLoai;
            return View(taiLieuList);
        }

        public IActionResult QuanLyTaiLieu()
        {
            ViewBag.DanhSachTaiLieu = _context.TaiLieu.ToList();
            return View();
        }
        [HttpGet]
        public IActionResult GetNextMaTaiLieu()
        {
            var maxMaTaiLieu = _context.TaiLieu
                .OrderByDescending(t => t.MaTaiLieu)
                .Select(t => t.MaTaiLieu)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(maxMaTaiLieu))
            {
                return Json(new { nextMaTaiLieu = "TL001" });
            }

            // Tách số từ mã (VD: "TL001" -> "001")
            string prefix = maxMaTaiLieu.Substring(0, 2); // "TL"
            int number = int.Parse(maxMaTaiLieu.Substring(2)) + 1; // 002

            string nextMaTaiLieu = $"{prefix}{number:D3}"; // "TL002"
            return Json(new { nextMaTaiLieu });
        }


        [HttpPost]
        public IActionResult CreateOrUpdate([FromBody] TaiLieu taiLieu)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });

            var existingTaiLieu = _context.TaiLieu.Find(taiLieu.MaTaiLieu);
            if (existingTaiLieu != null)
            {
                _context.Entry(existingTaiLieu).CurrentValues.SetValues(taiLieu);
            }
            else
            {
                _context.TaiLieu.Add(taiLieu);
            }
            _context.SaveChanges();
            return Json(new { success = true, message = existingTaiLieu != null ? "Cập nhật thành công!" : "Thêm thành công!" });
        }

        [HttpGet]
        public IActionResult Get(string maTaiLieu)
        {
            var taiLieu = _context.TaiLieu.Find(maTaiLieu);
            return taiLieu == null ? NotFound() : Json(taiLieu);
        }

        [HttpGet]
        public IActionResult GetAll() => Json(_context.TaiLieu.ToList());

        [HttpPost]
        public IActionResult Delete(string maTaiLieu)
        {
            var taiLieu = _context.TaiLieu.Find(maTaiLieu);
            if (taiLieu == null) return Json(new { success = false, message = "Xóa thất bại!" });
            bool hasPhieuMuon = _context.ChiTietMuonTra.Any(pm => pm.MaTaiLieu == maTaiLieu);
            if (hasPhieuMuon)
            {
                return Json(new { success = false, message = "Sách này đã có phiếu mượn kèm theo, không được xóa!" });
            }
            _context.TaiLieu.Remove(taiLieu);
            _context.SaveChanges();
            return Json(new { success = true, message = "Xóa thành công!" });
        }
    }

}
