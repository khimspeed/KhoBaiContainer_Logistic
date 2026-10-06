using Model;
namespace BLL {
  public interface IContainerBusiness {
    bool TaoMoi(ContainerModel m);
    bool CapNhat(ContainerModel m);
    bool XoaMem(string id);
    ContainerModel? LayTheoId(string id);
    PhanHoiPhanTrangModel<ContainerModel> TimKiem(int ti, int ts, string soContainer, string trangThai, string maKho);
    PhanHoiModel NhapBai(NhapBaiRequestDto dto, string nguoiThucHien);
    PhanHoiModel XuatBai(XuatBaiRequestDto dto, string nguoiThucHien);
    PhanHoiModel ChuyenViTri(ChuyenViTriRequestDto dto, string nguoiThucHien);
    PhanHoiPhanTrangModel<LichSuDiChuyenModel> LayLichSuDiChuyen(string containerId, int ti, int ts);
    List<ContainerModel> LayContainerQuaHan();
    List<LoaiContainerModel> LayDanhSachLoaiContainer();
    List<LoaiHangHoaModel> LayDanhSachLoaiHang();
  }
}
