namespace PrintSoftWeb.Models
{
    public class Dept
    {
        public string? d_Client_Name { get; set; }
        public string? d_Asset_Class { get; set; }
        public string? d_Branch { get; set; }
        public string? d_Principal_Outstanding { get; set; }
        public string? d_Date_of_Sanction { get; set; }
        public string? d_Date_Of_NPA { get; set; }
        public string? d_Date_of_writeOff { get; set; }
        public string? d_Provision_As_per_IGAAP { get; set; }
        public string? d_Provision_As_per_IndAS { get; set; }
        public string? d_Facility { get; set; }
        public string? d_Overdue_Bucket { get; set; }
        public string? d_Account_Limit { get; set; }
        public string? d_FIU_Outstanding { get; set; }
        public string? d_FIU_Overdue { get; set; }
        public string? d_Pending_Interest { get; set; }
        public string? d_Pending_Charges { get; set; }
        public string? d_Max_Overdue_Days { get; set; }
        public Guid? d_id { get; set; }
        public string? d_date { get; set; }
        public string? d_updatedby { get; set; }
        public string? d_isactive { get; set; }
        public DateTime? d_updateddate { get; set; }
    }
}
