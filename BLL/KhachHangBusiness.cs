using DAL;
using Model;

namespace BLL {
  /// <summary>
  /// Business logic cho KhachHang: tạo mới, cập nhật, xóa mềm, tìm kiếm phân trang.
  /// </summary>
  public partial class KhachHangBusiness : IKhachHangBusiness {
    private readonly IKhachHangRepository _repo;

    public KhachHangBusiness(IKhachHangRepository repo) {
      _repo = repo;
    }

    /// <summary>Tạo mới khách hàng từ DTO.</summary>
    public bool TaoMoi(ThemKhachHangRequestDto dto) {
      var m = new KhachHangModel {
        customer_id   = Guid.NewGuid().ToString("N")[..20],
        customer_code = dto.MaKhachHang,
        customer_name = dto.TenKhachHang,
        tax_code      = dto.MaSoThue,
        phone         = dto.SoDienThoai,
        email         = dto.Email,
        address       = dto.DiaChi
      };
      return _repo.TaoMoi(m);
    }

    /// <summary>Cập nhật thông tin khách hàng từ DTO.</summary>
    public bool CapNhat(CapNhatKhachHangRequestDto dto) {
      var m = new KhachHangModel {
        customer_id   = dto.MaKhachHangDb,
        customer_code = dto.MaKhachHang,
        customer_name = dto.TenKhachHang,
        tax_code      = dto.MaSoThue,
        phone         = dto.SoDienThoai,
        email         = dto.Email,
        address       = dto.DiaChi
      };
      return _repo.CapNhat(m);
    }

    /// <summary>Xóa mềm khách hàng theo ID.</summary>
    public bool XoaMem(string id) => _repo.XoaMem(id);

    /// <summary>Lấy khách hàng theo ID.</summary>
    public KhachHangModel? LayTheoId(string id) => _repo.LayTheoId(id);

    /// <summary>Tìm kiếm khách hàng có phân trang theo tên.</summary>
    public PhanHoiPhanTrangModel<KhachHangModel> TimKiem(int ti, int ts, string ten) {
      var list = _repo.TimKiem(ti, ts, out long tongSo, ten);
      return new PhanHoiPhanTrangModel<KhachHangModel> {
        DuLieu           = list,
        TongSoBanGhi     = tongSo,
        TrangHienTai     = ti,
        SoBanGhiMoiTrang = ts
      };
    }
  }
}
