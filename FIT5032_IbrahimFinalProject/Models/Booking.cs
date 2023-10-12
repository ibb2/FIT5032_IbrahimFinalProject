namespace FIT5032_IbrahimFinalProject.Models
{
    public class Booking
    {

        public int ID { get; set; }
        public int CustomerID { get; set; }
        public DateTime BookingDate { get; set; }
        public Customer Customer { get; set; }
    }
}
