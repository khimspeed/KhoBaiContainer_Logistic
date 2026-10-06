using DAL;
using Model;

namespace BLL {
  /// <summary>
  /// Business logic cho IoT: ghi nhận chỉ số cảm biến container và tổng hợp dashboard.
  /// </summary>
  public partial class IoTBusiness : IIoTBusiness {
    private readonly IIoTRepository _repo;

    public IoTBusiness(IIoTRepository repo) {
      _repo = repo;
    }

    /// <summary>
    /// Ghi nhận dữ liệu IoT (nhiệt độ, độ ẩm) cho container.
    /// Sinh reading_id và đánh dấu thời gian ghi tự động.
    /// </summary>
    public PhanHoiModel GhiNhanIoT(GhiNhanIoTRequestDto dto) {
      try {
        var m = new ThongSoIoTModel {
          reading_id    = Guid.NewGuid().ToString("N")[..20],
          container_id  = dto.MaContainerDb,
          nhiet_do      = dto.NhietDo,
          do_am         = dto.DoAm,
          thoi_gian_ghi = DateTime.Now
        };
        _repo.GhiNhan(m);
        return PhanHoiModel.OK(null, "Ghi nhận IoT thành công");
      } catch (Exception ex) {
        return PhanHoiModel.Loi(ex.Message);
      }
    }

    /// <summary>Lấy lịch sử chỉ số IoT của container, giới hạn số bản ghi trả về.</summary>
    public List<ThongSoIoTModel> LayLichSuIoT(string containerId, int gioiHan) =>
      _repo.LayTheoContainer(containerId, gioiHan);

    /// <summary>Lấy dữ liệu tổng hợp cho dashboard (số container, cảnh báo, v.v.).</summary>
    public ThongKeDashboardModel LayDashboard() => _repo.LayThongKeDashboard();
  }
}
