using Model;

namespace DAL
{
    /// <summary>
    /// Interface định nghĩa các thao tác với Vận Chuyển: Phương Tiện,
    /// Tàu Biển, Chuyến Hàng và các Chặng Vận Chuyển.
    /// </summary>
    public interface IVanChuyenRepository
    {
        // ── Phương Tiện ───────────────────────────────────────────────────────
        /// <summary>Tạo mới phương tiện vận chuyển.</summary>
        bool TaoPhuongTien(PhuongTienModel m);

        /// <summary>Cập nhật thông tin phương tiện.</summary>
        bool CapNhatPhuongTien(PhuongTienModel m);

        /// <summary>Lấy danh sách phương tiện có phân trang theo biển số.</summary>
        List<PhuongTienModel> LayDanhSachPhuongTien(int ti, int ts, out long tongSo, string bienSo);

        // ── Tàu Biển ──────────────────────────────────────────────────────────
        /// <summary>Tạo mới tàu biển.</summary>
        bool TaoTauBien(TauBienModel m);

        /// <summary>Lấy danh sách tàu biển có phân trang.</summary>
        List<TauBienModel> LayDanhSachTauBien(int ti, int ts, out long tongSo);

        // ── Chuyến Hàng ───────────────────────────────────────────────────────
        /// <summary>Tạo mới chuyến hàng.</summary>
        bool TaoChuyenHang(ChuyenHangModel m);

        /// <summary>Cập nhật trạng thái chuyến hàng.</summary>
        bool CapNhatTrangThaiChuyenHang(string shipmentId, string trangThai);

        /// <summary>Xóa chuyến hàng theo ID.</summary>
        bool XoaChuyenHang(string id);

        /// <summary>Lấy chuyến hàng theo ID.</summary>
        ChuyenHangModel? LayChuyenHangTheoId(string id);

        /// <summary>Tìm kiếm chuyến hàng có phân trang theo mã vận đơn và trạng thái.</summary>
        List<ChuyenHangModel> TimKiemChuyenHang(int ti, int ts, out long tongSo, string maVanDon, string trangThai);

        // ── Chặng Vận Chuyển ──────────────────────────────────────────────────
        /// <summary>Thêm một chặng vận chuyển vào chuyến hàng.</summary>
        bool ThemChang(ChangVanChuyenModel m);

        /// <summary>Lấy danh sách chặng theo chuyến hàng.</summary>
        List<ChangVanChuyenModel> LayChangTheoChuyenHang(string shipmentId);
    }
}
