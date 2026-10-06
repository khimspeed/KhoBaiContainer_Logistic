using System.ComponentModel.DataAnnotations;

namespace Model
{
    /// <summary>
    /// Model người dùng hệ thống
    /// </summary>
    public class NguoiDungModel
    {
        public string user_id { get; set; } = string.Empty;
        public string? hoten { get; set; }
        public DateTime? ngaysinh { get; set; }
        public string? diachi { get; set; }
        public string? gioitinh { get; set; }
        public string? email { get; set; }
        public string? taikhoan { get; set; }
        public string? matkhau { get; set; }
        public string? role { get; set; }
        public string? image_url { get; set; }
        public DateTime created_at { get; set; }
        public bool is_deleted { get; set; }
        public string? token { get; set; } // dùng sau đăng nhập
        public long RecordCount { get; set; }
        public long RowNumber { get; set; }
    }

    /// <summary>
    /// DTO yêu cầu đăng nhập
    /// </summary>
    public class DangNhapRequestDto
    {
        [Required] public string TaiKhoan { get; set; } = null!;
        [Required] public string MatKhau { get; set; } = null!;
    }

    /// <summary>
    /// DTO yêu cầu tạo tài khoản mới
    /// </summary>
    public class TaoTaiKhoanRequestDto
    {
        [Required] public string HoTen { get; set; } = null!;
        public DateTime? NgaySinh { get; set; }
        public string? DiaChi { get; set; }
        public string? GioiTinh { get; set; }
        public string? Email { get; set; }
        [Required] public string TaiKhoan { get; set; } = null!;
        [Required] public string MatKhau { get; set; } = null!;
        public string VaiTro { get; set; } = "Staff";
        public string? HinhAnhUrl { get; set; }
    }

    /// <summary>
    /// DTO yêu cầu cập nhật thông tin người dùng
    /// </summary>
    public class CapNhatNguoiDungRequestDto
    {
        [Required] public string MaNguoiDung { get; set; } = null!;
        public string? HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? DiaChi { get; set; }
        public string? GioiTinh { get; set; }
        public string? Email { get; set; }
        public string? VaiTro { get; set; }
        public string? HinhAnhUrl { get; set; }
    }
}
