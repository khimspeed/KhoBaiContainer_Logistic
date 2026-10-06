using DAL.Helper;
using Model;

namespace DAL
{
    /// <summary>
    /// Repository xử lý nghiệp vụ dữ liệu Vận Chuyển: Phương Tiện,
    /// Tàu Biển, Chuyến Hàng và Chặng Vận Chuyển thông qua Stored Procedure.
    /// </summary>
    public partial class VanChuyenRepository : IVanChuyenRepository
    {
        private readonly IDatabaseHelper _db;

        public VanChuyenRepository(IDatabaseHelper db)
        {
            _db = db;
        }

        // ══════════════════════════════════════════════════════════════════════
        // PHƯƠNG TIỆN
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Tạo mới phương tiện vận chuyển. Gọi SP: sp_phuongtien_create.
        /// </summary>
        public bool TaoPhuongTien(PhuongTienModel m)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_phuongtien_create",
                "@vehicle_id",   m.vehicle_id,
                "@plate_no",     m.bien_so,
                "@vehicle_type", m.loai_phuong_tien,
                "@driver_name",  m.ten_tai_xe,
                "@phone",        m.so_dien_thoai);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Cập nhật thông tin phương tiện. Gọi SP: sp_phuongtien_update.
        /// </summary>
        public bool CapNhatPhuongTien(PhuongTienModel m)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_phuongtien_update",
                "@vehicle_id",   m.vehicle_id,
                "@plate_no",     m.bien_so,
                "@vehicle_type", m.loai_phuong_tien,
                "@driver_name",  m.ten_tai_xe,
                "@phone",        m.so_dien_thoai,
                "@status",       m.trang_thai);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Lấy danh sách phương tiện có phân trang theo biển số.
        /// Gọi SP: sp_phuongtien_search.
        /// </summary>
        public List<PhuongTienModel> LayDanhSachPhuongTien(int ti, int ts, out long tongSo, string bienSo)
        {
            tongSo = 0;
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_phuongtien_search",
                "@page_index", ti,
                "@page_size",  ts,
                "@plate_no",   bienSo ?? "");

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            if (dt.Rows.Count > 0)
                tongSo = (long)dt.Rows[0]["RecordCount"];

            return dt.ConvertTo<PhuongTienModel>()?.ToList() ?? new();
        }

        // ══════════════════════════════════════════════════════════════════════
        // TÀU BIỂN
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Tạo mới tàu biển. Gọi SP: sp_taubien_create.
        /// </summary>
        public bool TaoTauBien(TauBienModel m)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_taubien_create",
                "@vessel_id",   m.vessel_id,
                "@vessel_code", m.vessel_code,
                "@vessel_name", m.vessel_name,
                "@imo_no",      m.imo_no);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Lấy danh sách tàu biển có phân trang. Gọi SP: sp_taubien_search.
        /// </summary>
        public List<TauBienModel> LayDanhSachTauBien(int ti, int ts, out long tongSo)
        {
            tongSo = 0;
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_taubien_search",
                "@page_index", ti,
                "@page_size",  ts);

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            if (dt.Rows.Count > 0)
                tongSo = (long)dt.Rows[0]["RecordCount"];

            return dt.ConvertTo<TauBienModel>()?.ToList() ?? new();
        }

        // ══════════════════════════════════════════════════════════════════════
        // CHUYẾN HÀNG
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Tạo mới chuyến hàng. Gọi SP: sp_chuyenhang_create.
        /// </summary>
        public bool TaoChuyenHang(ChuyenHangModel m)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_chuyenhang_create",
                "@shipment_id",  m.shipment_id,
                "@tracking_no",  m.ma_van_don,
                "@customer_id",  m.customer_id,
                "@origin",       m.diem_di,
                "@destination",  m.diem_den,
                "@eta",          m.du_kien_den);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Cập nhật trạng thái chuyến hàng. Gọi SP: sp_chuyenhang_update_trangthai.
        /// </summary>
        public bool CapNhatTrangThaiChuyenHang(string shipmentId, string trangThai)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_chuyenhang_update_trangthai",
                "@shipment_id", shipmentId,
                "@status",      trangThai);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Xóa chuyến hàng theo ID. Gọi SP: sp_chuyenhang_delete.
        /// </summary>
        public bool XoaChuyenHang(string id)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_chuyenhang_delete",
                "@shipment_id", id);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Lấy thông tin chuyến hàng theo ID. Gọi SP: sp_chuyenhang_get_by_id.
        /// </summary>
        public ChuyenHangModel? LayChuyenHangTheoId(string id)
        {
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_chuyenhang_get_by_id",
                "@shipment_id", id);

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            return dt.ConvertTo<ChuyenHangModel>()?.FirstOrDefault();
        }

        /// <summary>
        /// Tìm kiếm chuyến hàng có phân trang theo mã vận đơn và trạng thái.
        /// Gọi SP: sp_chuyenhang_search.
        /// </summary>
        public List<ChuyenHangModel> TimKiemChuyenHang(int ti, int ts, out long tongSo,
            string maVanDon, string trangThai)
        {
            tongSo = 0;
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_chuyenhang_search",
                "@page_index",   ti,
                "@page_size",    ts,
                "@tracking_no",  maVanDon  ?? "",
                "@status",       trangThai ?? "");

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            if (dt.Rows.Count > 0)
                tongSo = (long)dt.Rows[0]["RecordCount"];

            return dt.ConvertTo<ChuyenHangModel>()?.ToList() ?? new();
        }

        // ══════════════════════════════════════════════════════════════════════
        // CHẶNG VẬN CHUYỂN
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Thêm một chặng vận chuyển vào chuyến hàng. Gọi SP: sp_changvanchuyen_create.
        /// </summary>
        public bool ThemChang(ChangVanChuyenModel m)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_changvanchuyen_create",
                "@leg_id",       m.leg_id,
                "@shipment_id",  m.shipment_id,
                "@container_id", m.container_id,
                "@vehicle_id",   m.vehicle_id,
                "@vessel_id",    m.vessel_id,
                "@sequence_no",  m.thu_tu,
                "@mode",         m.hinh_thuc);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Lấy danh sách chặng theo chuyến hàng. Gọi SP: sp_changvanchuyen_get_by_shipment.
        /// </summary>
        public List<ChangVanChuyenModel> LayChangTheoChuyenHang(string shipmentId)
        {
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_changvanchuyen_get_by_shipment",
                "@shipment_id", shipmentId);

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            return dt.ConvertTo<ChangVanChuyenModel>()?.ToList() ?? new();
        }
    }
}
