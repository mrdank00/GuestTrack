using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuestTrack.Controllers
{
    public class Reservation
    {
        
            public string ReservationID { get; set; }
            public Guest Guest { get; set; }
            public Room Room { get; set; }
            public DateTime CheckInDate { get; set; }
            public DateTime CheckOutDate { get; set; }
            // Additional properties
       

    }
}
