using DAL;
using Model;

namespace BLL {
  /// <summary>
  /// Business logic cho HoaDon: hợp đồng, hóa đơn, thanh toán và tính phí lưu bãi.
  /// </summary>
  public partial class HoaDonBusiness : IHoaDonBusiness {
    private readonly IHoaDonRepository _repo;

    public HoaDonBusiness(IHoaDonRepository repo) {
      _repo = repo;
    }

    // ─── Hợp đồng ────────────────────────────────────────────────────────────

    /// <summary>Tạo mới hợp đồng dịch vụ kho bãi.</summary>
    public PhanHoiModel TaoHopDong(TaoHopDongRequestDto dto) {
      try {
        var m = new HopDongModel {
          contract_id     = Guid.NewGuid().ToString("N")[..20],
          so_hop_dong     = dto.SoHopDong,
          customer_id     = dto.MaKhachHangDb,
          warehouse_id    = dto.MaKhoDb,
          ngay_bat_dau    = dto.NgayBatDau,
          ngay_ket_thuc   = dto.NgayKetThuc,
          don_gia_luu_bai = dto.DonGiaLuuBai,
          trang_thai      = "Active"
        };
        _repo.TaoHopDong(m);
        return PhanHoiModel.OK(new { MaHopDong = m.contract_id }, "Tạo hợp đồng thành công");
      } catch (Exception ex) {
        return PhanHoiModel.Loi(ex.Message);
      }
    }

    /// <summary>Xóa hợp đồng theo ID.</summary>
    public bool XoaHopDong(string id) => _repo.XoaHopDong(id);

    /// <summary>Lấy hợp đồng theo ID.</summary>
    public HopDongModel? LayHopDongTheoId(string id) => _repo.LayHopDongTheoId(id);

    /// <summary>Tìm kiếm hợp đồng phân trang theo mã khách hàng.</summary>
    public PhanHoiPhanTrangModel<HopDongModel> TimKiemHopDong(int ti, int ts, string maKhachHang) {
      var list = _repo.TimKiemHopDong(ti, ts, out long tongSo, maKhachHang);
      return new PhanHoiPhanTrangModel<HopDongModel> {
        DuLieu           = list,
        TongSoBanGhi     = tongSo,
        TrangHienTai     = ti,
        SoBanGhiMoiTrang = ts
      };
    }

    // ─── Hóa đơn ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Tạo mới hóa đơn, sinh số hóa đơn tự động theo định dạng INV-{năm}-{6 ký tự}.
    /// </summary>
    public PhanHoiModel TaoHoaDon(TaoHoaDonRequestDto dto) {
      try {
        var m = new HoaDonModel {
          invoice_id    = Guid.NewGuid().ToString("N")[..20],
          so_hoa_don    = "INV-" + DateTime.Now.Year + "-" + Guid.NewGuid().ToString("N")[..6].ToUpper(),
          contract_id   = dto.MaHopDongDb,
          customer_id   = dto.MaKhachHangDb,
          ngay_lap      = DateTime.Today,
          han_thanh_toan = DateTime.Today.AddDays(dto.SoNgayHanThanhToan),
          so_tien       = dto.SoTien,
          trang_thai    = "Unpaid"
        };
        _repo.TaoHoaDon(m);
        return PhanHoiModel.OK(
          new { MaHoaDon = m.invoice_id, SoHoaDon = m.so_hoa_don },
          "Tạo hóa đơn thành công");
      } catch (Exception ex) {
        return PhanHoiModel.Loi(ex.Message);
      }
    }

    /// <summary>Xóa hóa đơn theo ID.</summary>
    public bool XoaHoaDon(string id) => _repo.XoaHoaDon(id);

    /// <summary>Lấy hóa đơn theo ID.</summary>
    public HoaDonModel? LayHoaDonTheoId(string id) => _repo.LayHoaDonTheoId(id);

    /// <summary>Tìm kiếm hóa đơn phân trang theo mã khách hàng và trạng thái.</summary>
    public PhanHoiPhanTrangModel<HoaDonModel> TimKiemHoaDon(int ti, int ts, string maKhachHang, string trangThai) {
      var list = _repo.TimKiemHoaDon(ti, ts, out long tongSo, maKhachHang, trangThai);
      return new PhanHoiPhanTrangModel<HoaDonModel> {
        DuLieu           = list,
        TongSoBanGhi     = tongSo,
        TrangHienTai     = ti,
        SoBanGhiMoiTrang = ts
      };
    }

    // ─── Thanh toán ──────────────────────────────────────────────────────────

    /// <summary>Ghi nhận thanh toán cho hóa đơn. Kiểm tra hóa đơn tồn tại trước khi ghi.</summary>
    public PhanHoiModel GhiNhanThanhToan(GhiNhanThanhToanRequestDto dto) {
      try {
        var hd = _repo.LayHoaDonTheoId(dto.MaHoaDonDb);
        if (hd == null) return PhanHoiModel.Loi("Không tìm thấy hóa đơn");

        var m = new ThanhToanModel {
          payment_id     = Guid.NewGuid().ToString("N")[..20],
          invoice_id     = dto.MaHoaDonDb,
          ngay_thanh_toan = DateTime.Now,
          so_tien        = dto.SoTien,
          phuong_thuc    = dto.PhuongThuc,
          ghi_chu        = dto.GhiChu
        };
        _repo.GhiNhanThanhToan(m);
        return PhanHoiModel.OK(null, "Ghi nhận thanh toán thành công");
      } catch (Exception ex) {
        return PhanHoiModel.Loi(ex.Message);
      }
    }

    /// <summary>Lấy danh sách các lần thanh toán theo hóa đơn.</summary>
    public List<ThanhToanModel> LayThanhToanTheoHoaDon(string invoiceId) =>
      _repo.LayThanhToanTheoHoaDon(invoiceId);

    // ─── Tính phí ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Tính phí lưu bãi dựa trên số ngày quá hạn miễn phí và đơn giá trong hợp đồng.
    /// Trả về 0 nếu còn trong thời gian miễn phí.
    /// </summary>
    public decimal TinhPhiLuuBai(ContainerModel container, HopDongModel hopDong) {
      // Nếu chưa hết miễn phí thì không tính tiền
      if (container.mien_phi_den_ngay == null || DateTime.Now <= container.mien_phi_den_ngay)
        return 0;

      var soNgayTinhTien = (DateTime.Now - container.mien_phi_den_ngay.Value).Days;
      return soNgayTinhTien * hopDong.don_gia_luu_bai;
    }
  }
}
