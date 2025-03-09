using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;

// [Authorize(Roles = "Admin")]
public class MuonTraController : Controller
{
    private readonly ApplicationDbContext _context;

    public MuonTraController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var danhSachPhieuMuon = await _context.MuonTra
            .Include(mt => mt.NguoiDung)
            .OrderByDescending(mt => mt.MaMuonTra)
            .ToListAsync();
        return View(danhSachPhieuMuon);
    }

    public async Task<IActionResult> ChiTiet(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("Mã phiếu mượn không hợp lệ.");
        }

        try
        {
            var danhSachChiTiet = await _context.ChiTietMuonTra
                .Where(ct => ct.MaMuonTra == id)
                .Include(ct => ct.TaiLieu)
                .ToListAsync();

            if (danhSachChiTiet == null || danhSachChiTiet.Count == 0)
            {
                return NotFound("Không tìm thấy chi tiết phiếu mượn.");
            }

            return View("ChiTiet", danhSachChiTiet);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Lỗi server: {ex.Message}");
        }
    }


    public IActionResult ThemPhieuMuon()
    {
        // Lấy mã phiếu mượn lớn nhất từ CSDL
        var lastMaMuonTra = _context.MuonTra
            .OrderByDescending(mt => mt.MaMuonTra)
            .Select(mt => mt.MaMuonTra)
            .FirstOrDefault();

        // Tạo mã mới (tăng giá trị cuối cùng lên 1)
        string newMaMuonTra = GenerateNextMaMuonTra(lastMaMuonTra);

        ViewBag.DanhSachNguoiDung = _context.NguoiDung.ToList(); // Lấy danh sách độc giả

        return View(new MuonTra
        {
            MaNguoiDung = "",
            MaMuonTra = newMaMuonTra,
            NgayMuon = DateTime.Now,
            NgayHenTra = DateTime.Now.AddDays(7),
            TinhTrang = "Đang mượn"
        });
    }

    // Hàm tạo mã phiếu mượn tiếp theo
    private string GenerateNextMaMuonTra(string? lastMa)
    {
        if (string.IsNullOrEmpty(lastMa))
        {
            return "MT001";
        }

        string prefix = lastMa.Substring(0, 2);
        int number = int.Parse(lastMa.Substring(2));
        number++;
        return $"{prefix}{number:D3}";
    }


    [HttpPost]
    public async Task<IActionResult> ThemPhieuMuon([Bind("MaMuonTra,MaCTMuonTra,MaNguoiDung,NgayHenTra,NgayTraThucTe,TinhTrang")] MuonTra model)
    {
        if (string.IsNullOrEmpty(model.MaNguoiDung))
        {
            ModelState.AddModelError("MaNguoiDung", "Vui lòng chọn độc giả.");
        }

        if (model.NgayHenTra == default)
        {
            ModelState.AddModelError("NgayHenTra", "Vui lòng chọn ngày hẹn trả.");
        }
        model.NgayMuon = DateTime.Now;
        if (model.NgayHenTra <= DateTime.Now.Date)
        {
            ModelState.AddModelError("NgayHenTra", "Ngày hẹn trả phải lớn hơn ngày mượn");
        }
        if (!ModelState.IsValid)
        {
            ViewBag.DanhSachNguoiDung = _context.NguoiDung.ToList();
            return View(model);
        }

        _context.MuonTra.Add(model);
        await _context.SaveChangesAsync();

        // Chuyển hướng đến trang Thêm Chi Tiết Phiếu Mượn
        return RedirectToAction("ThemChiTietMuonTra", "ChiTietMuonTra", new { maMuonTra = model.MaMuonTra });
    }

    [HttpGet]
    public async Task<IActionResult> GetPhieuMuon(string id)
    {
        var phieuMuon = await _context.MuonTra.FindAsync(id);
        if (phieuMuon == null)
        {
            return NotFound();
        }

        return Json(new
        {
            maMuonTra = phieuMuon.MaMuonTra,
            maNguoiDung = phieuMuon.MaNguoiDung,
            ngayMuon = phieuMuon.NgayMuon.ToString("yyyy-MM-dd"),
            ngayHenTra = phieuMuon.NgayHenTra.ToString("yyyy-MM-dd"),
            tinhTrang = phieuMuon.TinhTrang
        });
    }

    [HttpPost]
    public async Task<IActionResult> UpdatePhieuMuon(MuonTra model)
    {
        var phieuMuon = await _context.MuonTra.FindAsync(model.MaMuonTra);
        if (phieuMuon == null)
        {
            return NotFound();
        }
        if (model.NgayHenTra <= DateTime.Now.Date)
        {
            ModelState.AddModelError("NgayHenTra", "Ngày hẹn trả phải lớn hơn ngày mượn");
        }

        phieuMuon.MaMuonTra = model.MaMuonTra;
        phieuMuon.MaNguoiDung = model.MaNguoiDung;
        phieuMuon.NgayMuon = model.NgayMuon;
        phieuMuon.NgayHenTra = model.NgayHenTra;
        phieuMuon.TinhTrang = model.TinhTrang;

        _context.Update(phieuMuon);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> DeletePhieuMuon(string id)
    {
        var phieuMuon = await _context.MuonTra.FindAsync(id);
        if (phieuMuon == null)
        {
            return NotFound();
        }
        bool hasDetails = await _context.ChiTietMuonTra.AnyAsync(ct => ct.MaMuonTra == id);
        if (hasDetails)
        {
            return BadRequest("Không thể xóa vì có chi tiết phiếu mượn liên quan.");
        }

        _context.MuonTra.Remove(phieuMuon);
        await _context.SaveChangesAsync();
        return Ok();
    }

    public async Task<IActionResult> TraSach(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("Mã phiếu mượn không hợp lệ.");
        }

        var danhSachChiTiet = await _context.ChiTietMuonTra
            .Where(ct => ct.MaMuonTra == id)
            .Include(ct => ct.TaiLieu)
            .ToListAsync();

        if (danhSachChiTiet == null || danhSachChiTiet.Count == 0)
        {
            return NotFound("Không tìm thấy chi tiết phiếu mượn.");
        }

        ViewBag.MaMuonTra = id;
        return View(danhSachChiTiet);
    }

    [HttpPost]
    public async Task<IActionResult> XacNhanTraSach(string maMuonTra, List<ChiTietMuonTra> danhSachTra)
    {
        if (string.IsNullOrEmpty(maMuonTra) || danhSachTra == null || !danhSachTra.Any())
        {
            ModelState.AddModelError("", "Dữ liệu không hợp lệ.");
            return View("TraSach", danhSachTra);
        }

        var phieuMuon = await _context.MuonTra.FindAsync(maMuonTra);
        if (phieuMuon == null)
        {
            ModelState.AddModelError("", "Không tìm thấy phiếu mượn.");
            return View("TraSach", danhSachTra);
        }

        var danhSachGiaTriHopLe = new HashSet<string> { "Tốt", "Hư hỏng", "Mất" };
        bool hasErrors = false;

        foreach (var item in danhSachTra)
        {
            var chiTiet = await _context.ChiTietMuonTra
                .FirstOrDefaultAsync(ct => ct.MaChiTiet == item.MaChiTiet);

            if (chiTiet != null)
            {
                if (!danhSachGiaTriHopLe.Contains(item.TinhTrangTra))
                {
                    ModelState.AddModelError($"danhSachTra[{danhSachTra.IndexOf(item)}].TinhTrangTra", "Tình trạng trả không hợp lệ.");
                    hasErrors = true;
                }

                if (item.SoLuongTra < 0)
                {
                    ModelState.AddModelError($"danhSachTra[{danhSachTra.IndexOf(item)}].SoLuongTra", "Số lượng trả không thể là số âm.");
                    hasErrors = true;
                }
                else if (item.SoLuongTra > chiTiet.SoluongMuon)
                {
                    ModelState.AddModelError($"danhSachTra[{danhSachTra.IndexOf(item)}].SoLuongTra", $"Số lượng trả ({item.SoLuongTra}) không thể lớn hơn số lượng mượn ({chiTiet.SoluongMuon}).");
                    hasErrors = true;
                }

                if (item.PhiPhat < 0)
                {
                    ModelState.AddModelError($"danhSachTra[{danhSachTra.IndexOf(item)}].PhiPhat", "Số tiền phạt không thể là số âm.");
                    hasErrors = true;
                }

                if (hasErrors) continue; // Nếu có lỗi, không cập nhật dữ liệu

                chiTiet.SoLuongTra = item.SoLuongTra;
                chiTiet.TinhTrangTra = item.TinhTrangTra;
                chiTiet.PhiPhat = item.PhiPhat;

                var taiLieu = await _context.TaiLieu.FirstOrDefaultAsync(tl => tl.MaTaiLieu == chiTiet.MaTaiLieu);
                if (taiLieu != null && item.SoLuongTra.HasValue)
                {
                    taiLieu.SoLuong += item.SoLuongTra.Value;
                }
            }
        }

        if (hasErrors)
        {
            return View("TraSach", danhSachTra);
        }

        phieuMuon.NgayTraThucTe = DateTime.Now;
        phieuMuon.TinhTrang = "Đã trả";

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    [HttpGet]
    public async Task<IActionResult> GetDanhSachNguoiDung()
    {
        var danhSachNguoiDung = await _context.NguoiDung
            .Select(nd => new { nd.MaNguoiDung, nd.HoTen })
            .ToListAsync();
        return Json(danhSachNguoiDung);
    }













}
