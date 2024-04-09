using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourPlace.Infrastructure.Data.Entities
{
    public class Receptionist : User
    {
        [Required]
        public int HotelID;

        [NotMapped]
        public Hotel Hotel;
        public Receptionist()
        {
            
        }
        public Receptionist(string username, string email, string firstName, string surname, int hotelID)
        {
            this.UserName = username;
            this.NormalizedUserName = username.ToUpper();
            this.Email = email;
            this.NormalizedEmail = email.ToUpper();
            this.FirstName = firstName;
            this.Surname = surname;
            this.HotelID = hotelID;
        }
    }
}
