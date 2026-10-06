using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace API.Controllers
{
    /// <summary>Quản lý Khách hàng</summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class KhachHangController : ControllerBase
    {
        private readonly IKhachHangBusiness _bll;
        public KhachHangController(IKhachHangBusiness bll) { _bll = bll; }

        [HttpGet("tim-kiem")]
        public IActionResult TimKiem([FromQuery] int trangIndex = 1, [FromQuery] int trangSize = 10, [FromQuery] string ten = "")
        {
            try { return Ok(PhanHoiModel.OK(_bll.TimKiem(trangIndex, trangSize, ten))); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpGet("{id}")]
        public IActionResult LayTheoId(string id)
        {
            try
            {
                var data = _bll.LayTheoId(id);
                if (data == null) return NotFound(PhanHoiModel.Loi("Không tìm thấy khách hàng"));
                return Ok(PhanHoiModel.OK(data));
            }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpPost("them-moi")]
        [Authorize(Roles = "Admin,Dispatcher")]
        public IActionResult ThemMoi([FromBody] ThemKhachHangRequestDto form)
        {
            if (!ModelState.IsValid) return BadRequest(PhanHoiModel.Loi("Dữ liệu không hợp lệ"));
            try { _bll.TaoMoi(form); return Ok(PhanHoiModel.OK(null, "Thêm khách hàng thành công")); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpPut("cap-nhat")]
        [Authorize(Roles = "Admin,Dispatcher")]
        public IActionResult CapNhat([FromBody] CapNhatKhachHangRequestDto form)
        {
            if (!ModelState.IsValid) return BadRequest(PhanHoiModel.Loi("Dữ liệu không hợp lệ"));
            try { _bll.CapNhat(form); return Ok(PhanHoiModel.OK(null, "Cập nhật khách hàng thành công")); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult XoaMem(string id)
        {
            try { _bll.XoaMem(id); return Ok(PhanHoiModel.OK(null, "Xóa khách hàng thành công")); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }
    }
}
