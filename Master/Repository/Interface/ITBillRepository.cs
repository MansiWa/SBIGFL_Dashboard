using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface ITBillRepository
    {
        public Task<IActionResult> TBill(TBill user);
        public Task<IActionResult> Get(TBill user);
    }
}
