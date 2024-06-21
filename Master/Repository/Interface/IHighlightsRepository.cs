using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface IHighlightsRepository
    { 
        public Task<IActionResult> Highlights(Highlights model);
        public Task<IActionResult> Get(Highlights model);
    }
}
