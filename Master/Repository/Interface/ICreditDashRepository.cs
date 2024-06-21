using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface ICreditDashRepository
    {
        public Task<IActionResult> Credit(Dashboard model);

    }
}
