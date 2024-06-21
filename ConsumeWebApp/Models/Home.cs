using Tokens;

namespace PrintSoftWeb.Models
{
    public class Home
    {
        public BaseModel? BaseModel { get; set; }
        public string? RoleId { get; set; }
        public string? ParentId { get; set; }
        public string?  m_api { get; set; }
        public string?  m_menuname { get; set; }
        public string? m_id { get; set; }
        public string? m_action { get; set; }
        public string? m_icon { get; set; }
        public string? m_controller { get; set; }
        public string? Id { get; set; }
    }
    public class HomeC
    {
        public Home? data { get; set; }
    }
}
