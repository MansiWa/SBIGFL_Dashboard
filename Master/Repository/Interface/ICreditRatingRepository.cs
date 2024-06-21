using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface ICreditRatingRepository
    {
        public Task<IActionResult> CreditRating(CreditRating user);
        public Task<IActionResult> Get(CreditRating user);
    }
}
