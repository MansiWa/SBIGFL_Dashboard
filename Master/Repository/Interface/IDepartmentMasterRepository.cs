using Master.API.Entity;
using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{ 
    public interface IDepartmentMasterRepository
    {
        public Task<IActionResult> Department(DepartmentMaster model);
        public Task<IActionResult> Get(DepartmentMaster model);
    }
}
