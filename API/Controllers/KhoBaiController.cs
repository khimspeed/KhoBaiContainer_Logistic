using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace API.Controllers
{
    /// <summary>Quản lý Kho bãi - Khu vực - Vị trí</summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class KhoBaiController : ControllerBase
    {
        private readonly IKhoBaiBusiness _bll;
        public KhoBaiController(IKhoBaiBusiness bll) { _bll = bll; }

        // ── KHO ──────────────────────────────────────────────────────────────
        [HttpGet("tim-kiem")]
        public IActionResult TimKiemKho([FromQuery] int trangIndex = 1, [FromQuery] int trangSize = 10, [FromQuery] string ten = "")
        {
            try { return Ok(PhanHoiModel.OK(_bll.TimKiemKho(trangIndex, trangSize, ten))); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpGet("{id}")]
        public IActionResult LayKhoTheoId(string id)
        {
            try
            {
                var data = _bll.LayKhoTheoId(id);
                if (data == null) return NotFound(PhanHoiModel.Loi("Không tìm thấy kho"));
                return Ok(PhanHoiModel.OK(data));
            }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpPost("them-kho")]
        [Authorize(Roles = "Admin")]
        public IActionResult ThemKho([FromBody] ThemKhoBaiRequestDto form)
        {
            if (!ModelState.IsValid) return BadRequest(PhanHoiModel.Loi("Dữ liệu không hợp lệ"));
            try { _bll.TaoKho(form); return Ok(PhanHoiModel.OK(null, "Thêm kho thành công")); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpPut("cap-nhat-kho/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult CapNhatKho(string id, [FromBody] ThemKhoBaiRequestDto form)
        {
            try { _bll.CapNhatKho(id, form); return Ok(PhanHoiModel.OK(null, "Cập nhật kho thành công")); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpDelete("xoa-kho/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult XoaKho(string id)
        {
            try { _bll.XoaKho(id); return Ok(PhanHoiModel.OK(null, "Xóa kho thành công")); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        // ── KHU VỰC ──────────────────────────────────────────────────────────
        [HttpGet("{warehouseId}/khu-vuc")]
        public IActionResult LayKhuVucTheoKho(string warehouseId)
        {
            try { return Ok(PhanHoiModel.OK(_bll.LayKhuVucTheoKho(warehouseId))); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpPost("them-khu-vuc")]
        [Authorize(Roles = "Admin,WarehouseOps")]
        public IActionResult ThemKhuVuc([FromBody] ThemKhuVucRequestDto form)
        {
            if (!ModelState.IsValid) return BadRequest(PhanHoiModel.Loi("Dữ liệu không hợp lệ"));
            try { _bll.TaoKhuVuc(form); return Ok(PhanHoiModel.OK(null, "Thêm khu vực thành công")); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        // ── VỊ TRÍ ───────────────────────────────────────────────────────────
        [HttpGet("khu-vuc/{zoneId}/vi-tri")]
        public IActionResult LayViTriTheoKhuVuc(string zoneId)
        {
            try { return Ok(PhanHoiModel.OK(_bll.LayViTriTheoKhuVuc(zoneId))); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpGet("{warehouseId}/vi-tri-trong")]
        public IActionResult LayViTriTrong(string warehouseId)
        {
            try { return Ok(PhanHoiModel.OK(_bll.LayViTriTrong(warehouseId))); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpPost("them-vi-tri")]
        [Authorize(Roles = "Admin,WarehouseOps")]
        public IActionResult ThemViTri([FromBody] ThemViTriRequestDto form)
        {
            if (!ModelState.IsValid) return BadRequest(PhanHoiModel.Loi("Dữ liệu không hợp lệ"));
            try { _bll.TaoViTri(form); return Ok(PhanHoiModel.OK(null, "Thêm vị trí thành công")); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }
    }
}
