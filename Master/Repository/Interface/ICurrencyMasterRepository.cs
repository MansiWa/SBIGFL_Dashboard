using Master.API.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{ 
    public interface ICurrencyMasterRepository
    {
        public Task<IActionResult> Currency(CurrencyMaster model);
        public Task<IActionResult> Get(CurrencyMaster model);
    }
}
