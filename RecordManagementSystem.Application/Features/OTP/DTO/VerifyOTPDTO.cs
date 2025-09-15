using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagementSystem.Application.Features.OTP.DTO
{
    public class VerifyOTPDTO
    {
        public string sessionID { get; set; }
        public int OTP { get; set; }
    }
}
