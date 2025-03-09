📚 Phần mềm Quản lý Hệ thống Thư viện Trường Đại học Mở Hà Nội

📝 Giới thiệu
Dự án này phát triển một hệ thống quản lý thư viện cho Trường Đại học Mở Hà Nội, giúp tối ưu hóa việc quản lý tài liệu, mượn/trả sách, và thông tin độc giả.

🎯 Tính năng chính

    ✅ Quản lý tài liệu: Thêm, sửa, xóa sách, tìm kiếm  sách.
    ✅ Quản lý mượn/trả: Thêm, sửa, xóa Phiếu mượn, xử lý mượn sách, trả sách , tìm kiếm phiếu mượn
    ✅ Quản lý người dùng: Thêm, sửa, xóa sách, tìm kiếm người dùng.

🛠 Công nghệ sử dụng

    Ngôn ngữ lập trình: C#, ASP.NET Core MVC (.NET 8)
    Cơ sở dữ liệu: SQL Server
    Công cụ phát triển: Visual Studio Code, SQL Server Management Studio
    Quản lý mã nguồn: GitHub
    
🚀 Cài đặt và triển khai

    Yêu cầu hệ thống
        .NET 8.0 SDK
        SQL Server 
        Visual Studio Code
        
Hướng dẫn cài đặt

1. Clone repository
   
        git clone https://github.com/Nguyen-Thiet-Do-Tw1/BTL_NhapMonCNPM.git
   
2. Cấu hình cơ sở dữ liệu
   
      Khởi tạo SQL Server
   
      Mở file Database/SQLQuery.sql trong SQL Server Management Studio và chạy file
        
3. Cấu hình chuỗi kết nối SQL Server

      Vào SQL Server Management Studio lấy tên Server, tên Database
   
      Điền các thông số vào file .env
   
   Ví Dụ:
   
       SERVER_NAME=TIIDII\SQLEXPRESS // thay bằng tên Server của bạn
       DATABASE_NAME=DB_QuanLyThuVien
            
4. Chạy ứng dụng: mở terminal chạy lệnh
       cd QuanLyThuVien
       dotnet run
   
5. Xem kết quả chạy ứng dụng ở
   
       http://localhost:5250
   
📖 Hướng dẫn sử dụng

Đăng nhập theo vai trò: Độc giả, Quản trị viên.

   Tài khoản quản trị viên: 
        
    Tên đăng nhập: admin
    Mật khẩu: 123456
            
   Tài khoản Độc giả: 

    Tên đăng nhập: NguoiDung1
    Mật khẩu: 123456
            
Vai trò độc giả: 
    
   1. Hiện tại Độc giả chỉ xem được Trang chủ, giới thiệu cũng như danh mục tài liệu

   2. Chức năng đăng ký mượn sách, đọc sách trực tuyến của độc giả sẽ được phát triển trong tương lai

Vai trò Quản trị viên

   1. Quản lý tài liệu: Thêm, chỉnh sửa, xóa, tìm kiếm sách.

   2. Mượn & Trả sách: Thêm, sửa, xóa Phiếu mượn, xử lý trả sách.

   3. Quản lý người dùng: thêm, sửa xóa người dùng
