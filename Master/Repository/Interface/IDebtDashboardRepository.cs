
using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface IDebtDashboardRepository
    {
        public Task<IActionResult> Debt(Debt model);
    }
}
