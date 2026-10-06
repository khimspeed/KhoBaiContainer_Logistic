using System.ComponentModel.DataAnnotations;

namespace Model
{
    /// <summary>
    /// Model khách hàng
    /// </summary>
    public class KhachHangModel
    {
        public string customer_id { get; set; } = string.Empty;
        public string customer_code { get; set; } = string.Empty;
        public string customer_name { get; set; } = string.Empty;
        public string? tax_code { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public string? address { get; set; }
        public bool is_deleted { get; set; }
        public long RecordCount { get; set; }
        public long RowNumber { get; set; }
    }

    /// <summary>
    /// DTO yêu cầu thêm khách hàng mới
    /// </summary>
    public class ThemKhachHangRequestDto
    {
        [Required] public string MaKhachHang { get; set; } = null!;
        [Required] public string TenKhachHang { get; set; } = null!;
        public string? MaSoThue { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public string? DiaChi { get; set; }
    }

    /// <summary>
    /// DTO yêu cầu cập nhật khách hàng (kế thừa từ ThemKhachHangRequestDto)
    /// </summary>
    public class CapNhatKhachHangRequestDto : ThemKhachHangRequestDto
    {
        [Required] public string MaKhachHangDb { get; set; } = null!;
    }
}
