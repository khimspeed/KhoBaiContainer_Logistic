using Model;

namespace DAL
{
    /// <summary>
    /// Interface định nghĩa các thao tác với dữ liệu IoT (cảm biến container)
    /// và thống kê tổng quan dashboard.
    /// </summary>
    public interface IIoTRepository
    {
        /// <summary>Ghi nhận thông số IoT (nhiệt độ, độ ẩm) cho container.</summary>
        bool GhiNhan(ThongSoIoTModel m);

        /// <summary>Lấy danh sách thông số IoT theo container với giới hạn số bản ghi.</summary>
        List<ThongSoIoTModel> LayTheoContainer(string containerId, int gioi_han);

        /// <summary>Lấy số liệu tổng quan cho dashboard.</summary>
        ThongKeDashboardModel LayThongKeDashboard();
    }
}
