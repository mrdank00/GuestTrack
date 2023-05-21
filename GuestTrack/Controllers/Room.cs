using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuestTrack.Controllers
{
    public class Room
    {
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public decimal RoomRate { get; set; }
        public bool IsAvailable { get; set; }
    }
}
