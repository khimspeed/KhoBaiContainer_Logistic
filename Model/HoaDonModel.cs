using System.ComponentModel.DataAnnotations;

namespace Model
{
    /// <summary>
    /// Model hợp đồng lưu bãi
    /// </summary>
    public class HopDongModel
    {
        public string contract_id { get; set; } = string.Empty;
        public string so_hop_dong { get; set; } = string.Empty;
        public string customer_id { get; set; } = string.Empty;
        public string warehouse_id { get; set; } = string.Empty;
        public DateTime ngay_bat_dau { get; set; }
        public DateTime ngay_ket_thuc { get; set; }
        public decimal don_gia_luu_bai { get; set; }
        public string trang_thai { get; set; } = string.Empty;
        public bool is_deleted { get; set; }

        // Trường join
        public string? ten_khach_hang { get; set; }
        public string? ten_kho { get; set; }
        public long RecordCount { get; set; }
        public long RowNumber { get; set; }
    }

    /// <summary>
    /// Model hóa đơn thanh toán
    /// </summary>
    public class HoaDonModel
    {
        public string invoice_id { get; set; } = string.Empty;
        public string so_hoa_don { get; set; } = string.Empty;
        public string? contract_id { get; set; }
        public string customer_id { get; set; } = string.Empty;
        public DateTime ngay_lap { get; set; }
        public DateTime? han_thanh_toan { get; set; }
        public decimal so_tien { get; set; }
        public string trang_thai { get; set; } = string.Empty;
        public bool is_deleted { get; set; }

        // Trường join
        public string? ten_khach_hang { get; set; }
        public string? so_hop_dong { get; set; }
        public long RecordCount { get; set; }
        public long RowNumber { get; set; }
    }

    /// <summary>
    /// Model thanh toán hóa đơn
    /// </summary>
    public class ThanhToanModel
    {
        public string payment_id { get; set; } = string.Empty;
        public string invoice_id { get; set; } = string.Empty;
        public DateTime ngay_thanh_toan { get; set; }
        public decimal so_tien { get; set; }
        public string? phuong_thuc { get; set; }
        public string? ghi_chu { get; set; }
        public long RecordCount { get; set; }
        public long RowNumber { get; set; }
    }

    /// <summary>
    /// DTO yêu cầu tạo hợp đồng lưu bãi
    /// </summary>
    public class TaoHopDongRequestDto
    {
        [Required] public string SoHopDong { get; set; } = null!;
        [Required] public string MaKhachHangDb { get; set; } = null!;
        [Required] public string MaKhoDb { get; set; } = null!;
        [Required] public DateTime NgayBatDau { get; set; }
        [Required] public DateTime NgayKetThuc { get; set; }
        [Required] public decimal DonGiaLuuBai { get; set; }
    }

    /// <summary>
    /// DTO yêu cầu tạo hóa đơn
    /// </summary>
    public class TaoHoaDonRequestDto
    {
        [Required] public string MaKhachHangDb { get; set; } = null!;
        public string? MaHopDongDb { get; set; }
        [Required] public decimal SoTien { get; set; }
        public int SoNgayHanThanhToan { get; set; } = 15;
    }

    /// <summary>
    /// DTO yêu cầu ghi nhận thanh toán hóa đơn
    /// </summary>
    public class GhiNhanThanhToanRequestDto
    {
        [Required] public string MaHoaDonDb { get; set; } = null!;
        [Required] public decimal SoTien { get; set; }
        public string PhuongThuc { get; set; } = "ChuyenKhoan";
        public string? GhiChu { get; set; }
    }

    /// <summary>
    /// DTO yêu cầu tính phí lưu bãi cho container
    /// </summary>
    public class TinhPhiLuuBaiRequestDto
    {
        [Required] public string MaContainerDb { get; set; } = null!;
        [Required] public string MaHopDongDb { get; set; } = null!;
    }
}
