using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace QuanLyThuVien.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.ToString().ToLower();
            var loaiNguoiDung = context.Session.GetString("LoaiNguoiDung");

            // Nếu chưa đăng nhập, chặn truy cập vào các trang yêu cầu đăng nhập
            if (string.IsNullOrEmpty(loaiNguoiDung) && 
                (path.StartsWith("/Admin") || path.StartsWith("/tailieu")))
            {
                context.Response.Redirect("/Account/Login");
                return;
            }

            // Nếu không phải quản trị viên, chặn truy cập trang Admin
            if (path.StartsWith("/Admin") && loaiNguoiDung != "Quản trị viên")
            {
                context.Response.Redirect("/Account/Login");
                return;
            }

            await _next(context);
        }
    }

    // Middleware extension để đăng ký Middleware trong pipeline
    public static class AuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthorizationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizationMiddleware>();
        }
    }
}
