using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace API.Controllers
{
    /// <summary>Quản lý Hợp đồng / Hóa đơn / Thanh toán</summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonBusiness _bll;
        private readonly IContainerBusiness _containerBll;
        public HoaDonController(IHoaDonBusiness bll, IContainerBusiness containerBll)
        {
            _bll = bll;
            _containerBll = containerBll;
        }

        // ── HỢP ĐỒNG ─────────────────────────────────────────────────────────
        [HttpGet("hop-dong/tim-kiem")]
        public IActionResult TimKiemHopDong([FromQuery] int ti = 1, [FromQuery] int ts = 10, [FromQuery] string maKhachHang = "")
        {
            try { return Ok(PhanHoiModel.OK(_bll.TimKiemHopDong(ti, ts, maKhachHang))); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpGet("hop-dong/{id}")]
        public IActionResult LayHopDongTheoId(string id)
        {
            try
            {
                var data = _bll.LayHopDongTheoId(id);
                if (data == null) return NotFound(PhanHoiModel.Loi("Không tìm thấy hợp đồng"));
                return Ok(PhanHoiModel.OK(data));
            }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpPost("hop-dong/tao-moi")]
        [Authorize(Roles = "Admin,Dispatcher")]
        public IActionResult TaoHopDong([FromBody] TaoHopDongRequestDto form)
        {
            if (!ModelState.IsValid) return BadRequest(PhanHoiModel.Loi("Dữ liệu không hợp lệ"));
            try
            {
                var kq = _bll.TaoHopDong(form);
                if (!kq.ThanhCong) return BadRequest(kq);
                return Ok(kq);
            }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpDelete("hop-dong/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult XoaHopDong(string id)
        {
            try { _bll.XoaHopDong(id); return Ok(PhanHoiModel.OK(null, "Xóa hợp đồng thành công")); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        // ── HÓA ĐƠN ──────────────────────────────────────────────────────────
        [HttpGet("tim-kiem")]
        public IActionResult TimKiemHoaDon([FromQuery] int ti = 1, [FromQuery] int ts = 10,
            [FromQuery] string maKhachHang = "", [FromQuery] string trangThai = "")
        {
            try { return Ok(PhanHoiModel.OK(_bll.TimKiemHoaDon(ti, ts, maKhachHang, trangThai))); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpGet("{id}")]
        public IActionResult LayHoaDonTheoId(string id)
        {
            try
            {
                var data = _bll.LayHoaDonTheoId(id);
                if (data == null) return NotFound(PhanHoiModel.Loi("Không tìm thấy hóa đơn"));
                return Ok(PhanHoiModel.OK(data));
            }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpPost("tao-moi")]
        [Authorize(Roles = "Admin,Dispatcher")]
        public IActionResult TaoHoaDon([FromBody] TaoHoaDonRequestDto form)
        {
            if (!ModelState.IsValid) return BadRequest(PhanHoiModel.Loi("Dữ liệu không hợp lệ"));
            try
            {
                var kq = _bll.TaoHoaDon(form);
                if (!kq.ThanhCong) return BadRequest(kq);
                return Ok(kq);
            }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult XoaHoaDon(string id)
        {
            try { _bll.XoaHoaDon(id); return Ok(PhanHoiModel.OK(null, "Xóa hóa đơn thành công")); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        /// <summary>Tính phí lưu bãi cho container theo hợp đồng</summary>
        [HttpGet("tinh-phi-luu-bai")]
        public IActionResult TinhPhiLuuBai([FromQuery] string maContainerDb, [FromQuery] string maHopDongDb)
        {
            try
            {
                var container = _containerBll.LayTheoId(maContainerDb);
                var hopDong = _bll.LayHopDongTheoId(maHopDongDb);
                if (container == null) return NotFound(PhanHoiModel.Loi("Không tìm thấy container"));
                if (hopDong == null) return NotFound(PhanHoiModel.Loi("Không tìm thấy hợp đồng"));
                var phi = _bll.TinhPhiLuuBai(container, hopDong);
                return Ok(PhanHoiModel.OK(new { PhiLuuBai = phi }, "Tính phí thành công"));
            }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        // ── THANH TOÁN ────────────────────────────────────────────────────────
        [HttpGet("{invoiceId}/thanh-toan")]
        public IActionResult LayThanhToanTheoHoaDon(string invoiceId)
        {
            try { return Ok(PhanHoiModel.OK(_bll.LayThanhToanTheoHoaDon(invoiceId))); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpPost("ghi-nhan-thanh-toan")]
        [Authorize(Roles = "Admin,Dispatcher")]
        public IActionResult GhiNhanThanhToan([FromBody] GhiNhanThanhToanRequestDto form)
        {
            if (!ModelState.IsValid) return BadRequest(PhanHoiModel.Loi("Dữ liệu không hợp lệ"));
            try
            {
                var kq = _bll.GhiNhanThanhToan(form);
                if (!kq.ThanhCong) return BadRequest(kq);
                return Ok(kq);
            }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }
    }
}
