CREATE DATABASE DB_QuanLyThuVien
go
use DB_QuanLyThuVien

CREATE TABLE tblTaiLieu (
    sMaTaiLieu VARCHAR(20) PRIMARY KEY,
    sTenTaiLieu NVARCHAR(100) NOT NULL,
    sAnhURL VARCHAR(255),
    sTenTacGia NVARCHAR(100),
    iNamXuatBan INT,
    sNXB NVARCHAR(100),
    sTheLoai NVARCHAR(100),
    iSoLuong INT CHECK (iSoLuong >= 0),
    sTinhTrang NVARCHAR(50) CHECK (sTinhTrang IN (N'Có sẵn',N'Đang mượn',N'Bị mất',N'Hư hỏng')),
    sViTri NVARCHAR(50)
);

CREATE TABLE tblNguoiDung (
    sMaNguoiDung VARCHAR(20) PRIMARY KEY,
    sHoTen NVARCHAR(100) NOT NULL,
    sSDT VARCHAR(12) UNIQUE,
    sMatKhau VARCHAR(255) NOT NULL,
    sEmail NVARCHAR(100) UNIQUE,
    sLoaiNguoiDung NVARCHAR(100) CHECK (sLoaiNguoiDung IN (N'Độc giả',N'Thủ thư',N'Quản trị viên',N'Cán bộ chuyên môn'))
);

CREATE TABLE tblMuonTra (
    sMaMuonTra VARCHAR(20) PRIMARY KEY,
    sMaNguoiDung VARCHAR(20),
    dNgayMuon DATETIME NOT NULL,
    dNgayHenTra DATETIME NOT NULL,
    dNgayTraThucTe DATETIME,
    sTinhTrang NVARCHAR(100) CHECK (sTinhTrang IN (N'Đang mượn',N'Đã trả',N'Quá hạn')),
    FOREIGN KEY (sMaNguoiDung) REFERENCES tblNguoiDung(sMaNguoiDung)
);

CREATE TABLE tblChiTietMuonTra (
    sMaChiTiet VARCHAR(20) PRIMARY KEY,
    sMaMuonTra VARCHAR(20),
    sMaTaiLieu VARCHAR(20),
    iSoluongMuon INT NOT NULL,
    sTinhTrangMuon NVARCHAR(100) CHECK (sTinhTrangMuon IN (N'Mới',N'Hư hỏng',N'Cũ')),
    iSoLuongTra INT,
    sTinhTrangTra NVARCHAR(100) CHECK (sTinhTrangTra IN (N'Tốt',N'Hư hỏng',N'Mất')),
    fPhiPhat FLOAT,
    FOREIGN KEY (sMaMuonTra) REFERENCES tblMuonTra(sMaMuonTra),
    FOREIGN KEY (sMaTaiLieu) REFERENCES tblTaiLieu(sMaTaiLieu)
);

CREATE TABLE tblXuLyViPham (
    sMaViPham VARCHAR(20) PRIMARY KEY,
    sMaMuonTra VARCHAR(20),
    sMaNguoiDung VARCHAR(20),
    sMaChiTiet VARCHAR(20),
    sLoaiViPham NVARCHAR(100),
    fPhiPhat FLOAT,
    dNgayXuLy DATETIME NOT NULL,
    sTrangThaiXuLy NVARCHAR(100) CHECK (sTrangThaiXuLy IN (N'Chưa xử lý',N'Đã xử lý')),
    FOREIGN KEY (sMaMuonTra) REFERENCES tblMuonTra(sMaMuonTra),
    FOREIGN KEY (sMaNguoiDung) REFERENCES tblNguoiDung(sMaNguoiDung),
    FOREIGN KEY (sMaChiTiet) REFERENCES tblChiTietMuonTra(sMaChiTiet)
);

CREATE TABLE tblTaiKhoan (
    sMaTaiKhoan VARCHAR(20) PRIMARY KEY,
    sMaNguoiDung VARCHAR(20),
    sTenDangNhap NVARCHAR(100) UNIQUE NOT NULL,
    sMatKhau VARCHAR(255) NOT NULL,
    FOREIGN KEY (sMaNguoiDung) REFERENCES tblNguoiDung(sMaNguoiDung)
);

CREATE TABLE tblBaoCaoThongKe (
    sMaBaoCao VARCHAR(20) PRIMARY KEY,
    dNgayBaoCao DATETIME NOT NULL,
    sMaNguoiLap VARCHAR(20),
    iTongSoTL INT,
    iTongSachDangMuon INT,
    iTongSachBiMat INT,
    iTongSachHuHong INT,
    iSoLuotMuonHomNay INT,
    iSoLuongTraHomNay INT
	FOREIGN KEY (sMaNguoiLap) REFERENCES tblNguoiDung(sMaNguoiDung)
);

-- Thêm dữ liệu cho bảng tblTaiLieu
INSERT INTO tblTaiLieu VALUES
('TL001', N'Lập trình C++', 'https://nxbkhkt.com.vn/wp-content/uploads/2022/07/ngon-ngu-cc.jpg', N'Nguyễn Văn A', 2020, N'NXB Giáo Dục', N'Công nghệ thông tin', 15, N'Có sẵn', N'Kệ A1'),
('TL002', N'Lập trình Java', 'https://images.nxbxaydung.com.vn/Picture/2020/biasachnen-0616154230.png', N'Trần Thị B', 2019, N'NXB Trẻ', N'Công nghệ thông tin', 30, N'Có sẵn', N'Kệ A2'),
('TL003', N'Python cho người mới', 'https://cdn0166.cdn4s.com/media/moon/2-lap-trinh-python.jpg', N'Hoàng C', 2021, N'NXB Thanh Niên', N'Công nghệ thông tin', 20, N'Có sẵn', N'Kệ A3'),
('TL004', N'Thiết kế Web', 'https://vietbooks.info/attachments/upload_2021-9-21_23-49-38-png.2102/', N'Nguyễn D', 2018, N'NXB Đại học', N'Thiết kế', 17, N'Có sẵn', N'Kệ B1'),
('TL005', N'Cấu trúc dữ liệu', 'https://images.nxbbachkhoa.vn/Picture/2022/6/8/image-20220608081714868.jpg', N'Nguyễn E', 2017, N'NXB Thống Kê', N'Công nghệ thông tin', 20, N'Có sẵn', N'Kệ C1'),
('TL006', N'Trí tuệ nhân tạo', 'https://product.hstatic.net/200000900535/product/bia-tri-tue-nhan-tao-trong-marketing-1_4e3e0f3c4349495ab512d34f2594b7aa_1024x1024.jpg', N'Phạm F', 2022, N'NXB Khoa học', N'Công nghệ thông tin', 16, N'Có sẵn', N'Kệ A4'),
('TL007', N'Học máy cơ bản', 'https://vn-test-11.slatic.net/p/fa79412af546423ea73268a5164c7d8c.jpg', N'Lê G', 2023, N'NXB Đại học', N'Công nghệ thông tin', 14, N'Có sẵn', N'Kệ A5'),
('TL008', N'Lập trình di động', 'https://images.nxbxaydung.com.vn/Picture/2020/biasach-0626100809.jpg', N'Bùi H', 2019, N'NXB Trẻ', N'Công nghệ thông tin', 10, N'Có sẵn', N'Kệ B2'),
('TL009', N'Quản lý cơ sở dữ liệu', 'https://images.nxbxaydung.com.vn/Picture/2020/he-quan-tri-co-so-du-lieu-access-1029152401.jpg', N'Ngô I', 2020, N'NXB Thống Kê', N'Công nghệ thông tin', 12, N'Có sẵn', N'Kệ C2'),
('TL010', N'An toàn thông tin', 'https://product.hstatic.net/200000463807/product/329_afd38521f14d4cd58e97d1e6fbb85bcf_1024x1024.jpg', N'Đặng J', 2016, N'NXB Giáo Dục', N'Bảo mật', 5, N'Có sẵn', N'Kệ B3');

-- Thêm dữ liệu cho bảng tblNguoiDung
INSERT INTO tblNguoiDung VALUES
('ND001', N'Nguyễn Văn A', '0909123456', 'pass123', 'a@gmail.com', N'Độc giả'),
('ND002', N'Trần Thị B', '0911234567', 'pass234', 'b@gmail.com', N'Thủ thư'),
('ND003', N'Hoàng C', '0921345678', 'pass345', 'c@gmail.com', N'Quản trị viên'),
('ND004', N'Nguyễn D', '0931456789', 'pass456', 'd@gmail.com', N'Độc giả'),
('ND005', N'Phạm E', '0941567890', 'pass567', 'e@gmail.com', N'Cán bộ chuyên môn'),
('ND006', N'Lê F', '0951678901', 'pass678', 'f@gmail.com', N'Độc giả'),
('ND007', N'Bùi G', '0961789012', 'pass789', 'g@gmail.com', N'Quản trị viên'),
('ND008', N'Ngô H', '0971890123', 'pass890', 'h@gmail.com', N'Thủ thư'),
('ND009', N'Đặng I', '0981901234', 'pass901', 'i@gmail.com', N'Cán bộ chuyên môn'),
('ND010', N'Phan J', '0992012345', 'pass012', 'j@gmail.com', N'Độc giả');

-- Thêm dữ liệu cho bảng tblMuonTra
INSERT INTO tblMuonTra VALUES
('MT001', 'ND001', '2025-02-01', '2025-03-15', NULL, N'Đang mượn'),
('MT002', 'ND002', '2025-01-10', '2025-01-25', '2025-01-25', N'Đã trả'),
('MT003', 'ND004', '2025-02-05', '2025-03-20', NULL, N'Đang mượn'),
('MT004', 'ND006', '2025-01-20', '2025-02-05', '2025-02-04', N'Đã trả'),
('MT005', 'ND008', '2025-01-15', '2025-01-30', '2025-02-02', N'Đã trả'),
('MT006', 'ND001', '2025-02-10', '2025-02-25', NULL, N'Đang mượn'),
('MT007', 'ND003', '2025-01-22', '2025-02-07', '2025-02-08', N'Đã trả'),
('MT008', 'ND007', '2025-01-30', '2025-02-14', NULL, N'Đang mượn'),
('MT009', 'ND005', '2025-02-12', '2025-02-27', NULL, N'Đang mượn'),
('MT010', 'ND009', '2025-02-01', '2025-02-16', NULL, N'Đang mượn');


INSERT INTO tblChiTietMuonTra VALUES
('CT001', 'MT001', 'TL001', 1, N'Mới', NULL, NULL, NULL),
('CT002', 'MT001', 'TL002', 1, N'Cũ', NULL, NULL, NULL),
('CT003', 'MT001', 'TL003', 1, N'Hư hỏng', NULL, NULL, NULL),
('CT004', 'MT002', 'TL004', 2, N'Mới', 2, N'Tốt', 0),
('CT005', 'MT002', 'TL005', 1, N'Cũ', 1, N'Hư hỏng', 50000),
('CT006', 'MT002', 'TL006', 1, N'Mới', 1, N'Tốt', 0),
('CT007', 'MT003', 'TL007', 1, N'Cũ', NULL, NULL, NULL),
('CT008', 'MT003', 'TL008', 2, N'Mới', NULL, NULL, NULL),
('CT009', 'MT004', 'TL009', 1, N'Mới', NULL, NULL, NULL),
('CT010', 'MT004', 'TL007', 1, N'Cũ', NULL, NULL, NULL),
('CT011', 'MT005', 'TL008', 2, N'Mới', NULL, NULL, NULL),
('CT012', 'MT006', 'TL009', 1, N'Mới', NULL, NULL, NULL),
('CT013', 'MT006', 'TL007', 1, N'Cũ', NULL, NULL, NULL),
('CT014', 'MT006', 'TL008', 2, N'Mới', NULL, NULL, NULL),
('CT015', 'MT006', 'TL009', 1, N'Mới', NULL, NULL, NULL),
('CT016', 'MT007', 'TL007', 1, N'Cũ', NULL, NULL, NULL),
('CT017', 'MT007', 'TL008', 2, N'Mới', NULL, NULL, NULL),
('CT018', 'MT008', 'TL009', 1, N'Mới', NULL, NULL, NULL),
('CT019', 'MT009', 'TL007', 1, N'Cũ', NULL, NULL, NULL),
('CT020', 'MT010', 'TL008', 2, N'Mới', NULL, NULL, NULL),
('CT021', 'MT010', 'TL009', 1, N'Mới', NULL, NULL, NULL);

-- Thêm dữ liệu cho bảng tblXuLyViPham
INSERT INTO tblXuLyViPham VALUES
('VP001', 'MT001', 'ND001', 'CT005', N'Hư hỏng sách', 50000, '2024-02-20', N'Đã xử lý');


INSERT INTO tblTaiKhoan VALUES
('TK001','ND003', 'Admin', '123456'),
('TK002','ND001', 'NguoiDung1', '123456')

go
CREATE PROCEDURE usp_InsertNguoiDung
    @HoTen NVARCHAR(100),
    @SDT NVARCHAR(50),
    @MatKhau NVARCHAR(100),
    @Email NVARCHAR(100),
    @LoaiNguoiDung NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MaxId INT;
    DECLARE @NewId NVARCHAR(10);

    SELECT @MaxId = MAX(CAST(SUBSTRING(sMaNguoiDung, 3, LEN(sMaNguoiDung)) AS INT))
    FROM tblNguoiDung
    WHERE sMaNguoiDung LIKE 'ND%';

    SET @MaxId = ISNULL(@MaxId, 0) + 1;
    
    SET @NewId = 'ND' + RIGHT('000' + CAST(@MaxId AS VARCHAR(10)), 3);

    INSERT INTO tblNguoiDung (sMaNguoiDung, sHoTen, sSDT, sMatKhau, sEmail, sLoaiNguoiDung)
    VALUES (@NewId, @HoTen, @SDT, @MatKhau, @Email, @LoaiNguoiDung);

    SELECT @NewId AS NewMaNguoiDung;
END