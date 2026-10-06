using DAL.Helper;
using Model;

namespace DAL
{
    /// <summary>
    /// Repository xử lý nghiệp vụ dữ liệu Người Dùng thông qua Stored Procedure.
    /// </summary>
    public partial class NguoiDungRepository : INguoiDungRepository
    {
        private readonly IDatabaseHelper _db;

        public NguoiDungRepository(IDatabaseHelper db)
        {
            _db = db;
        }

        /// <summary>
        /// Tạo mới người dùng. Gọi SP: sp_user_create.
        /// </summary>
        public bool TaomMoi(NguoiDungModel model)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_user_create",
                "@user_id",   model.user_id,
                "@hoten",     model.hoten,
                "@ngaysinh",  model.ngaysinh,
                "@diachi",    model.diachi,
                "@gioitinh",  model.gioitinh,
                "@email",     model.email,
                "@taikhoan",  model.taikhoan,
                "@matkhau",   model.matkhau,
                "@role",      model.role,
                "@image_url", model.image_url);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Cập nhật thông tin người dùng. Gọi SP: sp_user_update.
        /// </summary>
        public bool CapNhat(NguoiDungModel model)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_user_update",
                "@user_id",   model.user_id,
                "@hoten",     model.hoten,
                "@ngaysinh",  model.ngaysinh,
                "@diachi",    model.diachi,
                "@gioitinh",  model.gioitinh,
                "@email",     model.email,
                "@taikhoan",  model.taikhoan,
                "@matkhau",   model.matkhau,
                "@role",      model.role,
                "@image_url", model.image_url);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Xóa mềm người dùng theo ID. Gọi SP: sp_user_delete.
        /// </summary>
        public bool XoaMem(string id)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_user_delete",
                "@user_id", id);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Lấy thông tin người dùng theo ID. Gọi SP: sp_user_get_by_id.
        /// </summary>
        public NguoiDungModel? LayTheoId(string id)
        {
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_user_get_by_id",
                "@user_id", id);

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            return dt.ConvertTo<NguoiDungModel>()?.FirstOrDefault();
        }

        /// <summary>
        /// Lấy người dùng theo tài khoản và mật khẩu (dùng cho đăng nhập).
        /// Gọi SP: sp_user_get_by_username_password.
        /// </summary>
        public NguoiDungModel? LayTheoTaiKhoanMatKhau(string taikhoan, string matkhau)
        {
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_user_get_by_username_password",
                "@taikhoan", taikhoan,
                "@matkhau",  matkhau);

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            return dt.ConvertTo<NguoiDungModel>()?.FirstOrDefault();
        }

        /// <summary>
        /// Tìm kiếm người dùng có phân trang. Gọi SP: sp_user_search.
        /// Trả về tổng số bản ghi qua tham số out tongSoBanGhi.
        /// </summary>
        public List<NguoiDungModel> TimKiem(int trangIndex, int trangSize, out long tongSoBanGhi, string hoTen, string taiKhoan)
        {
            tongSoBanGhi = 0;
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_user_search",
                "@page_index", trangIndex,
                "@page_size",  trangSize,
                "@hoten",      hoTen    ?? "",
                "@taikhoan",   taiKhoan ?? "");

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            if (dt.Rows.Count > 0)
                tongSoBanGhi = (long)dt.Rows[0]["RecordCount"];

            return dt.ConvertTo<NguoiDungModel>()?.ToList() ?? new();
        }
    }
}
