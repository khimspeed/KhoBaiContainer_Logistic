using System.ComponentModel.DataAnnotations;

namespace Model
{
    /// <summary>
    /// Model loại container (20ft, 40ft, v.v.)
    /// </summary>
    public class LoaiContainerModel
    {
        public string type_id { get; set; } = string.Empty;
        public string type_code { get; set; } = string.Empty;
        public string type_name { get; set; } = string.Empty;
        public decimal teu { get; set; }
        public long RecordCount { get; set; }
        public long RowNumber { get; set; }
    }

    /// <summary>
    /// Model loại hàng hóa (thường, lạnh, nguy hiểm, v.v.)
    /// </summary>
    public class LoaiHangHoaModel
    {
        public string cargo_type_id { get; set; } = string.Empty;
        public string cargo_code { get; set; } = string.Empty;
        public string cargo_name { get; set; } = string.Empty;
        public bool need_reefer { get; set; }
        public long RecordCount { get; set; }
        public long RowNumber { get; set; }
    }

    /// <summary>
    /// Model container, bao gồm thông tin join từ các bảng liên quan
    /// </summary>
    public class ContainerModel
    {
        public string container_id { get; set; } = string.Empty;
        public string container_no { get; set; } = string.Empty;
        public string type_id { get; set; } = string.Empty;
        public string? cargo_type_id { get; set; }
        public string? warehouse_id { get; set; }
        public string? slot_id { get; set; }
        public string? customer_id { get; set; }
        public string trang_thai { get; set; } = string.Empty;       // maps: status
        public DateTime? thoi_gian_vao_bai { get; set; }             // maps: check_in_at
        public DateTime? mien_phi_den_ngay { get; set; }             // maps: free_until
        public bool da_qua_han { get; set; }                         // maps: is_overdue
        public bool is_deleted { get; set; }

        // Trường join từ bảng khác
        public string? ten_loai_container { get; set; }
        public decimal teu { get; set; }
        public string? ten_loai_hang { get; set; }
        public string? ten_kho { get; set; }
        public string? ma_vi_tri { get; set; }
        public string? ten_khach_hang { get; set; }
        public long RecordCount { get; set; }
        public long RowNumber { get; set; }
    }

    /// <summary>
    /// Model lịch sử di chuyển container (nhập bãi, xuất bãi, chuyển vị trí)
    /// </summary>
    public class LichSuDiChuyenModel
    {
        public string movement_id { get; set; } = string.Empty;
        public string container_id { get; set; } = string.Empty;
        public string? warehouse_id { get; set; }
        public string? from_slot_id { get; set; }
        public string? to_slot_id { get; set; }
        public string loai_di_chuyen { get; set; } = string.Empty;   // IN / OUT / RELOCATE
        public DateTime thoi_gian_di_chuyen { get; set; }
        public string? nguoi_thuc_hien { get; set; }
        public string? ghi_chu { get; set; }

        // Trường join
        public string? so_container { get; set; }
        public string? ten_kho { get; set; }
        public long RecordCount { get; set; }
        public long RowNumber { get; set; }
    }

    /// <summary>
    /// DTO yêu cầu nhập container vào bãi
    /// </summary>
    public class NhapBaiRequestDto
    {
        [Required] public string SoContainer { get; set; } = null!;
        [Required] public string MaLoaiContainerDb { get; set; } = null!;
        public string? MaLoaiHangDb { get; set; }
        [Required] public string MaKhoDb { get; set; } = null!;
        [Required] public string MaViTriDb { get; set; } = null!;
        [Required] public string MaKhachHangDb { get; set; } = null!;
        public int SoNgayMienPhi { get; set; } = 7;
        public string? GhiChu { get; set; }
    }

    /// <summary>
    /// DTO yêu cầu xuất container ra khỏi bãi
    /// </summary>
    public class XuatBaiRequestDto
    {
        [Required] public string MaContainerDb { get; set; } = null!;
        public string? GhiChu { get; set; }
    }

    /// <summary>
    /// DTO yêu cầu chuyển vị trí container trong bãi
    /// </summary>
    public class ChuyenViTriRequestDto
    {
        [Required] public string MaContainerDb { get; set; } = null!;
        [Required] public string MaViTriMoiDb { get; set; } = null!;
        public string? GhiChu { get; set; }
    }
}
