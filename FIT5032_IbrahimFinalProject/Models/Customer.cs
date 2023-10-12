namespace FIT5032_IbrahimFinalProject.Models
{
    public class Customer
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string DOB { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BookingDate { get; set; }


        public ICollection<Booking> Bookings { get; set; }
    }
}
