using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace API.Controllers
{
    /// <summary>Xác thực người dùng - Đăng nhập / Đăng ký</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class XacThucController : ControllerBase
    {
        private readonly INguoiDungBusiness _nguoiDungBusiness;

        public XacThucController(INguoiDungBusiness nguoiDungBusiness)
        {
            _nguoiDungBusiness = nguoiDungBusiness;
        }

        /// <summary>Đăng nhập hệ thống, trả về JWT token</summary>
        [HttpPost("dang-nhap")]
        [AllowAnonymous]
        public IActionResult DangNhap([FromBody] DangNhapRequestDto form)
        {
            if (!ModelState.IsValid)
                return BadRequest(PhanHoiModel.Loi("Dữ liệu không hợp lệ"));
            try
            {
                var user = _nguoiDungBusiness.DangNhap(form.TaiKhoan, form.MatKhau);
                if (user == null)
                    return Unauthorized(PhanHoiModel.Loi("Sai tài khoản hoặc mật khẩu"));
                return Ok(PhanHoiModel.OK(user, "Đăng nhập thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, PhanHoiModel.Loi(ex.Message));
            }
        }

        /// <summary>Đăng ký tài khoản mới (chỉ Admin)</summary>
        [HttpPost("dang-ky")]
        [Authorize(Roles = "Admin")]
        public IActionResult DangKy([FromBody] TaoTaiKhoanRequestDto form)
        {
            if (!ModelState.IsValid)
                return BadRequest(PhanHoiModel.Loi("Dữ liệu không hợp lệ"));
            try
            {
                var ketQua = _nguoiDungBusiness.DangKy(form);
                if (!ketQua)
                    return BadRequest(PhanHoiModel.Loi("Đăng ký thất bại"));
                return Ok(PhanHoiModel.OK(null, "Đăng ký tài khoản thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, PhanHoiModel.Loi(ex.Message));
            }
        }
    }
}
