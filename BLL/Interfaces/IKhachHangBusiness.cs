using Model;
namespace BLL {
  public interface IKhachHangBusiness {
    bool TaoMoi(ThemKhachHangRequestDto dto);
    bool CapNhat(CapNhatKhachHangRequestDto dto);
    bool XoaMem(string id);
    KhachHangModel? LayTheoId(string id);
    PhanHoiPhanTrangModel<KhachHangModel> TimKiem(int ti, int ts, string ten);
  }
}
