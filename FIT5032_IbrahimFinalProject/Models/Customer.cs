﻿using System.ComponentModel.DataAnnotations;

namespace FIT5032_IbrahimFinalProject.Models
{
    public class Customer
    {
        public int ID { get; set; }
        [Required]
        public string? UserId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNo { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public string DOB { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
    }
}
