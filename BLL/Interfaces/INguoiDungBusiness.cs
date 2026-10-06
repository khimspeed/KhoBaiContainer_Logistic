using Model;
namespace BLL {
  public interface INguoiDungBusiness {
    NguoiDungModel? DangNhap(string taiKhoan, string matKhau);
    bool DangKy(TaoTaiKhoanRequestDto dto);
    bool CapNhat(CapNhatNguoiDungRequestDto dto);
    bool XoaMem(string id);
    NguoiDungModel? LayTheoId(string id);
    PhanHoiPhanTrangModel<NguoiDungModel> TimKiem(int trangIndex, int trangSize, string hoTen, string taiKhoan);
  }
}
