using System.ComponentModel.DataAnnotations;

namespace FIT5032_IbrahimFinalProject.Models
{
    public class Booking
    {

        public int ID { get; set; }
        [Required]
        public int CustomerID { get; set; }
        [Required]
        public DateTime BookingDate { get; set; }
        public Customer Customer { get; set; }
    }
}
