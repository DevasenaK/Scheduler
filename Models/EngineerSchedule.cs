using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Employee_Scheduler.Models
{
    public class EngineerSchedule
    {
        public IEnumerable<Engineer> engineer { get; set; }
        public string ScheduleDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

    }
}