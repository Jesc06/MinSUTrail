namespace RecordManagementSystem.DTOs.Account
{
    public class JwtRefreshTokenResponseApi
    {
        public string newAccessToken { get; set; }
        public string newRefreshToken { get; set; }
    }
}
