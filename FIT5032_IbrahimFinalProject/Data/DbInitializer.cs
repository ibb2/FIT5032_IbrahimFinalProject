using System;
using System.Linq;
using FIT5032_IbrahimFinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FIT5032_IbrahimFinalProject.Data
{
    public class DbInitializer
    {
        public static void Initialize(ClinicContext context)
        {
            context.Database.EnsureCreated();

            if (context.Customers.Any())
            {
                return;
            }

            // Hardcoded fake customer data
            var customers = new Customer[]
            {
                new Customer
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    PhoneNo = "123-456-7890",
                    DOB = "1990-01-01",
                    BookingDate = DateTime.Now
                },
                new Customer
                {
                    FirstName = "Alice",
                    LastName = "Smith",
                    Email = "alice.smith@example.com",
                    PhoneNo = "555-123-4567",
                    DOB = "1985-03-15",
                    BookingDate = DateTime.Now
                },
                new Customer
                {
                    FirstName = "Bob",
                    LastName = "Johnson",
                    Email = "bob.johnson@example.com",
                    PhoneNo = "222-333-4444",
                    DOB = "1978-08-20",
                    BookingDate = DateTime.Now
                },
                new Customer
                {
                    FirstName = "Sarah",
                    LastName = "Wilson",
                    Email = "sarah.wilson@example.com",
                    PhoneNo = "777-888-9999",
                    DOB = "1995-12-10",
                    BookingDate = DateTime.Now
                },
                new Customer
                {
                    FirstName = "Michael",
                    LastName = "Brown",
                    Email = "michael.brown@example.com",
                    PhoneNo = "555-666-7777",
                    DOB = "1980-04-05",
                    BookingDate = DateTime.Now
                },
                new Customer
                {
                    FirstName = "Emma",
                    LastName = "Davis",
                    Email = "emma.davis@example.com",
                    PhoneNo = "999-888-7777",
                    DOB = "1992-06-28",
                    BookingDate = DateTime.Now
                },
                new Customer
                {
                    FirstName = "Daniel",
                    LastName = "Lee",
                    Email = "daniel.lee@example.com",
                    PhoneNo = "111-222-3333",
                    DOB = "1987-02-14",
                    BookingDate = DateTime.Now
                },
                new Customer
                {
                    FirstName = "Olivia",
                    LastName = "White",
                    Email = "olivia.white@example.com",
                    PhoneNo = "333-444-5555",
                    DOB = "1989-11-03",
                    BookingDate = DateTime.Now
                },
                new Customer
                {
                    FirstName = "William",
                    LastName = "Harris",
                    Email = "william.harris@example.com",
                    PhoneNo = "666-777-8888",
                    DOB = "1982-09-18",
                    BookingDate = DateTime.Now
                },
                new Customer
                {
                    FirstName = "Ava",
                    LastName = "Jackson",
                    Email = "ava.jackson@example.com",
                    PhoneNo = "444-555-6666",
                    DOB = "1997-07-22",
                    BookingDate = DateTime.Now
                }
            };

            // Add customers to the context
            foreach (Customer c in customers)
            {
                context.Customers.Add(c);
            }

            context.SaveChanges();

            // Hardcoded fake booking data
            var bookings = new Booking[]
            {
                new Booking
                {
                    CustomerID = customers[0].ID,
                    BookingDate = DateTime.Now.AddHours(1)
                },
                new Booking
                {
                    CustomerID = customers[1].ID,
                    BookingDate = DateTime.Now.AddHours(2)
                },
                new Booking
                {
                    CustomerID = customers[2].ID,
                    BookingDate = DateTime.Now.AddHours(3)
                },
                new Booking
                {
                    CustomerID = customers[3].ID,
                    BookingDate = DateTime.Now.AddHours(4)
                },
                new Booking
                {
                    CustomerID = customers[4].ID,
                    BookingDate = DateTime.Now.AddHours(5)
                },
                new Booking
                {
                    CustomerID = customers[5].ID,
                    BookingDate = DateTime.Now.AddHours(6)
                },
                new Booking
                {
                    CustomerID = customers[6].ID,
                    BookingDate = DateTime.Now.AddHours(7)
                },
                new Booking
                {
                    CustomerID = customers[7].ID,
                    BookingDate = DateTime.Now.AddHours(8)
                },
                new Booking
                {
                    CustomerID = customers[8].ID,
                    BookingDate = DateTime.Now.AddHours(9)
                },
                new Booking
                {
                    CustomerID = customers[9].ID,
                    BookingDate = DateTime.Now.AddHours(10)
                }
            };

            // Add bookings to the context
            foreach (Booking b in bookings)
            {
                context.Bookings.Add(b);
            }
            context.SaveChanges();
        }
    }
}
