using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Master.Entity;
using Tokens;

namespace Master.Repository.Interface
{
    public interface ILoginDetailsRepository
    {
        public Task<IActionResult> ValidateServer(LoginDetails model);
        public Task<IActionResult> LogOut(TokenInfo model);
        public Task<IActionResult> ValidateEmail(LoginDetails model);
       // public Task<IActionResult> ValidateStaff(string? UserName, string? Password);
        //public Task<IActionResult> ValidateLoginOTP(OTP model);

      //  public  Task<IActionResult> ValidateUserMasterLogin(Login model);
    }
}
