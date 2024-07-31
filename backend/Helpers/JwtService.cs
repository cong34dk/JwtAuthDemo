using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace backend.Helpers
{
    public class JwtService
    {
        private readonly string _secureKey;

        public JwtService(string secureKey)
        {
            // Khởi tạo dịch vụ JWT với một khóa bảo mật được cung cấp
            _secureKey = secureKey ?? throw new ArgumentNullException(nameof(secureKey));
        }

        public string Generate(int id)
        {
            // Tạo mã JWT từ một ID được cung cấp
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secureKey));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            // Tạo phần header của JWT
            var header = new JwtHeader(credentials);

            // Tạo phần payload của JWT
            var payload = new JwtPayload
            {
                { "sub", id.ToString() }, // Chỉ định 'sub' (subject) là ID của người dùng
                { "exp", new DateTimeOffset(DateTime.UtcNow.AddDays(7)).ToUnixTimeSeconds() } // Chỉ định 'exp' (expiration) là thời gian hết hạn của JWT (7 ngày sau khi được tạo)
            };

            // Tạo một JWT từ header và payload đã tạo
            var securityToken = new JwtSecurityToken(header, payload);

            // Trả về chuỗi JWT đã tạo dưới dạng một chuỗi ký tự
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public JwtSecurityToken Verify(string jwt)
        {
            // Xác thực và giải mã chuỗi JWT được cung cấp
            if (string.IsNullOrEmpty(jwt))
            {
                throw new ArgumentException("JWT token cannot be null or empty", nameof(jwt));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secureKey);

            // Xác thực JWT với các tham số đặt ra
            tokenHandler.ValidateToken(jwt, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key), // Sử dụng khóa bí mật để xác minh chữ ký của JWT
                ValidateIssuerSigningKey = true, // Xác minh rằng khóa được cung cấp là hợp lệ
                ValidateIssuer = false, // Không yêu cầu xác minh nguồn phát hành của JWT
                ValidateAudience = false, // Không yêu cầu xác minh đối tượng người dùng của JWT
                ClockSkew = TimeSpan.Zero // Đảm bảo thời gian hết hạn của JWT được xác định chính xác
            }, out SecurityToken validatedToken);

            // Trả về JWT đã xác thực
            return (JwtSecurityToken)validatedToken;
        }

    }
}
