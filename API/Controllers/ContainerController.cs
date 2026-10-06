using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Security.Claims;

namespace API.Controllers
{
    /// <summary>Quản lý Container - Nhập bãi / Xuất bãi / Chuyển vị trí</summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContainerController : ControllerBase
    {
        private readonly IContainerBusiness _bll;
        public ContainerController(IContainerBusiness bll) { _bll = bll; }

        private string LayMaNguoiDung() =>
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "he-thong";

        // ── DANH MỤC ──────────────────────────────────────────────────────────
        [HttpGet("loai-container")]
        public IActionResult LayDanhSachLoaiContainer()
        {
            try { return Ok(PhanHoiModel.OK(_bll.LayDanhSachLoaiContainer())); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpGet("loai-hang-hoa")]
        public IActionResult LayDanhSachLoaiHang()
        {
            try { return Ok(PhanHoiModel.OK(_bll.LayDanhSachLoaiHang())); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        // ── CONTAINER ─────────────────────────────────────────────────────────
        [HttpGet("tim-kiem")]
        public IActionResult TimKiem([FromQuery] int trangIndex = 1, [FromQuery] int trangSize = 10,
            [FromQuery] string soContainer = "", [FromQuery] string trangThai = "", [FromQuery] string maKho = "")
        {
            try { return Ok(PhanHoiModel.OK(_bll.TimKiem(trangIndex, trangSize, soContainer, trangThai, maKho))); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpGet("{id}")]
        public IActionResult LayTheoId(string id)
        {
            try
            {
                var data = _bll.LayTheoId(id);
                if (data == null) return NotFound(PhanHoiModel.Loi("Không tìm thấy container"));
                return Ok(PhanHoiModel.OK(data));
            }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        [HttpGet("qua-han")]
        public IActionResult LayContainerQuaHan()
        {
            try { return Ok(PhanHoiModel.OK(_bll.LayContainerQuaHan())); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        // ── NGHIỆP VỤ NHẬP/XUẤT BÃI ─────────────────────────────────────────
        /// <summary>Nhập container vào bãi - Gán vị trí slot, tính ngày miễn phí</summary>
        [HttpPost("nhap-bai")]
        [Authorize(Roles = "Admin,Dispatcher,WarehouseOps")]
        public IActionResult NhapBai([FromBody] NhapBaiRequestDto form)
        {
            if (!ModelState.IsValid) return BadRequest(PhanHoiModel.Loi("Dữ liệu không hợp lệ"));
            try
            {
                var ketQua = _bll.NhapBai(form, LayMaNguoiDung());
                if (!ketQua.ThanhCong) return BadRequest(ketQua);
                return Ok(ketQua);
            }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        /// <summary>Xuất container ra khỏi bãi</summary>
        [HttpPost("xuat-bai")]
        [Authorize(Roles = "Admin,Dispatcher,WarehouseOps")]
        public IActionResult XuatBai([FromBody] XuatBaiRequestDto form)
        {
            if (!ModelState.IsValid) return BadRequest(PhanHoiModel.Loi("Dữ liệu không hợp lệ"));
            try
            {
                var ketQua = _bll.XuatBai(form, LayMaNguoiDung());
                if (!ketQua.ThanhCong) return BadRequest(ketQua);
                return Ok(ketQua);
            }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        /// <summary>Chuyển container sang vị trí slot khác trong bãi</summary>
        [HttpPost("chuyen-vi-tri")]
        [Authorize(Roles = "Admin,Dispatcher,WarehouseOps")]
        public IActionResult ChuyenViTri([FromBody] ChuyenViTriRequestDto form)
        {
            if (!ModelState.IsValid) return BadRequest(PhanHoiModel.Loi("Dữ liệu không hợp lệ"));
            try
            {
                var ketQua = _bll.ChuyenViTri(form, LayMaNguoiDung());
                if (!ketQua.ThanhCong) return BadRequest(ketQua);
                return Ok(ketQua);
            }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }

        // ── LỊCH SỬ DI CHUYỂN ────────────────────────────────────────────────
        [HttpGet("{containerId}/lich-su-di-chuyen")]
        public IActionResult LayLichSuDiChuyen(string containerId, [FromQuery] int trangIndex = 1, [FromQuery] int trangSize = 20)
        {
            try { return Ok(PhanHoiModel.OK(_bll.LayLichSuDiChuyen(containerId, trangIndex, trangSize))); }
            catch (Exception ex) { return StatusCode(500, PhanHoiModel.Loi(ex.Message)); }
        }
    }
}
