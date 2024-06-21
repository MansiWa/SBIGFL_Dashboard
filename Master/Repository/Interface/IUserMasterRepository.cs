using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface IUserMasterRepository 
    {
        public Task<IActionResult> UserMaster(UserMaster model);
        public Task<IActionResult> Get(UserMaster model);
        public Task<IActionResult> StaffGet(UserMaster model);
    }
}
