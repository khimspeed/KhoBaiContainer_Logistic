using Model;
namespace BLL {
  public interface IIoTBusiness {
    PhanHoiModel GhiNhanIoT(GhiNhanIoTRequestDto dto);
    List<ThongSoIoTModel> LayLichSuIoT(string containerId, int gioiHan);
    ThongKeDashboardModel LayDashboard();
  }
}
