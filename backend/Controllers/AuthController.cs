using backend.Data;
using backend.Dtos;
using backend.Helpers;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly JwtService _jwtService;

        public AuthController(IUserRepository repository, JwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            // Xử lý yêu cầu đăng ký người dùng mới
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password) // Mã hóa mật khẩu trước khi lưu vào cơ sở dữ liệu
            };

            // Tạo mới người dùng và trả về kết quả thành công
            return Created("success", _repository.Create(user));
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            // Xử lý yêu cầu đăng nhập
            var user = _repository.GetByEmail(dto.Email); // Lấy thông tin người dùng từ email được cung cấp
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                // Kiểm tra xem người dùng tồn tại và mật khẩu có khớp không
                return BadRequest(new { message = "Email or password is incorrect" });
            }

            // Tạo JWT cho người dùng đã xác thực thành công
            var jwt = _jwtService.Generate(user.Id);

            // Lưu JWT vào cookie để sử dụng trong các yêu cầu sau này
            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true, // Đảm bảo rằng cookie chỉ có thể được truy cập bởi máy chủ
                SameSite = SameSiteMode.None, // Đặt SameSite là None để có thể sử dụng ở các trang khác miền
                Secure = true // Phải có HTTPS
            });

            // Trả về kết quả thành công
            return Ok(new
            {
                message = "Login successful"
            });
        }

        [HttpGet("user")]
        public IActionResult User()
        {
            try
            {
                var jwt = Request.Cookies["jwt"]; // Lấy JWT từ cookie của yêu cầu
                if (jwt == null)
                {
                    return Unauthorized(new { message = "JWT token not found" }); // Trả về lỗi nếu không tìm thấy JWT
                }

                var token = _jwtService.Verify(jwt); // Xác thực JWT để lấy thông tin người dùng

                var userId = int.Parse(token.Claims.First(c => c.Type == "sub").Value); // Lấy ID của người dùng từ claim 'sub' trong JWT

                var user = _repository.GetById(userId); // Lấy thông tin người dùng từ ID đã lấy được

                return Ok(user); // Trả về thông tin người dùng thành công
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message }); // Xử lý lỗi nếu có lỗi xảy ra trong quá trình xử lý
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Xử lý yêu cầu đăng xuất
            Response.Cookies.Delete("jwt", new CookieOptions  // Xóa cookie JWT để đăng xuất người dùng
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true
            });
            return Ok(new
            {
                message = "logout success"
            });
        }

    }

}
