using Tokens;

namespace Master.Entity
{
    public class Dashboard
    {
        public BaseModel? BaseModel { get; set; }


        public string? UserId { get; set; }
        public string? date { get; set; }
        public string? rp { get; set; }
        public string? dfyield { get; set; }
        public string? rfyield { get; set; }
        public string[]? client_npa { get; set; }
        public string[]? Product { get; set; }
        public string? ProductString { get; set; }
        public string? npaString { get; set; }
    }
}
