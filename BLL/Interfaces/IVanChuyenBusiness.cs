using Model;
namespace BLL {
  public interface IVanChuyenBusiness {
    bool TaoPhuongTien(PhuongTienModel m);
    bool CapNhatPhuongTien(PhuongTienModel m);
    PhanHoiPhanTrangModel<PhuongTienModel> LayDanhSachPhuongTien(int ti, int ts, string bienSo);
    bool TaoTauBien(TauBienModel m);
    PhanHoiPhanTrangModel<TauBienModel> LayDanhSachTauBien(int ti, int ts);
    PhanHoiModel TaoChuyenHang(TaoChuyenHangRequestDto dto);
    bool CapNhatTrangThaiChuyenHang(string id, string trangThai);
    bool XoaChuyenHang(string id);
    ChuyenHangModel? LayChuyenHangTheoId(string id);
    PhanHoiPhanTrangModel<ChuyenHangModel> TimKiemChuyenHang(int ti, int ts, string maVanDon, string trangThai);
    PhanHoiModel ThemChang(ThemChangRequestDto dto);
    List<ChangVanChuyenModel> LayChangTheoChuyenHang(string shipmentId);
  }
}
