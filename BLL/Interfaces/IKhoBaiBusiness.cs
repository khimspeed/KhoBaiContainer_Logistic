using Model;
namespace BLL {
  public interface IKhoBaiBusiness {
    bool TaoKho(ThemKhoBaiRequestDto dto);
    bool CapNhatKho(string id, ThemKhoBaiRequestDto dto);
    bool XoaKho(string id);
    KhoBaiModel? LayKhoTheoId(string id);
    PhanHoiPhanTrangModel<KhoBaiModel> TimKiemKho(int ti, int ts, string ten);
    bool TaoKhuVuc(ThemKhuVucRequestDto dto);
    List<KhuVucKhoModel> LayKhuVucTheoKho(string warehouseId);
    bool TaoViTri(ThemViTriRequestDto dto);
    List<ViTriKhoModel> LayViTriTheoKhuVuc(string zoneId);
    List<ViTriKhoModel> LayViTriTrong(string warehouseId);
  }
}
