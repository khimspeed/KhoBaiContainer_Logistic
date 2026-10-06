namespace Model
{
    /// <summary>
    /// Model phản hồi chung cho các API
    /// </summary>
    public class PhanHoiModel
    {
        public bool ThanhCong { get; set; }
        public string ThongBao { get; set; } = string.Empty;
        public object? DuLieu { get; set; }

        public static PhanHoiModel OK(object? data = null, string msg = "Thành công") =>
            new() { ThanhCong = true, ThongBao = msg, DuLieu = data };

        public static PhanHoiModel Loi(string msg) =>
            new() { ThanhCong = false, ThongBao = msg };
    }

    /// <summary>
    /// Model phản hồi có phân trang
    /// </summary>
    public class PhanHoiPhanTrangModel<T>
    {
        public bool ThanhCong { get; set; } = true;
        public List<T> DuLieu { get; set; } = new();
        public long TongSoBanGhi { get; set; }
        public int TrangHienTai { get; set; }
        public int SoBanGhiMoiTrang { get; set; }
    }
}
