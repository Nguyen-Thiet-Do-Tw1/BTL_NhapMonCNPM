using Microsoft.Data.SqlClient; // Sử dụng Microsoft.Data.SqlClient để hỗ trợ Unicode
using System.Data;
using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;
using Microsoft.EntityFrameworkCore;

namespace QuanLyThuVien.Controllers
{
    public class NguoiDungController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NguoiDungController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách người dùng
        public IActionResult Index()
        {
            var users = _context.NguoiDung.ToList();
            return View(users);
        }

        // GET: Trang thêm người dùng
        public IActionResult Create()
        {
            return View();
        }

        // POST: Xử lý thêm người dùng qua stored procedure
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NguoiDung model)
        {
            if (ModelState.IsValid)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(model.SDT ?? "", @"^\d{1,12}$"))
                {
                    return BadRequest("Số điện thoại không hợp lệ! Chỉ chứa số và tối đa 12 ký tự.");
                }
                // Kiểm tra xem SDT hoặc Email đã tồn tại chưa
                bool isDuplicate = _context.NguoiDung.Any(u => u.SDT == model.SDT || u.Email == model.Email);

                if (isDuplicate)
                {
                    return BadRequest("Số điện thoại hoặc email đã được sử dụng!");
                }

                try
                {
                    string newId = InsertNguoiDungUsingProcedure(model);
                    System.Diagnostics.Debug.WriteLine("Mã người dùng mới: " + newId);
                    return Json(new { success = true }); // Trả về JSON khi thành công
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Lỗi khi thêm người dùng: " + ex.Message);
                    return BadRequest("Lỗi khi thêm người dùng: " + ex.Message);
                }
            }
            return BadRequest("Dữ liệu không hợp lệ!");
        }



        // Phương thức gọi stored procedure usp_InsertNguoiDung
        private string InsertNguoiDungUsingProcedure(NguoiDung model)
        {
            string newMaNguoiDung = string.Empty;

            // Chuẩn hóa các chuỗi tiếng Việt
            model.HoTen = model.HoTen.Trim().Normalize(System.Text.NormalizationForm.FormC);
            if (!string.IsNullOrEmpty(model.SDT))
                model.SDT = model.SDT.Trim().Normalize(System.Text.NormalizationForm.FormC);
            model.MatKhau = model.MatKhau.Trim().Normalize(System.Text.NormalizationForm.FormC);
            model.Email = model.Email.Trim().Normalize(System.Text.NormalizationForm.FormC);
            model.LoaiNguoiDung = model.LoaiNguoiDung.Trim().Normalize(System.Text.NormalizationForm.FormC);

            using (var connection = _context.Database.GetDbConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "usp_InsertNguoiDung";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@HoTen", System.Data.SqlDbType.NVarChar, 100) { Value = model.HoTen });
                    command.Parameters.Add(new SqlParameter("@SDT", System.Data.SqlDbType.NVarChar, 50)
                    { Value = string.IsNullOrEmpty(model.SDT) ? DBNull.Value : (object)model.SDT });
                    command.Parameters.Add(new SqlParameter("@MatKhau", System.Data.SqlDbType.NVarChar, 100) { Value = model.MatKhau });
                    command.Parameters.Add(new SqlParameter("@Email", System.Data.SqlDbType.NVarChar, 100) { Value = model.Email });
                    command.Parameters.Add(new SqlParameter("@LoaiNguoiDung", System.Data.SqlDbType.NVarChar, 50) { Value = model.LoaiNguoiDung });

                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        newMaNguoiDung = result.ToString();
                    }
                }
            }
            return newMaNguoiDung;
        }

        // GET: Trang chỉnh sửa người dùng
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = _context.NguoiDung.FirstOrDefault(u => u.MaNguoiDung == id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: Xử lý chỉnh sửa người dùng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, NguoiDung model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.NguoiDung.FirstOrDefault(u => u.MaNguoiDung == id);
                if (user == null)
                    return NotFound();

                // Kiểm tra số điện thoại hợp lệ
                if (!System.Text.RegularExpressions.Regex.IsMatch(model.SDT ?? "", @"^\d{1,12}$"))
                {
                    ModelState.AddModelError("SDT", "Số điện thoại phải là số và chỉ chứa tối đa 12 chữ số.");
                    return View(model);
                }

                // Kiểm tra số điện thoại hoặc email đã tồn tại ở người dùng khác
                bool isDuplicate = _context.NguoiDung.Any(u => (u.SDT == model.SDT || u.Email == model.Email) && u.MaNguoiDung != id);
                if (isDuplicate)
                {
                    ModelState.AddModelError("", "Số điện thoại hoặc email đã được sử dụng bởi người khác.");
                    return View(model);
                }

                // Cập nhật thông tin người dùng
                user.HoTen = model.HoTen.Trim().Normalize(System.Text.NormalizationForm.FormC);
                user.SDT = model.SDT?.Trim().Normalize(System.Text.NormalizationForm.FormC);
                user.MatKhau = model.MatKhau.Trim().Normalize(System.Text.NormalizationForm.FormC);
                user.Email = model.Email.Trim().Normalize(System.Text.NormalizationForm.FormC);
                user.LoaiNguoiDung = model.LoaiNguoiDung.Trim().Normalize(System.Text.NormalizationForm.FormC);

                // Giữ nguyên mật khẩu cũ nếu không nhập mới
                if (string.IsNullOrWhiteSpace(model.MatKhau))
                {
                    model.MatKhau = user.MatKhau; // Giữ nguyên mật khẩu cũ
                }
                else
                {
                    user.MatKhau = model.MatKhau.Trim().Normalize(System.Text.NormalizationForm.FormC);
                }


                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }



        // GET: Trang xác nhận xóa người dùng
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = _context.NguoiDung.FirstOrDefault(u => u.MaNguoiDung == id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: Thực hiện xóa người dùng
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var user = _context.NguoiDung.FirstOrDefault(u => u.MaNguoiDung == id);
            if (user == null)
                return NotFound();

            // Kiểm tra xem người dùng có liên quan đến bảng Mượn Trả không
            bool hasBorrowedBooks = _context.MuonTra.Any(mt => mt.MaNguoiDung == id);

            if (hasBorrowedBooks)
            {
                TempData["ErrorMessage"] = "Không thể xóa người dùng vì có dữ liệu trong bảng Mượn Trả.";
                return RedirectToAction("Index");
            }

            // Nếu không có ràng buộc thì xóa người dùng
            _context.NguoiDung.Remove(user);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Xóa người dùng thành công!";
            return RedirectToAction("Index");
        }

    }
}
