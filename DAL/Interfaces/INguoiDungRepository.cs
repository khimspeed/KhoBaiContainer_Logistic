using Model;

namespace DAL
{
    /// <summary>
    /// Interface định nghĩa các thao tác với bảng NguoiDung (Người Dùng).
    /// </summary>
    public interface INguoiDungRepository
    {
        /// <summary>Tạo mới một người dùng.</summary>
        bool TaomMoi(NguoiDungModel model);

        /// <summary>Cập nhật thông tin người dùng.</summary>
        bool CapNhat(NguoiDungModel model);

        /// <summary>Xóa mềm người dùng theo ID.</summary>
        bool XoaMem(string id);

        /// <summary>Lấy người dùng theo ID.</summary>
        NguoiDungModel? LayTheoId(string id);

        /// <summary>Lấy người dùng theo tài khoản và mật khẩu (đăng nhập).</summary>
        NguoiDungModel? LayTheoTaiKhoanMatKhau(string taikhoan, string matkhau);

        /// <summary>Tìm kiếm người dùng có phân trang.</summary>
        List<NguoiDungModel> TimKiem(int trangIndex, int trangSize, out long tongSoBanGhi, string hoTen, string taiKhoan);
    }
}
