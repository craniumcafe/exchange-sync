using Redemption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraniumCafeSync.Models
{
    class Appointment
    {
        public string EntryID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool AllDayEvent { get; set; }
        public string Subject { get; set; } 
        public string Categories { get; set; }
        public bool IsRecurring { get; set; }
        public RDORecurrencePattern RecurrencePattern { get; set; }
        public string Body { get; set; }
    }
}
