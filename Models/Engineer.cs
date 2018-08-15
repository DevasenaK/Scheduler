using System;
using System.ComponentModel;

namespace Employee_Scheduler.Models
{
    public class Engineer
    {
        public int ID { get; set; }
        public String Name { get; set; }
        [DefaultValue(true)]
        public bool IsAvailable { get; set; }
    }
}