using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace API.Controllers
{
    /// <summary>Quản lý Vận chuyển - Phương tiện / Tàu biển / Chuyến hàng đa chặng</summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VanChuyenController : ControllerBase
    {
        private readonly IVanChuyenBusiness _bll;
        public VanChuyenController(IVanChuyenBusiness bll) { _bll = bll; }

        // ── PHƯƠNG TIỆN ───────────────────────────────────────────────────────
        [HttpGet("phuong-tien")]
        public IActionResult LayDanhSachPhuongTien([FromQuery] int ti = 1, [FromQuery] int ts = 10, [FromQuery] string bienSo = "")
        {
            try { return Ok(PhanHoiModel.OK(_bll.LayDanhSachPhuongTien(ti, ts, bienSo))); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpPost("phuong-tien/them-moi")]
        [Authorize(Roles = "Admin,Dispatcher")]
        public IActionResult ThemPhuongTien([FromBody] PhuongTienModel form)
        {
            try { _bll.TaoPhuongTien(form); return Ok(PhanHoiModel.OK(null, "Thêm phương tiện thành công")); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpPut("phuong-tien/cap-nhat")]
        [Authorize(Roles = "Admin,Dispatcher")]
        public IActionResult CapNhatPhuongTien([FromBody] PhuongTienModel form)
        {
            try { _bll.CapNhatPhuongTien(form); return Ok(PhanHoiModel.OK(null, "Cập nhật phương tiện thành công")); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        // ── TÀU BIỂN ──────────────────────────────────────────────────────────
        [HttpGet("tau-bien")]
        public IActionResult LayDanhSachTauBien([FromQuery] int ti = 1, [FromQuery] int ts = 10)
        {
            try { return Ok(PhanHoiModel.OK(_bll.LayDanhSachTauBien(ti, ts))); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpPost("tau-bien/them-moi")]
        [Authorize(Roles = "Admin,Dispatcher")]
        public IActionResult ThemTauBien([FromBody] TauBienModel form)
        {
            try { _bll.TaoTauBien(form); return Ok(PhanHoiModel.OK(null, "Thêm tàu biển thành công")); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        // ── CHUYẾN HÀNG ───────────────────────────────────────────────────────
        [HttpGet("chuyen-hang/tim-kiem")]
        public IActionResult TimKiemChuyenHang([FromQuery] int ti = 1, [FromQuery] int ts = 10,
            [FromQuery] string maVanDon = "", [FromQuery] string trangThai = "")
        {
            try { return Ok(PhanHoiModel.OK(_bll.TimKiemChuyenHang(ti, ts, maVanDon, trangThai))); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpGet("chuyen-hang/{id}")]
        public IActionResult LayChuyenHangTheoId(string id)
        {
            try
            {
                var data = _bll.LayChuyenHangTheoId(id);
                if (data == null) return NotFound(PhanHoiModel.Loi("Không tìm thấy chuyến hàng"));
                return Ok(PhanHoiModel.OK(data));
            }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpPost("chuyen-hang/tao-moi")]
        [Authorize(Roles = "Admin,Dispatcher")]
        public IActionResult TaoChuyenHang([FromBody] TaoChuyenHangRequestDto form)
        {
            if (!ModelState.IsValid) return BadRequest(PhanHoiModel.Loi("Dữ liệu không hợp lệ"));
            try
            {
                var kq = _bll.TaoChuyenHang(form);
                if (!kq.ThanhCong) return BadRequest(kq);
                return Ok(kq);
            }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpPut("chuyen-hang/{id}/trang-thai")]
        [Authorize(Roles = "Admin,Dispatcher")]
        public IActionResult CapNhatTrangThaiChuyenHang(string id, [FromQuery] string trangThai)
        {
            try { _bll.CapNhatTrangThaiChuyenHang(id, trangThai); return Ok(PhanHoiModel.OK(null, "Cập nhật trạng thái thành công")); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpDelete("chuyen-hang/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult XoaChuyenHang(string id)
        {
            try { _bll.XoaChuyenHang(id); return Ok(PhanHoiModel.OK(null, "Xóa chuyến hàng thành công")); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        // ── CHẶNG VẬN CHUYỂN ─────────────────────────────────────────────────
        [HttpGet("chuyen-hang/{shipmentId}/cac-chang")]
        public IActionResult LayChangTheoChuyenHang(string shipmentId)
        {
            try { return Ok(PhanHoiModel.OK(_bll.LayChangTheoChuyenHang(shipmentId))); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpPost("chang/them-moi")]
        [Authorize(Roles = "Admin,Dispatcher")]
        public IActionResult ThemChang([FromBody] ThemChangRequestDto form)
        {
            if (!ModelState.IsValid) return BadRequest(PhanHoiModel.Loi("Dữ liệu không hợp lệ"));
            try
            {
                var kq = _bll.ThemChang(form);
                if (!kq.ThanhCong) return BadRequest(kq);
                return Ok(kq);
            }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }
    }
}
