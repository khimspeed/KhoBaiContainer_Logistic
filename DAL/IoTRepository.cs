using DAL.Helper;
using Model;

namespace DAL
{
    /// <summary>
    /// Repository xử lý nghiệp vụ dữ liệu IoT (cảm biến container)
    /// và thống kê tổng quan dashboard thông qua Stored Procedure.
    /// </summary>
    public partial class IoTRepository : IIoTRepository
    {
        private readonly IDatabaseHelper _db;

        public IoTRepository(IDatabaseHelper db)
        {
            _db = db;
        }

        /// <summary>
        /// Ghi nhận thông số IoT (nhiệt độ, độ ẩm) cho container.
        /// Gọi SP: sp_iot_ghinhan.
        /// </summary>
        public bool GhiNhan(ThongSoIoTModel m)
        {
            string err;
            var r = _db.ExecuteScalarSProcedureWithTransaction(out err, "sp_iot_ghinhan",
                "@reading_id",   m.reading_id,
                "@container_id", m.container_id,
                "@temperature",  m.nhiet_do,
                "@humidity",     m.do_am);

            if ((r != null && !string.IsNullOrEmpty(r.ToString())) || !string.IsNullOrEmpty(err))
                throw new Exception(r + err);

            return true;
        }

        /// <summary>
        /// Lấy danh sách thông số IoT theo container với giới hạn số bản ghi.
        /// Gọi SP: sp_iot_get_by_container.
        /// </summary>
        public List<ThongSoIoTModel> LayTheoContainer(string containerId, int gioi_han)
        {
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_iot_get_by_container",
                "@container_id", containerId,
                "@top",          gioi_han);

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            return dt.ConvertTo<ThongSoIoTModel>()?.ToList() ?? new();
        }

        /// <summary>
        /// Lấy số liệu tổng quan cho dashboard (tổng container, tỷ lệ lấp đầy, v.v.).
        /// Gọi SP: sp_dashboard_tonquan.
        /// </summary>
        public ThongKeDashboardModel LayThongKeDashboard()
        {
            string err;
            var dt = _db.ExecuteSProcedureReturnDataTable(out err, "sp_dashboard_tonquan");

            if (!string.IsNullOrEmpty(err))
                throw new Exception(err);

            return dt.ConvertTo<ThongKeDashboardModel>()?.FirstOrDefault() ?? new();
        }
    }
}
