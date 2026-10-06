using Model;

namespace DAL
{
    /// <summary>
    /// Interface định nghĩa các thao tác với Kho Bãi, Khu Vực và Vị Trí Kho.
    /// </summary>
    public interface IKhoBaiRepository
    {
        // ── Kho Bãi ──────────────────────────────────────────────────────────
        /// <summary>Tạo mới kho bãi.</summary>
        bool TaoKho(KhoBaiModel m);

        /// <summary>Cập nhật thông tin kho bãi.</summary>
        bool CapNhatKho(KhoBaiModel m);

        /// <summary>Xóa kho bãi theo ID.</summary>
        bool XoaKho(string id);

        /// <summary>Lấy thông tin kho bãi theo ID.</summary>
        KhoBaiModel? LayKhoTheoId(string id);

        /// <summary>Tìm kiếm kho bãi có phân trang.</summary>
        List<KhoBaiModel> TimKiemKho(int ti, int ts, out long tongSo, string ten);

        // ── Khu Vực Kho ───────────────────────────────────────────────────────
        /// <summary>Tạo mới khu vực trong kho.</summary>
        bool TaoKhuVuc(KhuVucKhoModel m);

        /// <summary>Lấy danh sách khu vực theo ID kho.</summary>
        List<KhuVucKhoModel> LayKhuVucTheoKho(string warehouseId);

        // ── Vị Trí Kho ────────────────────────────────────────────────────────
        /// <summary>Tạo mới vị trí (slot) trong khu vực.</summary>
        bool TaoViTri(ViTriKhoModel m);

        /// <summary>Lấy danh sách vị trí theo khu vực.</summary>
        List<ViTriKhoModel> LayViTriTheoKhuVuc(string zoneId);

        /// <summary>Lấy danh sách vị trí trống trong kho.</summary>
        List<ViTriKhoModel> LayViTriTrong(string warehouseId);
    }
}
