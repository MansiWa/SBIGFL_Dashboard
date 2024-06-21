using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface IAccountDashboardRepository
    {
        public Task<IActionResult> AccountDashboard(AccountDashboard model);
        public Task<IActionResult> CustomerDashboard(AccountDashboard model);
        public Task<IActionResult> BDDashboard(AccountDashboard model);
    }
}
