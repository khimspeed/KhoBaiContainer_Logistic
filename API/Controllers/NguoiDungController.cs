using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace API.Controllers
{
    /// <summary>Quản lý Người dùng hệ thống</summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NguoiDungController : ControllerBase
    {
        private readonly INguoiDungBusiness _bll;
        public NguoiDungController(INguoiDungBusiness bll) { _bll = bll; }

        /// <summary>Tìm kiếm người dùng có phân trang</summary>
        [HttpGet("tim-kiem")]
        [Authorize(Roles = "Admin")]
        public IActionResult TimKiem([FromQuery] int trangIndex = 1, [FromQuery] int trangSize = 10,
            [FromQuery] string hoTen = "", [FromQuery] string taiKhoan = "")
        {
            try { return Ok(PhanHoiModel.OK(_bll.TimKiem(trangIndex, trangSize, hoTen, taiKhoan))); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        /// <summary>Lấy thông tin người dùng theo ID</summary>
        [HttpGet("{id}")]
        public IActionResult LayTheoId(string id)
        {
            try
            {
                var data = _bll.LayTheoId(id);
                if (data == null) return NotFound(PhanHoiModel.Loi("Không tìm thấy người dùng"));
                data.matkhau = null;
                return Ok(PhanHoiModel.OK(data));
            }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        /// <summary>Cập nhật thông tin người dùng</summary>
        [HttpPut("cap-nhat")]
        public IActionResult CapNhat([FromBody] CapNhatNguoiDungRequestDto form)
        {
            if (!ModelState.IsValid) return BadRequest(PhanHoiModel.Loi("Dữ liệu không hợp lệ"));
            try
            {
                _bll.CapNhat(form);
                return Ok(PhanHoiModel.OK(null, "Cập nhật người dùng thành công"));
            }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        /// <summary>Xóa mềm người dùng (Admin)</summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult XoaMem(string id)
        {
            try
            {
                _bll.XoaMem(id);
                return Ok(PhanHoiModel.OK(null, "Xóa người dùng thành công"));
            }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }
    }
}
