using Tokens;

namespace Master.Entity
{
    public class LoginDetails
    {
        //  public string? UserName { get; set; }
        public BaseModel? BaseModel { get; set; }
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
       
        public string? Contact_no { get; set; }
        public string? EmailId { get; set; }
        public string? com_id { get; set; }
        public string? com_password { get; set; }
        public string? NewPassword { get; set; }
        public string? ip_address { get; set; }
        public string? browser_name { get; set; }
        public string? browser_version { get; set; }
        public string? server_Value { get; set; }
        public string? is_signIn { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? LoginId { get; set; }
        public string? com_company_name { get; set; }
        public string? CountryId { get; set; }
    }
}
