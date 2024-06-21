using Master.API.Entity;
using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface IFileUploadRepository
    { 
        public Task<IActionResult> FileUpload(FileUpload model);
        public Task<IActionResult> Get(FileUpload model);
        public Task<IActionResult> BDUpload(FileUpload model);
        public Task<IActionResult> GetBD(FileUpload model);
    }
}
