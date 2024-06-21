using Master.Entity;
using Microsoft.AspNetCore.Mvc;
namespace Master.Repository.Interface
{
    public interface IRoleMasterRepository
    {
        public Task<IActionResult> Role(RoleMaster model);
        public Task<IActionResult> Get(RoleMaster model);
    }
}


