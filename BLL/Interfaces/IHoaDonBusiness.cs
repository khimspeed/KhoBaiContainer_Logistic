using Model;
namespace BLL {
  public interface IHoaDonBusiness {
    PhanHoiModel TaoHopDong(TaoHopDongRequestDto dto);
    bool XoaHopDong(string id);
    HopDongModel? LayHopDongTheoId(string id);
    PhanHoiPhanTrangModel<HopDongModel> TimKiemHopDong(int ti, int ts, string maKhachHang);
    PhanHoiModel TaoHoaDon(TaoHoaDonRequestDto dto);
    bool XoaHoaDon(string id);
    HoaDonModel? LayHoaDonTheoId(string id);
    PhanHoiPhanTrangModel<HoaDonModel> TimKiemHoaDon(int ti, int ts, string maKhachHang, string trangThai);
    PhanHoiModel GhiNhanThanhToan(GhiNhanThanhToanRequestDto dto);
    List<ThanhToanModel> LayThanhToanTheoHoaDon(string invoiceId);
    decimal TinhPhiLuuBai(ContainerModel container, HopDongModel hopDong);
  }
}
