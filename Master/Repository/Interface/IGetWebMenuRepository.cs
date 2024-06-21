using Master.API.Entity;
using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface IGetWebMenuRepository
    {
        public Task<IActionResult> GetWebMenuR(GetWebMenu model);
    }
}
