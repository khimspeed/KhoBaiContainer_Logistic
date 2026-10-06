using DAL;
using Model;

namespace BLL {
  /// <summary>
  /// Business logic cho Container: CRUD, nhập bãi, xuất bãi, chuyển vị trí, lịch sử di chuyển, danh mục.
  /// </summary>
  public partial class ContainerBusiness : IContainerBusiness {
    private readonly IContainerRepository _repo;

    public ContainerBusiness(IContainerRepository repo) {
      _repo = repo;
    }

    /// <summary>Tạo mới container (sinh ID tự động).</summary>
    public bool TaoMoi(ContainerModel m) {
      m.container_id = Guid.NewGuid().ToString("N")[..20];
      return _repo.TaoMoi(m);
    }

    /// <summary>Cập nhật thông tin container.</summary>
    public bool CapNhat(ContainerModel m) => _repo.CapNhat(m);

    /// <summary>Xóa mềm container theo ID.</summary>
    public bool XoaMem(string id) => _repo.XoaMem(id);

    /// <summary>Lấy container theo ID.</summary>
    public ContainerModel? LayTheoId(string id) => _repo.LayTheoId(id);

    /// <summary>Tìm kiếm container phân trang theo số container, trạng thái, mã kho.</summary>
    public PhanHoiPhanTrangModel<ContainerModel> TimKiem(int ti, int ts, string soContainer, string trangThai, string maKho) {
      var list = _repo.TimKiem(ti, ts, out long tongSo, soContainer, trangThai, maKho);
      return new PhanHoiPhanTrangModel<ContainerModel> {
        DuLieu           = list,
        TongSoBanGhi     = tongSo,
        TrangHienTai     = ti,
        SoBanGhiMoiTrang = ts
      };
    }

    /// <summary>
    /// Nhập bãi: tạo container mới rồi gọi SP nhập bãi để xếp vào vị trí được chọn.
    /// </summary>
    public PhanHoiModel NhapBai(NhapBaiRequestDto dto, string nguoiThucHien) {
      try {
        string containerId = Guid.NewGuid().ToString("N")[..20];

        // Tạo bản ghi container trước
        var c = new ContainerModel {
          container_id   = containerId,
          container_no   = dto.SoContainer,
          type_id        = dto.MaLoaiContainerDb,
          cargo_type_id  = dto.MaLoaiHangDb,
          customer_id    = dto.MaKhachHangDb
        };
        _repo.TaoMoi(c);

        // Thực hiện nhập bãi (cập nhật vị trí + ghi lịch sử di chuyển)
        var freeUntil = DateTime.Now.AddDays(dto.SoNgayMienPhi);
        _repo.NhapBai(containerId, dto.MaKhoDb, dto.MaViTriDb, freeUntil, nguoiThucHien, dto.GhiChu);

        return PhanHoiModel.OK(new { MaContainer = containerId }, "Nhập bãi thành công");
      } catch (Exception ex) {
        return PhanHoiModel.Loi(ex.Message);
      }
    }

    /// <summary>Xuất bãi container (cập nhật trạng thái + ghi lịch sử).</summary>
    public PhanHoiModel XuatBai(XuatBaiRequestDto dto, string nguoiThucHien) {
      try {
        _repo.XuatBai(dto.MaContainerDb, nguoiThucHien, dto.GhiChu);
        return PhanHoiModel.OK(null, "Xuất bãi thành công");
      } catch (Exception ex) {
        return PhanHoiModel.Loi(ex.Message);
      }
    }

    /// <summary>Chuyển vị trí container sang vị trí mới trong bãi.</summary>
    public PhanHoiModel ChuyenViTri(ChuyenViTriRequestDto dto, string nguoiThucHien) {
      try {
        _repo.ChuyenViTri(dto.MaContainerDb, dto.MaViTriMoiDb, nguoiThucHien, dto.GhiChu);
        return PhanHoiModel.OK(null, "Chuyển vị trí thành công");
      } catch (Exception ex) {
        return PhanHoiModel.Loi(ex.Message);
      }
    }

    /// <summary>Lấy lịch sử di chuyển của container có phân trang.</summary>
    public PhanHoiPhanTrangModel<LichSuDiChuyenModel> LayLichSuDiChuyen(string containerId, int ti, int ts) {
      var list = _repo.LayLichSuDiChuyen(containerId, ti, ts, out long tongSo);
      return new PhanHoiPhanTrangModel<LichSuDiChuyenModel> {
        DuLieu           = list,
        TongSoBanGhi     = tongSo,
        TrangHienTai     = ti,
        SoBanGhiMoiTrang = ts
      };
    }

    /// <summary>Lấy danh sách container đã quá hạn miễn phí lưu bãi.</summary>
    public List<ContainerModel> LayContainerQuaHan() => _repo.LayContainerQuaHan();

    /// <summary>Lấy danh sách loại container (danh mục).</summary>
    public List<LoaiContainerModel> LayDanhSachLoaiContainer() => _repo.LayDanhSachLoaiContainer();

    /// <summary>Lấy danh sách loại hàng hóa (danh mục).</summary>
    public List<LoaiHangHoaModel> LayDanhSachLoaiHang() => _repo.LayDanhSachLoaiHang();
  }
}
