using DAL.Helper;
using Model;

namespace DAL
{
    /// <summary>
    /// Repository xử lý nghiệp vụ dữ liệu Khách Hàng thông qua Stored Procedure.
    /// </summary>
    public partial class KhachHangRepository : IKhachHangRepository
    {
        private readonly IDatabaseHelper _db;

        public KhachHangRepository(IDatabaseHelper db)
        {
            _db = db;
        }

        /// <summary>
        /// Tạo mới khách hàng. Gọi SP: sp_khachhang_create.
        /// </summary>
        public bool TaoMoi(KhachHangModel m)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_khachhang_create",
                "@customer_id",   m.customer_id,
                "@customer_code", m.customer_code,
                "@customer_name", m.customer_name,
                "@tax_code",      m.tax_code,
                "@phone",         m.phone,
                "@email",         m.email,
                "@address",       m.address);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Cập nhật thông tin khách hàng. Gọi SP: sp_khachhang_update.
        /// </summary>
        public bool CapNhat(KhachHangModel m)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_khachhang_update",
                "@customer_id",   m.customer_id,
                "@customer_code", m.customer_code,
                "@customer_name", m.customer_name,
                "@tax_code",      m.tax_code,
                "@phone",         m.phone,
                "@email",         m.email,
                "@address",       m.address);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Xóa mềm khách hàng theo ID. Gọi SP: sp_khachhang_delete.
        /// </summary>
        public bool XoaMem(string id)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_khachhang_delete",
                "@customer_id", id);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Lấy thông tin khách hàng theo ID. Gọi SP: sp_khachhang_get_by_id.
        /// </summary>
        public KhachHangModel? LayTheoId(string id)
        {
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_khachhang_get_by_id",
                "@customer_id", id);

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            return dt.ConvertTo<KhachHangModel>()?.FirstOrDefault();
        }

        /// <summary>
        /// Tìm kiếm khách hàng có phân trang theo tên. Gọi SP: sp_khachhang_search.
        /// Trả về tổng số bản ghi qua tham số out tongSo.
        /// </summary>
        public List<KhachHangModel> TimKiem(int trangIndex, int trangSize, out long tongSo, string ten)
        {
            tongSo = 0;
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_khachhang_search",
                "@page_index",    trangIndex,
                "@page_size",     trangSize,
                "@customer_name", ten ?? "");

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            if (dt.Rows.Count > 0)
                tongSo = (long)dt.Rows[0]["RecordCount"];

            return dt.ConvertTo<KhachHangModel>()?.ToList() ?? new();
        }
    }
}
