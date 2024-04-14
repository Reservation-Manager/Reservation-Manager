using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourPlace.Infrastructure.Data.Entities
{
    public class Receptionist
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserID { get; set; }
        [Required]
        public int HotelID { get; set; }

        [NotMapped]
        public Hotel Hotel;
        public Receptionist()
        {
            
        }
        public Receptionist(string userID, int hotelID)
        {
            this.UserID = userID;
            this.HotelID = hotelID;
        }
    }
}
