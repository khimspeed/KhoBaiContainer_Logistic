using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace API.Controllers
{
    /// <summary>Dashboard tổng quan + IoT cảm biến nhiệt độ/độ ẩm</summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ThongKeController : ControllerBase
    {
        private readonly IIoTBusiness _bll;
        public ThongKeController(IIoTBusiness bll) { _bll = bll; }

        /// <summary>Dashboard: Tổng số container, TEU đã dùng, quá hạn, doanh thu, hóa đơn chưa trả</summary>
        [HttpGet("dashboard")]
        public IActionResult LayDashboard()
        {
            try { return Ok(PhanHoiModel.OK(_bll.LayDashboard())); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        /// <summary>Ghi nhận thông số IoT (nhiệt độ, độ ẩm) cho container lạnh</summary>
        [HttpPost("iot/ghi-nhan")]
        [AllowAnonymous] // Cho phep thiet bi IoT gui du lieu khong can token
        public IActionResult GhiNhanIoT([FromBody] GhiNhanIoTRequestDto form)
        {
            if (!ModelState.IsValid) return BadRequest(PhanHoiModel.Loi("Dữ liệu không hợp lệ"));
            try
            {
                var kq = _bll.GhiNhanIoT(form);
                if (!kq.ThanhCong) return BadRequest(kq);
                return Ok(kq);
            }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        /// <summary>Lịch sử IoT theo container (mặc định 50 bản ghi gần nhất)</summary>
        [HttpGet("iot/{containerId}")]
        public IActionResult LayLichSuIoT(string containerId, [FromQuery] int gioiHan = 50)
        {
            try { return Ok(PhanHoiModel.OK(_bll.LayLichSuIoT(containerId, gioiHan))); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }
    }
}
