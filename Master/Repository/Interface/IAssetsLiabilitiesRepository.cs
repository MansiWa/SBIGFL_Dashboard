using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface IAssetsLiabilitiesRepository
    {
        public Task<IActionResult> AssetsLiabilities(AssetsLiabilities user);
        public Task<IActionResult> Get(AssetsLiabilities user);
    }
}
