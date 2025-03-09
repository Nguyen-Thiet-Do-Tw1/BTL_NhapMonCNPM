using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

public class AdminController : Controller
{
    public IActionResult Index()
    {
        var loaiNguoiDung = HttpContext.Session.GetString("LoaiNguoiDung");

        if (loaiNguoiDung != "Quản trị viên")
        {
            return RedirectToAction("Login", "Account");
        }

        return View();
    }
}
