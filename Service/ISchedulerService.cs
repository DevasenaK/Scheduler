using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee_Scheduler.Models;

namespace Employee_Scheduler.Service
{
    public interface ISchedulerService
    {
        //Get Schedules for Next 2 weeks
        List<EngineerSchedule> GetAllEngineerSchedules();
        //Get Schedule for an Engineer
        List<EngineerSchedule> GetSchedules(int engineerID);
    }
}
