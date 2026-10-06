using DAL.Helper;
using Model;

namespace DAL
{
    /// <summary>
    /// Repository xử lý nghiệp vụ dữ liệu Kho Bãi, Khu Vực và Vị Trí Kho
    /// thông qua Stored Procedure.
    /// </summary>
    public partial class KhoBaiRepository : IKhoBaiRepository
    {
        private readonly IDatabaseHelper _db;

        public KhoBaiRepository(IDatabaseHelper db)
        {
            _db = db;
        }

        // ══════════════════════════════════════════════════════════════════════
        // KHO BÃI
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Tạo mới kho bãi. Gọi SP: sp_kho_create.
        /// </summary>
        public bool TaoKho(KhoBaiModel m)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_kho_create",
                "@warehouse_id",   m.warehouse_id,
                "@warehouse_code", m.warehouse_code,
                "@warehouse_name", m.warehouse_name,
                "@address",        m.address,
                "@capacity_teu",   m.capacity_teu);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Cập nhật thông tin kho bãi. Gọi SP: sp_kho_update.
        /// </summary>
        public bool CapNhatKho(KhoBaiModel m)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_kho_update",
                "@warehouse_id",   m.warehouse_id,
                "@warehouse_code", m.warehouse_code,
                "@warehouse_name", m.warehouse_name,
                "@address",        m.address,
                "@capacity_teu",   m.capacity_teu);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Xóa kho bãi theo ID. Gọi SP: sp_kho_delete.
        /// </summary>
        public bool XoaKho(string id)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_kho_delete",
                "@warehouse_id", id);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Lấy thông tin kho bãi theo ID. Gọi SP: sp_kho_get_by_id.
        /// </summary>
        public KhoBaiModel? LayKhoTheoId(string id)
        {
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_kho_get_by_id",
                "@warehouse_id", id);

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            return dt.ConvertTo<KhoBaiModel>()?.FirstOrDefault();
        }

        /// <summary>
        /// Tìm kiếm kho bãi có phân trang theo tên. Gọi SP: sp_kho_search.
        /// Trả về tổng số bản ghi qua tham số out tongSo.
        /// </summary>
        public List<KhoBaiModel> TimKiemKho(int ti, int ts, out long tongSo, string ten)
        {
            tongSo = 0;
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_kho_search",
                "@page_index",    ti,
                "@page_size",     ts,
                "@warehouse_name", ten ?? "");

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            if (dt.Rows.Count > 0)
                tongSo = (long)dt.Rows[0]["RecordCount"];

            return dt.ConvertTo<KhoBaiModel>()?.ToList() ?? new();
        }

        // ══════════════════════════════════════════════════════════════════════
        // KHU VỰC KHO
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Tạo mới khu vực trong kho. Gọi SP: sp_khuvuc_create.
        /// </summary>
        public bool TaoKhuVuc(KhuVucKhoModel m)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_khuvuc_create",
                "@zone_id",      m.zone_id,
                "@warehouse_id", m.warehouse_id,
                "@zone_code",    m.zone_code,
                "@zone_name",    m.zone_name);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Lấy danh sách khu vực theo ID kho. Gọi SP: sp_khuvuc_get_by_kho.
        /// </summary>
        public List<KhuVucKhoModel> LayKhuVucTheoKho(string warehouseId)
        {
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_khuvuc_get_by_kho",
                "@warehouse_id", warehouseId);

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            return dt.ConvertTo<KhuVucKhoModel>()?.ToList() ?? new();
        }

        // ══════════════════════════════════════════════════════════════════════
        // VỊ TRÍ KHO (SLOT)
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Tạo mới vị trí (slot) trong khu vực. Gọi SP: sp_vitri_create.
        /// </summary>
        public bool TaoViTri(ViTriKhoModel m)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_vitri_create",
                "@slot_id",  m.slot_id,
                "@zone_id",  m.zone_id,
                "@slot_code", m.slot_code,
                "@max_teu",  m.max_teu);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Lấy danh sách vị trí theo khu vực. Gọi SP: sp_vitri_get_by_khuvuc.
        /// </summary>
        public List<ViTriKhoModel> LayViTriTheoKhuVuc(string zoneId)
        {
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_vitri_get_by_khuvuc",
                "@zone_id", zoneId);

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            return dt.ConvertTo<ViTriKhoModel>()?.ToList() ?? new();
        }

        /// <summary>
        /// Lấy danh sách vị trí còn trống trong kho. Gọi SP: sp_vitri_trong_theo_kho.
        /// </summary>
        public List<ViTriKhoModel> LayViTriTrong(string warehouseId)
        {
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_vitri_trong_theo_kho",
                "@warehouse_id", warehouseId);

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            return dt.ConvertTo<ViTriKhoModel>()?.ToList() ?? new();
        }
    }
}
