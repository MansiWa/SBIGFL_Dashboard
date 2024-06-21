using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface IRFRDocRepository
    {
        public Task<IActionResult> Report(BorroAndRFR user);
        public Task<IActionResult> Get(BorroAndRFR user);
    }
}