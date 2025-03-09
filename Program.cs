
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

var envFilePath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
if (File.Exists(envFilePath))
{
    foreach (var line in File.ReadAllLines(envFilePath))
    {
        var parts = line.Split('=', 2);
        if (parts.Length == 2)
        {
            Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
        }
    }
}

var serverName = Environment.GetEnvironmentVariable("SERVER_NAME");
var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");

var connectionString = $"Server={serverName};Database={databaseName};Trusted_Connection=True;TrustServerCertificate=True;";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));



builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Đăng ký Controllers và Views
builder.Services.AddControllersWithViews();

// // Thêm dịch vụ xác thực Cookie
// builder.Services
//     .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//     .AddCookie(options =>
//     {
//         options.LoginPath = "/Account/Login"; // Đường dẫn đến trang đăng nhập
//         options.LogoutPath = "/Account/Logout"; // Đường dẫn đăng xuất
//         options.AccessDeniedPath = "/Account/AccessDenied"; // Đường dẫn khi bị từ chối truy cập
//     });



var app = builder.Build();

// Cấu hình lỗi trong môi trường Production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorizationMiddleware();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
