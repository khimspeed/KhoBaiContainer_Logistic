using BCrypt.Net;
using DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BLL {
  /// <summary>
  /// Business logic cho NguoiDung: đăng nhập (tạo JWT), đăng ký, cập nhật, xóa mềm, tìm kiếm.
  /// </summary>
  public partial class NguoiDungBusiness : INguoiDungBusiness {
    private readonly INguoiDungRepository _repo;
    private readonly string _jwtSecret;

    public NguoiDungBusiness(INguoiDungRepository repo, IConfiguration config) {
      _repo = repo;
      _jwtSecret = config["AppSettings:Secret"] ?? "DefaultSecretKey";
    }

    /// <summary>
    /// Xác thực tài khoản và mật khẩu, trả về model kèm JWT nếu hợp lệ.
    /// </summary>
    public NguoiDungModel? DangNhap(string taiKhoan, string matKhau) {
      // Lấy người dùng theo tài khoản + mật khẩu (SP hiện dùng plain text)
      // TODO: Nâng cấp lên BCrypt.Verify khi SP lưu hash
      var user = _repo.LayTheoTaiKhoanMatKhau(taiKhoan, matKhau);
      if (user == null) return null;

      // Tạo JWT token
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_jwtSecret);
      var tokenDescriptor = new SecurityTokenDescriptor {
        Subject = new ClaimsIdentity(new[] {
          new Claim(ClaimTypes.NameIdentifier, user.user_id),
          new Claim(ClaimTypes.Name, user.hoten ?? ""),
          new Claim(ClaimTypes.Role, user.role ?? "Staff"),
          new Claim("taikhoan", user.taikhoan ?? "")
        }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(
          new SymmetricSecurityKey(key),
          SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      user.token = tokenHandler.WriteToken(token);
      user.matkhau = null; // Xóa mật khẩu trước khi trả về client
      return user;
    }

    /// <summary>
    /// Tạo tài khoản người dùng mới từ DTO.
    /// </summary>
    public bool DangKy(TaoTaiKhoanRequestDto dto) {
      var model = new NguoiDungModel {
        user_id    = Guid.NewGuid().ToString("N")[..20],
        hoten      = dto.HoTen,
        ngaysinh   = dto.NgaySinh,
        diachi     = dto.DiaChi,
        gioitinh   = dto.GioiTinh,
        email      = dto.Email,
        taikhoan   = dto.TaiKhoan,
        matkhau    = dto.MatKhau, // TODO: BCrypt.HashPassword(dto.MatKhau) sau khi đổi SP
        role       = dto.VaiTro,
        image_url  = dto.HinhAnhUrl
      };
      return _repo.TaomMoi(model);
    }

    /// <summary>
    /// Cập nhật thông tin người dùng — chỉ các trường được truyền vào (null = giữ nguyên).
    /// </summary>
    public bool CapNhat(CapNhatNguoiDungRequestDto dto) {
      var existing = _repo.LayTheoId(dto.MaNguoiDung);
      if (existing == null) throw new Exception("Không tìm thấy người dùng");

      existing.hoten     = dto.HoTen      ?? existing.hoten;
      existing.ngaysinh  = dto.NgaySinh   ?? existing.ngaysinh;
      existing.diachi    = dto.DiaChi     ?? existing.diachi;
      existing.gioitinh  = dto.GioiTinh   ?? existing.gioitinh;
      existing.email     = dto.Email      ?? existing.email;
      existing.role      = dto.VaiTro     ?? existing.role;
      existing.image_url = dto.HinhAnhUrl ?? existing.image_url;

      return _repo.CapNhat(existing);
    }

    /// <summary>Xóa mềm người dùng theo ID.</summary>
    public bool XoaMem(string id) => _repo.XoaMem(id);

    /// <summary>Lấy người dùng theo ID.</summary>
    public NguoiDungModel? LayTheoId(string id) => _repo.LayTheoId(id);

    /// <summary>Tìm kiếm người dùng có phân trang.</summary>
    public PhanHoiPhanTrangModel<NguoiDungModel> TimKiem(int trangIndex, int trangSize, string hoTen, string taiKhoan) {
      var list = _repo.TimKiem(trangIndex, trangSize, out long tongSo, hoTen, taiKhoan);
      return new PhanHoiPhanTrangModel<NguoiDungModel> {
        DuLieu           = list,
        TongSoBanGhi     = tongSo,
        TrangHienTai     = trangIndex,
        SoBanGhiMoiTrang = trangSize
      };
    }
  }
}
