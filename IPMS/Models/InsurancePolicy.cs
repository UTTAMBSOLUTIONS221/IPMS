namespace IPMS.Models
{
    public class InsurancePolicy
    {
        public int Id { get; set; }
        public string? PolicyNumber { get; set; }
        public string? PolicyType { get; set; }
        public decimal PremiumAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int PolicyHolderId { get; set; }
        public PolicyHolder? PolicyHolder { get; set; }
    }
}
