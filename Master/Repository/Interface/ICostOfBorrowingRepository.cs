using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface ICostOfBorrowingRepository
    {
        public Task<IActionResult> CostOfBorrowing(CostOfBorrowing user);
        public Task<IActionResult> Get(CostOfBorrowing user);
    }
}
