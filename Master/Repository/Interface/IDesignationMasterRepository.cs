using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface IDesignationMasterRepository
    {
        public Task<IActionResult> DesignationMaster(DesignationMaster model);
        public Task<IActionResult> Get(DesignationMaster model);

    }
}
