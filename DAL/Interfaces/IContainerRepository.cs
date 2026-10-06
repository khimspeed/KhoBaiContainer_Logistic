using Model;

namespace DAL
{
    /// <summary>
    /// Interface định nghĩa các thao tác với Container, bao gồm nhập/xuất bãi,
    /// chuyển vị trí, lịch sử di chuyển và danh mục tra cứu.
    /// </summary>
    public interface IContainerRepository
    {
        /// <summary>Tạo mới container.</summary>
        bool TaoMoi(ContainerModel m);

        /// <summary>Cập nhật thông tin container.</summary>
        bool CapNhat(ContainerModel m);

        /// <summary>Xóa mềm container theo ID.</summary>
        bool XoaMem(string id);

        /// <summary>Lấy thông tin container theo ID.</summary>
        ContainerModel? LayTheoId(string id);

        /// <summary>Tìm kiếm container có phân trang theo nhiều tiêu chí.</summary>
        List<ContainerModel> TimKiem(int ti, int ts, out long tongSo, string soContainer, string trangThai, string maKho);

        /// <summary>Nhập container vào bãi, gán vào vị trí cụ thể.</summary>
        bool NhapBai(string containerId, string warehouseId, string slotId, DateTime freeUntil, string createdBy, string? ghiChu);

        /// <summary>Xuất container ra khỏi bãi.</summary>
        bool XuatBai(string containerId, string createdBy, string? ghiChu);

        /// <summary>Chuyển container sang vị trí mới trong bãi.</summary>
        bool ChuyenViTri(string containerId, string slotMoiId, string createdBy, string? ghiChu);

        /// <summary>Lấy lịch sử di chuyển của container có phân trang.</summary>
        List<LichSuDiChuyenModel> LayLichSuDiChuyen(string containerId, int ti, int ts, out long tongSo);

        /// <summary>Lấy danh sách container đã quá hạn lưu bãi.</summary>
        List<ContainerModel> LayContainerQuaHan();

        /// <summary>Lấy danh sách loại container (danh mục).</summary>
        List<LoaiContainerModel> LayDanhSachLoaiContainer();

        /// <summary>Lấy danh sách loại hàng hóa (danh mục).</summary>
        List<LoaiHangHoaModel> LayDanhSachLoaiHang();
    }
}
