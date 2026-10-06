using DAL.Helper;
using Model;

namespace DAL
{
    /// <summary>
    /// Repository xử lý nghiệp vụ dữ liệu Hợp Đồng, Hóa Đơn và Thanh Toán
    /// thông qua Stored Procedure.
    /// </summary>
    public partial class HoaDonRepository : IHoaDonRepository
    {
        private readonly IDatabaseHelper _db;

        public HoaDonRepository(IDatabaseHelper db)
        {
            _db = db;
        }

        // ══════════════════════════════════════════════════════════════════════
        // HỢP ĐỒNG
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Tạo mới hợp đồng lưu kho. Gọi SP: sp_hopdong_create.
        /// </summary>
        public bool TaoHopDong(HopDongModel m)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_hopdong_create",
                "@contract_id",    m.contract_id,
                "@contract_no",    m.so_hop_dong,
                "@customer_id",    m.customer_id,
                "@warehouse_id",   m.warehouse_id,
                "@start_date",     m.ngay_bat_dau,
                "@end_date",       m.ngay_ket_thuc,
                "@storage_rate",   m.don_gia_luu_bai);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Xóa hợp đồng theo ID. Gọi SP: sp_hopdong_delete.
        /// </summary>
        public bool XoaHopDong(string id)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_hopdong_delete",
                "@contract_id", id);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Lấy thông tin hợp đồng theo ID. Gọi SP: sp_hopdong_get_by_id.
        /// </summary>
        public HopDongModel? LayHopDongTheoId(string id)
        {
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_hopdong_get_by_id",
                "@contract_id", id);

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            return dt.ConvertTo<HopDongModel>()?.FirstOrDefault();
        }

        /// <summary>
        /// Tìm kiếm hợp đồng có phân trang theo mã khách hàng.
        /// Gọi SP: sp_hopdong_search.
        /// </summary>
        public List<HopDongModel> TimKiemHopDong(int ti, int ts, out long tongSo, string maKhachHang)
        {
            tongSo = 0;
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_hopdong_search",
                "@page_index",   ti,
                "@page_size",    ts,
                "@customer_id",  maKhachHang ?? "");

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            if (dt.Rows.Count > 0)
                tongSo = (long)dt.Rows[0]["RecordCount"];

            return dt.ConvertTo<HopDongModel>()?.ToList() ?? new();
        }

        // ══════════════════════════════════════════════════════════════════════
        // HÓA ĐƠN
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Tạo mới hóa đơn. Gọi SP: sp_hoadon_create.
        /// </summary>
        public bool TaoHoaDon(HoaDonModel m)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_hoadon_create",
                "@invoice_id",   m.invoice_id,
                "@invoice_no",   m.so_hoa_don,
                "@contract_id",  m.contract_id,
                "@customer_id",  m.customer_id,
                "@invoice_date", m.ngay_lap,
                "@due_date",     m.han_thanh_toan,
                "@amount",       m.so_tien);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Xóa hóa đơn theo ID. Gọi SP: sp_hoadon_delete.
        /// </summary>
        public bool XoaHoaDon(string id)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_hoadon_delete",
                "@invoice_id", id);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Lấy thông tin hóa đơn theo ID. Gọi SP: sp_hoadon_get_by_id.
        /// </summary>
        public HoaDonModel? LayHoaDonTheoId(string id)
        {
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_hoadon_get_by_id",
                "@invoice_id", id);

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            return dt.ConvertTo<HoaDonModel>()?.FirstOrDefault();
        }

        /// <summary>
        /// Tìm kiếm hóa đơn có phân trang theo mã khách hàng và trạng thái.
        /// Gọi SP: sp_hoadon_search.
        /// </summary>
        public List<HoaDonModel> TimKiemHoaDon(int ti, int ts, out long tongSo,
            string maKhachHang, string trangThai)
        {
            tongSo = 0;
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_hoadon_search",
                "@page_index",   ti,
                "@page_size",    ts,
                "@customer_id",  maKhachHang ?? "",
                "@status",       trangThai   ?? "");

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            if (dt.Rows.Count > 0)
                tongSo = (long)dt.Rows[0]["RecordCount"];

            return dt.ConvertTo<HoaDonModel>()?.ToList() ?? new();
        }

        // ══════════════════════════════════════════════════════════════════════
        // THANH TOÁN
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Ghi nhận một lần thanh toán cho hóa đơn. Gọi SP: sp_thanhtoan_create.
        /// </summary>
        public bool GhiNhanThanhToan(ThanhToanModel m)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_thanhtoan_create",
                "@payment_id", m.payment_id,
                "@invoice_id", m.invoice_id,
                "@amount",     m.so_tien,
                "@method",     m.phuong_thuc,
                "@note",       m.ghi_chu);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Lấy danh sách thanh toán theo hóa đơn. Gọi SP: sp_thanhtoan_get_by_hoadon.
        /// </summary>
        public List<ThanhToanModel> LayThanhToanTheoHoaDon(string invoiceId)
        {
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_thanhtoan_get_by_hoadon",
                "@invoice_id", invoiceId);

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            return dt.ConvertTo<ThanhToanModel>()?.ToList() ?? new();
        }
    }
}
