using System.ComponentModel.DataAnnotations;

namespace Model
{
    /// <summary>
    /// Model thông số IoT thu thập từ sensor gắn trên container
    /// </summary>
    public class ThongSoIoTModel
    {
        public string reading_id { get; set; } = string.Empty;
        public string container_id { get; set; } = string.Empty;
        public decimal? nhiet_do { get; set; }
        public decimal? do_am { get; set; }
        public DateTime thoi_gian_ghi { get; set; }

        // Trường join
        public string? so_container { get; set; }
        public long RecordCount { get; set; }
        public long RowNumber { get; set; }
    }

    /// <summary>
    /// DTO yêu cầu ghi nhận dữ liệu IoT từ sensor
    /// </summary>
    public class GhiNhanIoTRequestDto
    {
        [Required] public string MaContainerDb { get; set; } = null!;
        public decimal? NhietDo { get; set; }
        public decimal? DoAm { get; set; }
    }

    /// <summary>
    /// Model thống kê tổng quan Dashboard
    /// </summary>
    public class ThongKeDashboardModel
    {
        public int TongSoContainer { get; set; }
        public int ContainerTrongBai { get; set; }
        public int ContainerDangVanChuyen { get; set; }
        public int ContainerQuaHan { get; set; }
        public int HoaDonChuaThanhToan { get; set; }
        public decimal TongDoanhThu { get; set; }
        public decimal TeuDaDung { get; set; }
        public decimal TongSucChuaTeu { get; set; }
    }
}
