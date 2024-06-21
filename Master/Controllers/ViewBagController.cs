using Common.Token;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Context;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewBagController : ControllerBase
    {
        private readonly DapperContext dapper;
        public ViewBagController(DapperContext dapperContext)
        {
            dapper = dapperContext;

        }
        [HttpGet("GetViewBag")]
        public Task<IActionResult> GetViewBag(string sTableName, string id, string sValue, string IsActiveColumn, string? sCoulmnName, string? sColumnValue, string? sCompanyCode)
        {
            try
            {
                FillDropdown user = new FillDropdown();


                IList<SelectListDto> selectListDtos = new List<SelectListDto>();

                FillDropdown fillDropdownModel = new FillDropdown();
                fillDropdownModel.sTableName = sTableName;
                fillDropdownModel.Id = id;
                fillDropdownModel.Value = sValue;
                fillDropdownModel.sColumnName = sCoulmnName;
                fillDropdownModel.sColumnValue = sColumnValue;
                fillDropdownModel.IsActiveColumn = IsActiveColumn;
                FillDropdownRepository _dropdown = new FillDropdownRepository(dapper);



                var createduser = _dropdown.GetFillDropDown(fillDropdownModel);
                //var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
