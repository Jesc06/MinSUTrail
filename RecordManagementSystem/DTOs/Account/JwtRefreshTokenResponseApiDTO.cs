namespace RecordManagementSystem.DTOs.Account
{
    public class JwtRefreshTokenResponseApiDTO
    {
        public string newAccessToken { get; set; }
        public string newRefreshToken { get; set; }
    }
}
