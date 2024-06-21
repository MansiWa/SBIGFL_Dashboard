namespace PrintSoftWeb.Models
{
	public class CustomerModel
	{
        public IEnumerable<FIU_Turnover>? FIU_Turnover { get; set; }
        public IEnumerable<FIU_Outstanding>? FIU_Outstanding { get; set; }
        public IEnumerable<Branchwise_FIU>? Branchwise_FIU { get; set; }
    }
    public class Branchwise_FIU
    {
        public string? Particulars { get; set; }
        public string? col_1 { get; set; }
        public string? col_2 { get; set; }
        public string? col_3 { get; set; }
        public string? col_4 { get; set; }
        public string? col_5 { get; set; }
        public string? LastYearMarch { get; set; }
        public string? FileDate { get; set; }

    }
    public class FIU_Outstanding
    {
        public string? Particulars { get; set; }
        public string? col_1 { get; set; }
        public string? col_2 { get; set; }
        public string? col_3 { get; set; }
        public string? col_4 { get; set; }
        public string? col_5 { get; set; }
        public string? LastYearMarch { get; set; }
        public string? FileDate { get; set; }
        public string? FileDate2 { get; set; }

    }
    public class FIU_Turnover
    {
        public string? Particulars { get; set; }
        public string? col_1 { get; set; }
        public string? col_2 { get; set; }
        public string? col_3 { get; set; }
        public string? col_4 { get; set; }
        public string? col_5 { get; set; }
        public string? LastYearMarch { get; set; }

    }
}
