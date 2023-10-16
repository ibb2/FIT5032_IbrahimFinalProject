namespace FIT5032_IbrahimFinalProject.Models
{
    public class Rating
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public int? StaffID { get; set; }

        public int RatingScore { get; set; }
        public string? Message { get; set; }
        public string? Reply { get; set; }
        public string? Location { get; set; }

        public Customer? Customer { get; set; }
    }
}
