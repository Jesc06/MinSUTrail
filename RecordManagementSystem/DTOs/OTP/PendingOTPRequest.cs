using RecordManagementSystem.Application.Features.Account.DTO;

namespace RecordManagementSystem.DTOs.OTP
{
    public class PendingOTPRequest
    {
        public AddStudentAccountDTO account { get; set; }
        public int OTP { get; set; }
    }
}
