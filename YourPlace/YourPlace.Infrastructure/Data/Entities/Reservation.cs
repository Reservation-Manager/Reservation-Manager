using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourPlace.Infrastructure.Data.Entities
{
    public class Reservation
    {
        [Key]
        public int ReservationID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        public DateOnly ArrivalDate { get; set; }

        [Required]
        public DateOnly LeavingDate { get; set; }

        [Required]
        public int PeopleCount { get; set; }

        [Required]
        public decimal Price { get; set; }

        [AllowNull]
        public bool Verified { get; set; } = false;

        [ForeignKey("Hotel")]
        public int HotelID { get; set; }

        [NotMapped]
        public Hotel Hotel { get; set; }

        [NotMapped]
        public List<RoomSelection> ReservedRooms { get; set; } //пазят се всички резервирани стаи за дадената резервация

        public Reservation()
        {

        }
        public Reservation(string firstName, string surname, DateOnly arrivalDate, DateOnly leavingDate, decimal price, int peopleCount, int hotelID, List<RoomSelection> reservedRooms)
        {
            FirstName = firstName;
            Surname = surname;
            ArrivalDate = arrivalDate;
            LeavingDate = leavingDate;
            Price = price;
            PeopleCount = peopleCount;
            HotelID = hotelID;
            ReservedRooms = reservedRooms;
        }
    }
}
