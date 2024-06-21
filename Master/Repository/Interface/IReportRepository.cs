using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface IReportRepository
    {
        public Task<IActionResult> Report(Report user);
        public Task<IActionResult> Get(Report user);

    }
}
