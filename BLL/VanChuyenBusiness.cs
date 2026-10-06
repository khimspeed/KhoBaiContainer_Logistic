using DAL;
using Model;

namespace BLL {
  /// <summary>
  /// Business logic cho Vận Chuyển: phương tiện, tàu biển, chuyến hàng và chặng vận chuyển.
  /// </summary>
  public partial class VanChuyenBusiness : IVanChuyenBusiness {
    private readonly IVanChuyenRepository _repo;

    public VanChuyenBusiness(IVanChuyenRepository repo) {
      _repo = repo;
    }

    // ─── Phương tiện ─────────────────────────────────────────────────────────

    /// <summary>Tạo mới phương tiện (sinh ID tự động).</summary>
    public bool TaoPhuongTien(PhuongTienModel m) {
      m.vehicle_id = Guid.NewGuid().ToString("N")[..20];
      return _repo.TaoPhuongTien(m);
    }

    /// <summary>Cập nhật thông tin phương tiện.</summary>
    public bool CapNhatPhuongTien(PhuongTienModel m) => _repo.CapNhatPhuongTien(m);

    /// <summary>Lấy danh sách phương tiện có phân trang, lọc theo biển số.</summary>
    public PhanHoiPhanTrangModel<PhuongTienModel> LayDanhSachPhuongTien(int ti, int ts, string bienSo) {
      var list = _repo.LayDanhSachPhuongTien(ti, ts, out long tongSo, bienSo);
      return new PhanHoiPhanTrangModel<PhuongTienModel> {
        DuLieu           = list,
        TongSoBanGhi     = tongSo,
        TrangHienTai     = ti,
        SoBanGhiMoiTrang = ts
      };
    }

    // ─── Tàu biển ────────────────────────────────────────────────────────────

    /// <summary>Tạo mới tàu biển (sinh ID tự động).</summary>
    public bool TaoTauBien(TauBienModel m) {
      m.vessel_id = Guid.NewGuid().ToString("N")[..20];
      return _repo.TaoTauBien(m);
    }

    /// <summary>Lấy danh sách tàu biển có phân trang.</summary>
    public PhanHoiPhanTrangModel<TauBienModel> LayDanhSachTauBien(int ti, int ts) {
      var list = _repo.LayDanhSachTauBien(ti, ts, out long tongSo);
      return new PhanHoiPhanTrangModel<TauBienModel> {
        DuLieu           = list,
        TongSoBanGhi     = tongSo,
        TrangHienTai     = ti,
        SoBanGhiMoiTrang = ts
      };
    }

    // ─── Chuyến hàng ─────────────────────────────────────────────────────────

    /// <summary>
    /// Tạo mới chuyến hàng, sinh mã vận đơn tự động theo định dạng TRK-{năm}-{6 ký tự}.
    /// </summary>
    public PhanHoiModel TaoChuyenHang(TaoChuyenHangRequestDto dto) {
      try {
        var m = new ChuyenHangModel {
          shipment_id  = Guid.NewGuid().ToString("N")[..20],
          ma_van_don   = "TRK-" + DateTime.Now.Year + "-" + Guid.NewGuid().ToString("N")[..6].ToUpper(),
          customer_id  = dto.MaKhachHangDb,
          diem_di      = dto.DiemDi,
          diem_den     = dto.DiemDen,
          du_kien_den  = dto.DuKienDen,
          trang_thai   = "Planned"
        };
        _repo.TaoChuyenHang(m);
        return PhanHoiModel.OK(
          new { MaChuyenHang = m.shipment_id, MaVanDon = m.ma_van_don },
          "Tạo chuyến hàng thành công");
      } catch (Exception ex) {
        return PhanHoiModel.Loi(ex.Message);
      }
    }

    /// <summary>Cập nhật trạng thái chuyến hàng.</summary>
    public bool CapNhatTrangThaiChuyenHang(string id, string trangThai) =>
      _repo.CapNhatTrangThaiChuyenHang(id, trangThai);

    /// <summary>Xóa chuyến hàng theo ID.</summary>
    public bool XoaChuyenHang(string id) => _repo.XoaChuyenHang(id);

    /// <summary>Lấy chuyến hàng theo ID.</summary>
    public ChuyenHangModel? LayChuyenHangTheoId(string id) => _repo.LayChuyenHangTheoId(id);

    /// <summary>Tìm kiếm chuyến hàng phân trang theo mã vận đơn và trạng thái.</summary>
    public PhanHoiPhanTrangModel<ChuyenHangModel> TimKiemChuyenHang(int ti, int ts, string maVanDon, string trangThai) {
      var list = _repo.TimKiemChuyenHang(ti, ts, out long tongSo, maVanDon, trangThai);
      return new PhanHoiPhanTrangModel<ChuyenHangModel> {
        DuLieu           = list,
        TongSoBanGhi     = tongSo,
        TrangHienTai     = ti,
        SoBanGhiMoiTrang = ts
      };
    }

    // ─── Chặng vận chuyển ────────────────────────────────────────────────────

    /// <summary>Thêm chặng vận chuyển vào chuyến hàng.</summary>
    public PhanHoiModel ThemChang(ThemChangRequestDto dto) {
      try {
        var m = new ChangVanChuyenModel {
          leg_id      = Guid.NewGuid().ToString("N")[..20],
          shipment_id = dto.MaChuyenHang,
          container_id = dto.MaContainerDb,
          vehicle_id  = dto.MaPhuongTien,
          vessel_id   = dto.MaTauDb,
          thu_tu      = dto.ThuTu,
          hinh_thuc   = dto.HinhThuc,
          trang_thai  = "Planned"
        };
        _repo.ThemChang(m);
        return PhanHoiModel.OK(null, "Thêm chặng thành công");
      } catch (Exception ex) {
        return PhanHoiModel.Loi(ex.Message);
      }
    }

    /// <summary>Lấy danh sách chặng theo chuyến hàng.</summary>
    public List<ChangVanChuyenModel> LayChangTheoChuyenHang(string shipmentId) =>
      _repo.LayChangTheoChuyenHang(shipmentId);
  }
}
