namespace PrintSoftWeb.Models
{
    public class CostOfBorrowingModel
    {
        public Guid? UserId { get; set; }
        public Guid? cob_id { get; set; }
        public string? cob_period { get; set; }
        public string? cob_finantialyear { get; set; }
        public string? cob_includingncd { get; set; }
        public string? cob_excludingncd { get; set; }
       
        public DateTime? cob_createddate { get; set; }
        public string? cob_isactive { get; set; }
    }
}
