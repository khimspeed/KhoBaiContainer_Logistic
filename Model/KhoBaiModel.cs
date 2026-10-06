using System.ComponentModel.DataAnnotations;

namespace Model
{
    /// <summary>
    /// Model kho bãi container
    /// </summary>
    public class KhoBaiModel
    {
        public string warehouse_id { get; set; } = string.Empty;
        public string warehouse_code { get; set; } = string.Empty;
        public string warehouse_name { get; set; } = string.Empty;
        public string? address { get; set; }
        public decimal capacity_teu { get; set; }
        public decimal used_teu { get; set; }
        public bool is_deleted { get; set; }
        public long RecordCount { get; set; }
        public long RowNumber { get; set; }
    }

    /// <summary>
    /// Model khu vực trong kho bãi
    /// </summary>
    public class KhuVucKhoModel
    {
        public string zone_id { get; set; } = string.Empty;
        public string warehouse_id { get; set; } = string.Empty;
        public string zone_code { get; set; } = string.Empty;
        public string zone_name { get; set; } = string.Empty;
        public long RecordCount { get; set; }
        public long RowNumber { get; set; }
    }

    /// <summary>
    /// Model vị trí (slot) trong khu vực kho
    /// </summary>
    public class ViTriKhoModel
    {
        public string slot_id { get; set; } = string.Empty;
        public string zone_id { get; set; } = string.Empty;
        public string slot_code { get; set; } = string.Empty;
        public decimal max_teu { get; set; }
        public bool is_occupied { get; set; }
        public long RecordCount { get; set; }
        public long RowNumber { get; set; }
    }

    /// <summary>
    /// DTO yêu cầu thêm kho bãi mới
    /// </summary>
    public class ThemKhoBaiRequestDto
    {
        [Required] public string MaKho { get; set; } = null!;
        [Required] public string TenKho { get; set; } = null!;
        public string? DiaChi { get; set; }
        [Required] public decimal SucChuaTeu { get; set; }
    }

    /// <summary>
    /// DTO yêu cầu thêm khu vực vào kho
    /// </summary>
    public class ThemKhuVucRequestDto
    {
        [Required] public string MaKhoDb { get; set; } = null!;
        [Required] public string MaKhuVuc { get; set; } = null!;
        [Required] public string TenKhuVuc { get; set; } = null!;
    }

    /// <summary>
    /// DTO yêu cầu thêm vị trí vào khu vực
    /// </summary>
    public class ThemViTriRequestDto
    {
        [Required] public string MaKhuVucDb { get; set; } = null!;
        [Required] public string MaViTri { get; set; } = null!;
        [Required] public decimal TeuToiDa { get; set; }
    }
}
