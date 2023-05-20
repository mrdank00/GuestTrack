using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuestTrack.Controllers
{
    public class Invoice
    {
        public string InvoiceNumber { get; set; }
        public Guest Guest { get; set; }
        public decimal RoomCharges { get; set; }
        public decimal AdditionalServicesCharges { get; set; }
        public decimal Taxes { get; set; }
        public decimal Discounts { get; set; }
        // Additional properties
    }
}
