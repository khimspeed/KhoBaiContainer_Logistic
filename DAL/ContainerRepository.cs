using DAL.Helper;
using Model;

namespace DAL
{
    /// <summary>
    /// Repository xử lý nghiệp vụ dữ liệu Container: CRUD, nhập/xuất bãi,
    /// chuyển vị trí, lịch sử di chuyển, danh mục loại container và loại hàng.
    /// </summary>
    public partial class ContainerRepository : IContainerRepository
    {
        private readonly IDatabaseHelper _db;

        public ContainerRepository(IDatabaseHelper db)
        {
            _db = db;
        }

        // ══════════════════════════════════════════════════════════════════════
        // CRUD CƠ BẢN
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Tạo mới container. Gọi SP: sp_container_create.
        /// </summary>
        public bool TaoMoi(ContainerModel m)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_container_create",
                "@container_id",   m.container_id,
                "@container_no",   m.container_no,
                "@type_id",        m.type_id,
                "@cargo_type_id",  m.cargo_type_id,
                "@customer_id",    m.customer_id);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Cập nhật thông tin container. Gọi SP: sp_container_update.
        /// </summary>
        public bool CapNhat(ContainerModel m)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_container_update",
                "@container_id",   m.container_id,
                "@container_no",   m.container_no,
                "@type_id",        m.type_id,
                "@cargo_type_id",  m.cargo_type_id,
                "@customer_id",    m.customer_id);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Xóa mềm container theo ID. Gọi SP: sp_container_delete.
        /// </summary>
        public bool XoaMem(string id)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_container_delete",
                "@container_id", id);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Lấy thông tin container theo ID. Gọi SP: sp_container_get_by_id.
        /// </summary>
        public ContainerModel? LayTheoId(string id)
        {
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_container_get_by_id",
                "@container_id", id);

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            return dt.ConvertTo<ContainerModel>()?.FirstOrDefault();
        }

        /// <summary>
        /// Tìm kiếm container có phân trang theo số container, trạng thái và mã kho.
        /// Gọi SP: sp_container_search.
        /// </summary>
        public List<ContainerModel> TimKiem(int ti, int ts, out long tongSo,
            string soContainer, string trangThai, string maKho)
        {
            tongSo = 0;
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_container_search",
                "@page_index",    ti,
                "@page_size",     ts,
                "@container_no",  soContainer ?? "",
                "@trang_thai",    trangThai   ?? "",
                "@warehouse_id",  maKho       ?? "");

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            if (dt.Rows.Count > 0)
                tongSo = (long)dt.Rows[0]["RecordCount"];

            return dt.ConvertTo<ContainerModel>()?.ToList() ?? new();
        }

        // ══════════════════════════════════════════════════════════════════════
        // NGHIỆP VỤ BÃI
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Nhập container vào bãi, gán vào một vị trí cụ thể.
        /// Gọi SP: sp_container_nhapdong.
        /// </summary>
        public bool NhapBai(string containerId, string warehouseId, string slotId,
            DateTime freeUntil, string createdBy, string? ghiChu)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_container_nhapdong",
                "@container_id", containerId,
                "@warehouse_id", warehouseId,
                "@slot_id",      slotId,
                "@free_until",   freeUntil,
                "@created_by",   createdBy,
                "@note",         ghiChu);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Xuất container ra khỏi bãi. Gọi SP: sp_container_xuatbai.
        /// </summary>
        public bool XuatBai(string containerId, string createdBy, string? ghiChu)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_container_xuatbai",
                "@container_id", containerId,
                "@created_by",   createdBy,
                "@note",         ghiChu);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Chuyển container sang vị trí mới trong bãi. Gọi SP: sp_container_chuyenvitri.
        /// </summary>
        public bool ChuyenViTri(string containerId, string slotMoiId, string createdBy, string? ghiChu)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_container_chuyenvitri",
                "@container_id", containerId,
                "@slot_moi_id",  slotMoiId,
                "@created_by",   createdBy,
                "@note",         ghiChu);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        // ══════════════════════════════════════════════════════════════════════
        // LỊCH SỬ & BÁO CÁO
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Lấy lịch sử di chuyển của container có phân trang.
        /// Gọi SP: sp_lichsudichuyenpaging.
        /// </summary>
        public List<LichSuDiChuyenModel> LayLichSuDiChuyen(string containerId, int ti, int ts, out long tongSo)
        {
            tongSo = 0;
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_lichsudichuyenpaging",
                "@container_id", containerId,
                "@page_index",   ti,
                "@page_size",    ts);

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            if (dt.Rows.Count > 0)
                tongSo = (long)dt.Rows[0]["RecordCount"];

            return dt.ConvertTo<LichSuDiChuyenModel>()?.ToList() ?? new();
        }

        /// <summary>
        /// Lấy danh sách container đã quá hạn lưu bãi. Gọi SP: sp_container_quahan.
        /// </summary>
        public List<ContainerModel> LayContainerQuaHan()
        {
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_container_quahan");

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            return dt.ConvertTo<ContainerModel>()?.ToList() ?? new();
        }

        // ══════════════════════════════════════════════════════════════════════
        // DANH MỤC
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Lấy danh sách loại container. Gọi SP: sp_loaicontainer_getall.
        /// </summary>
        public List<LoaiContainerModel> LayDanhSachLoaiContainer()
        {
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_loaicontainer_getall");

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            return dt.ConvertTo<LoaiContainerModel>()?.ToList() ?? new();
        }

        /// <summary>
        /// Lấy danh sách loại hàng hóa. Gọi SP: sp_loaihang_getall.
        /// </summary>
        public List<LoaiHangHoaModel> LayDanhSachLoaiHang()
        {
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_loaihang_getall");

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            return dt.ConvertTo<LoaiHangHoaModel>()?.ToList() ?? new();
        }
    }
}
