using Master.API.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{ 
    public interface ICurrencyRateMasterRepository
    {
        public Task<IActionResult> CurrencyRate(CurrencyRateMaster model);
        public Task<IActionResult> Get(CurrencyRateMaster model);


    }
}






