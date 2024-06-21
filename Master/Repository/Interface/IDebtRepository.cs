using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface IDebtRepository
    {
        public Task<IActionResult> Debt(Debt model);
        public Task<IActionResult> Get(Debt model);
    }
}
