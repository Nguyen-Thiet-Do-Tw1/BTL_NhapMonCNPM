using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ChiTietMuonTraController : Controller
{
    private readonly ApplicationDbContext _context;

    public ChiTietMuonTraController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult ThemChiTietMuonTra(string maMuonTra)
    {
        var lastMaMuonCTTra = _context.ChiTietMuonTra
            .OrderByDescending(mt => mt.MaChiTiet)
            .Select(mt => mt.MaChiTiet)
            .FirstOrDefault();

        string newMaCTMuonTra = GenerateNextMaCTMuonTra(lastMaMuonCTTra);

        ViewBag.MaMuonTra = maMuonTra;
        ViewBag.DanhSachTaiLieu = _context.TaiLieu.ToList();

        return View(new ChiTietMuonTra
        {
            MaChiTiet = newMaCTMuonTra,
            MaTaiLieu = "",
            SoluongMuon = 1,
            TinhTrangSachMuon = "Mới",
            MaMuonTra = maMuonTra
        });
    }

    [HttpGet]
    public IActionResult GenerateNewMaChiTiet()
    {
        var lastMaCT = _context.ChiTietMuonTra
            .OrderByDescending(mt => mt.MaChiTiet)
            .Select(mt => mt.MaChiTiet)
            .FirstOrDefault();

        string newMaCT = GenerateNextMaCTMuonTra(lastMaCT);
        return Json(newMaCT);
    }



    private string GenerateNextMaCTMuonTra(string? lastMaCT)
    {
        if (string.IsNullOrEmpty(lastMaCT))
        {
            return "CT001"; // Nếu chưa có dữ liệu
        }

        string prefix = "CT";
        int number;
        if (lastMaCT.StartsWith(prefix) && int.TryParse(lastMaCT.Substring(2), out number))
        {
            number++;
            return $"{prefix}{number:D3}";
        }

        return "CT001"; // Trường hợp lỗi
    }


    [HttpPost]
    public async Task<IActionResult> ThemChiTietMuonTra(List<ChiTietMuonTra> ChiTietMuonTraList, string maMuonTra)
    {
        if (ChiTietMuonTraList == null || !ChiTietMuonTraList.Any())
        {
            ModelState.AddModelError("", "Dữ liệu không được gửi lên đúng cách.");
            ViewBag.DanhSachTaiLieu = _context.TaiLieu.ToList();
            return View();
        }

        var lastMaMuonCTTra = _context.ChiTietMuonTra
            .OrderByDescending(mt => mt.MaChiTiet)
            .Select(mt => mt.MaChiTiet)
            .FirstOrDefault();

        List<string> errors = new List<string>();

        foreach (var item in ChiTietMuonTraList)
        {
            var taiLieu = await _context.TaiLieu.FindAsync(item.MaTaiLieu);
            if (taiLieu == null)
            {
                errors.Add($"Tài liệu có mã {item.MaTaiLieu} không tồn tại.");
                continue;
            }

            if (taiLieu.SoLuong < item.SoluongMuon)
            {
                errors.Add($"Tài liệu '{taiLieu.TenTaiLieu}' chỉ còn {taiLieu.SoLuong} cuốn, không đủ để mượn {item.SoluongMuon} cuốn.");
                continue;
            }

            // Cập nhật mã chi tiết mới trước khi lưu
            item.MaChiTiet = GenerateNextMaCTMuonTra(lastMaMuonCTTra);
            lastMaMuonCTTra = item.MaChiTiet; // Cập nhật mã mới để không bị trùng

            item.MaMuonTra = maMuonTra;
            taiLieu.SoLuong -= item.SoluongMuon;
            _context.TaiLieu.Update(taiLieu);
        }


        if (errors.Count > 0)
        {
            ViewBag.Errors = errors;
            ViewBag.DanhSachTaiLieu = _context.TaiLieu.ToList();
            ViewBag.MaMuonTra = maMuonTra; // Đảm bảo truyền lại mã phiếu mượn khi có lỗi
            return View(ChiTietMuonTraList);
        }


        _context.ChiTietMuonTra.AddRange(ChiTietMuonTraList);
        await _context.SaveChangesAsync();

        return RedirectToAction("ChiTiet", "MuonTra", new { id = maMuonTra });
    }
}