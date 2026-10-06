using Model;

namespace DAL
{
    /// <summary>
    /// Interface định nghĩa các thao tác với Hợp Đồng, Hóa Đơn và Thanh Toán.
    /// </summary>
    public interface IHoaDonRepository
    {
        // ── Hợp Đồng ──────────────────────────────────────────────────────────
        /// <summary>Tạo mới hợp đồng lưu kho.</summary>
        bool TaoHopDong(HopDongModel m);

        /// <summary>Xóa hợp đồng theo ID.</summary>
        bool XoaHopDong(string id);

        /// <summary>Lấy hợp đồng theo ID.</summary>
        HopDongModel? LayHopDongTheoId(string id);

        /// <summary>Tìm kiếm hợp đồng có phân trang theo mã khách hàng.</summary>
        List<HopDongModel> TimKiemHopDong(int ti, int ts, out long tongSo, string maKhachHang);

        // ── Hóa Đơn ───────────────────────────────────────────────────────────
        /// <summary>Tạo mới hóa đơn.</summary>
        bool TaoHoaDon(HoaDonModel m);

        /// <summary>Xóa hóa đơn theo ID.</summary>
        bool XoaHoaDon(string id);

        /// <summary>Lấy hóa đơn theo ID.</summary>
        HoaDonModel? LayHoaDonTheoId(string id);

        /// <summary>Tìm kiếm hóa đơn có phân trang theo mã khách hàng và trạng thái.</summary>
        List<HoaDonModel> TimKiemHoaDon(int ti, int ts, out long tongSo, string maKhachHang, string trangThai);

        // ── Thanh Toán ────────────────────────────────────────────────────────
        /// <summary>Ghi nhận một lần thanh toán cho hóa đơn.</summary>
        bool GhiNhanThanhToan(ThanhToanModel m);

        /// <summary>Lấy danh sách thanh toán theo hóa đơn.</summary>
        List<ThanhToanModel> LayThanhToanTheoHoaDon(string invoiceId);
    }
}
