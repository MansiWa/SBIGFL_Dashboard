using System.ComponentModel;
using Tokens;

namespace PrintSoftWeb.Models
{
    public class StateMasterModel
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public Guid? s_id { get; set; }


        [DisplayName("Country Name")]
        public string? s_country_name { get; set; }

        [DisplayName("Country Id")]
        public string? s_country_id { get; set; }

        [DisplayName("Code")]
        public string? s_state_code { get; set; }

        [DisplayName("Name")]
        public string? s_state_name { get; set; }
        [DisplayName("Updated By")]
        public string? s_updatedby { get; set; }
        [DisplayName("Status")]
        public string? s_isactive { get; set; }




    }

    public class State
    {
        public StateMasterModel? data { get; set; }
    }
}
