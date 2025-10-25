namespace RecordManagementSystem.DTOs.Account
{
    public class JwtRefreshTokenDTO
    {
        //jwt DTOs
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
        public int ExpiresIn { get; set; }
    }
}
