using System.ComponentModel.DataAnnotations;

namespace Model
{
    /// <summary>
    /// Model phương tiện vận chuyển (xe tải)
    /// </summary>
    public class PhuongTienModel
    {
        public string vehicle_id { get; set; } = string.Empty;
        public string bien_so { get; set; } = string.Empty;   // maps: plate_no
        public string? loai_phuong_tien { get; set; }
        public string? ten_tai_xe { get; set; }
        public string? so_dien_thoai { get; set; }
        public string trang_thai { get; set; } = string.Empty;
        public long RecordCount { get; set; }
        public long RowNumber { get; set; }
    }

    /// <summary>
    /// Model tàu biển
    /// </summary>
    public class TauBienModel
    {
        public string vessel_id { get; set; } = string.Empty;
        public string vessel_code { get; set; } = string.Empty;
        public string vessel_name { get; set; } = string.Empty;
        public string? imo_no { get; set; }
        public long RecordCount { get; set; }
        public long RowNumber { get; set; }
    }

    /// <summary>
    /// Model chuyến hàng (lô hàng vận chuyển)
    /// </summary>
    public class ChuyenHangModel
    {
        public string shipment_id { get; set; } = string.Empty;
        public string ma_van_don { get; set; } = string.Empty;   // maps: tracking_no
        public string? customer_id { get; set; }
        public string? diem_di { get; set; }
        public string? diem_den { get; set; }
        public string trang_thai { get; set; } = string.Empty;
        public DateTime? du_kien_den { get; set; }
        public DateTime ngay_tao { get; set; }
        public bool is_deleted { get; set; }

        // Trường join
        public string? ten_khach_hang { get; set; }
        public long RecordCount { get; set; }
        public long RowNumber { get; set; }
    }

    /// <summary>
    /// Model chặng vận chuyển trong một chuyến hàng
    /// </summary>
    public class ChangVanChuyenModel
    {
        public string leg_id { get; set; } = string.Empty;
        public string shipment_id { get; set; } = string.Empty;
        public string? container_id { get; set; }
        public string? vehicle_id { get; set; }
        public string? vessel_id { get; set; }
        public int thu_tu { get; set; }
        public string hinh_thuc { get; set; } = string.Empty;   // TRUCK / VESSEL
        public string trang_thai { get; set; } = string.Empty;
        public DateTime? thoi_gian_di { get; set; }
        public DateTime? thoi_gian_den { get; set; }

        // Trường join
        public string? so_container { get; set; }
        public string? bien_so { get; set; }
        public string? ten_tau { get; set; }
        public long RecordCount { get; set; }
        public long RowNumber { get; set; }
    }

    /// <summary>
    /// DTO yêu cầu tạo chuyến hàng mới
    /// </summary>
    public class TaoChuyenHangRequestDto
    {
        [Required] public string DiemDi { get; set; } = null!;
        [Required] public string DiemDen { get; set; } = null!;
        [Required] public string MaKhachHangDb { get; set; } = null!;
        public DateTime? DuKienDen { get; set; }
    }

    /// <summary>
    /// DTO yêu cầu thêm chặng vào chuyến hàng
    /// </summary>
    public class ThemChangRequestDto
    {
        [Required] public string MaChuyenHang { get; set; } = null!;
        public string? MaContainerDb { get; set; }
        public string? MaPhuongTien { get; set; }
        public string? MaTauDb { get; set; }
        [Required] public int ThuTu { get; set; }
        [Required] public string HinhThuc { get; set; } = null!;   // TRUCK hoặc VESSEL
    }
}
