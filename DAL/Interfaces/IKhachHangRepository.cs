using Model;

namespace DAL
{
    /// <summary>
    /// Interface định nghĩa các thao tác với bảng KhachHang (Khách Hàng).
    /// </summary>
    public interface IKhachHangRepository
    {
        /// <summary>Tạo mới một khách hàng.</summary>
        bool TaoMoi(KhachHangModel model);

        /// <summary>Cập nhật thông tin khách hàng.</summary>
        bool CapNhat(KhachHangModel model);

        /// <summary>Xóa mềm khách hàng theo ID.</summary>
        bool XoaMem(string id);

        /// <summary>Lấy khách hàng theo ID.</summary>
        KhachHangModel? LayTheoId(string id);

        /// <summary>Tìm kiếm khách hàng có phân trang.</summary>
        List<KhachHangModel> TimKiem(int trangIndex, int trangSize, out long tongSo, string ten);
    }
}
