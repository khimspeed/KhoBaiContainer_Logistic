using DAL;
using Model;

namespace BLL {
  /// <summary>
  /// Business logic cho KhoBai: quản lý kho, khu vực và vị trí bãi container.
  /// </summary>
  public partial class KhoBaiBusiness : IKhoBaiBusiness {
    private readonly IKhoBaiRepository _repo;

    public KhoBaiBusiness(IKhoBaiRepository repo) {
      _repo = repo;
    }

    // ─── Kho ────────────────────────────────────────────────────────────────

    /// <summary>Tạo mới kho bãi từ DTO.</summary>
    public bool TaoKho(ThemKhoBaiRequestDto dto) {
      var m = new KhoBaiModel {
        warehouse_id   = Guid.NewGuid().ToString("N")[..20],
        warehouse_code = dto.MaKho,
        warehouse_name = dto.TenKho,
        address        = dto.DiaChi,
        capacity_teu   = dto.SucChuaTeu
      };
      return _repo.TaoKho(m);
    }

    /// <summary>Cập nhật thông tin kho bãi.</summary>
    public bool CapNhatKho(string id, ThemKhoBaiRequestDto dto) {
      var m = new KhoBaiModel {
        warehouse_id   = id,
        warehouse_code = dto.MaKho,
        warehouse_name = dto.TenKho,
        address        = dto.DiaChi,
        capacity_teu   = dto.SucChuaTeu
      };
      return _repo.CapNhatKho(m);
    }

    /// <summary>Xóa kho bãi theo ID.</summary>
    public bool XoaKho(string id) => _repo.XoaKho(id);

    /// <summary>Lấy kho bãi theo ID.</summary>
    public KhoBaiModel? LayKhoTheoId(string id) => _repo.LayKhoTheoId(id);

    /// <summary>Tìm kiếm kho bãi có phân trang theo tên.</summary>
    public PhanHoiPhanTrangModel<KhoBaiModel> TimKiemKho(int ti, int ts, string ten) {
      var list = _repo.TimKiemKho(ti, ts, out long tongSo, ten);
      return new PhanHoiPhanTrangModel<KhoBaiModel> {
        DuLieu           = list,
        TongSoBanGhi     = tongSo,
        TrangHienTai     = ti,
        SoBanGhiMoiTrang = ts
      };
    }

    // ─── Khu vực ─────────────────────────────────────────────────────────────

    /// <summary>Tạo mới khu vực trong kho.</summary>
    public bool TaoKhuVuc(ThemKhuVucRequestDto dto) {
      var m = new KhuVucKhoModel {
        zone_id      = Guid.NewGuid().ToString("N")[..20],
        warehouse_id = dto.MaKhoDb,
        zone_code    = dto.MaKhuVuc,
        zone_name    = dto.TenKhuVuc
      };
      return _repo.TaoKhuVuc(m);
    }

    /// <summary>Lấy danh sách khu vực theo kho.</summary>
    public List<KhuVucKhoModel> LayKhuVucTheoKho(string warehouseId) =>
      _repo.LayKhuVucTheoKho(warehouseId);

    // ─── Vị trí ──────────────────────────────────────────────────────────────

    /// <summary>Tạo mới vị trí (slot) trong khu vực.</summary>
    public bool TaoViTri(ThemViTriRequestDto dto) {
      var m = new ViTriKhoModel {
        slot_id  = Guid.NewGuid().ToString("N")[..20],
        zone_id  = dto.MaKhuVucDb,
        slot_code = dto.MaViTri,
        max_teu  = dto.TeuToiDa
      };
      return _repo.TaoViTri(m);
    }

    /// <summary>Lấy danh sách vị trí theo khu vực.</summary>
    public List<ViTriKhoModel> LayViTriTheoKhuVuc(string zoneId) =>
      _repo.LayViTriTheoKhuVuc(zoneId);

    /// <summary>Lấy danh sách vị trí còn trống trong kho.</summary>
    public List<ViTriKhoModel> LayViTriTrong(string warehouseId) =>
      _repo.LayViTriTrong(warehouseId);
  }
}
