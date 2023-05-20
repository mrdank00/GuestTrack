using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuestTrack.Controllers
{
    public class Guest
    {
        public string GuestID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdPassport { get; set; }
        public string GuestAddress { get; set; }
        public string EmailAddress { get; set; }
        public string SpecialReqs { get; set; }
    }
}
