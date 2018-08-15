using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Employee_Scheduler.Models;
using Employee_Scheduler.Models.Repository;
using System.Threading;


namespace Employee_Scheduler.Service
{
    public class SchedulerService : ISchedulerService
    {
        private IEngineerRepository _repository;
        //Config settings
        private readonly int scheduleSpan = 2;
        private readonly int shiftDays = 10;


        public SchedulerService(IEngineerRepository repository)
        {
            _repository = repository;
        }
        
        public List<EngineerSchedule> GetAllEngineerSchedules()
        {

            List<Engineer> engineers = _repository.GetAllEngineers().Where(e => e.IsAvailable == true).ToList();
            List<Engineer> initialList = _repository.GetAllEngineers().ToList();
            List<EngineerSchedule> previousweek = null;

            if (engineers.Count < (shiftDays / 2))
            {
                throw new InvalidOperationException("There are no enough engineers to generate schedule,adhering the rules");
            }
            List<EngineerSchedule> engineerSchedules = new List<EngineerSchedule>();
            DateTime currentDate = System.DateTime.Now;
            //Schedule starts from next monday
            int diffDays = ((int)DayOfWeek.Monday - (int)currentDate.DayOfWeek + 7) % 7;

            DateTime scheduleStartdate = currentDate.AddDays(diffDays);
            //Schedule spans for 2weeks 12 days(Monday to friday)
            DateTime scheduleEnddate = scheduleStartdate.AddDays(12);
            DateTime currentScheduleStart = scheduleStartdate;

            for (int j = 0; j < scheduleSpan; j++)
            {
                if (j > 0)
                {
                    //To ensure that we are not getting the same random numbers in each week
                    Thread.Sleep(100);
                }
                var rand = new Random();
                var randomEngineers = engineers.OrderBy(item => rand.Next()).Where(e => e.IsAvailable == true).ToList();
                if (randomEngineers.Count < shiftDays)
                {
                    int noOfengShortage = shiftDays - randomEngineers.Count;
                    //To make sure randomEngineers picked for 2nd week hasnt completed 1 day of support already in previous week
                    if (j > 0 && previousweek.Count > 0)
                    {
                        List<Engineer> assignedEngineers = new List<Engineer>();
                        foreach (EngineerSchedule es in previousweek)
                        {
                            foreach (Engineer eng in es.engineer)
                            {
                                assignedEngineers.Add(eng);
                            }
                        }


                        //To avoid duplicates in 2nd week,and make sure engineers opt out in last week completes 1 whole day of support
                        foreach (Engineer e in engineers)
                        {
                            bool isShiftcompletedPrevweek = assignedEngineers.Any(emp => emp.ID == e.ID);
                            bool isRandom = randomEngineers.Any(emp => emp.ID == e.ID);

                            if (isShiftcompletedPrevweek == true && isRandom == true)
                                e.IsAvailable = false;
                        }
                    }
                    //Shuffle the available Engineers list again,so that we get enough engineers to work for the existing week
                    var randomEngineers1 = engineers.OrderBy(item => rand.Next()).Where(e => e.IsAvailable == true).ToList().Take(noOfengShortage).ToList();
                    //Add it to the original randomEngineers list
                    foreach (Engineer e in randomEngineers1)
                    {
                        bool isOvertime = false;

                        if (isOvertime == false)
                            randomEngineers.Add(e);
                    }

                    if (j == 0)
                        //Set IsAvailable=false for engineers picked in 2nd shuffle,if they have completed 1 day of support
                        engineers = UpdateAvailableEngineersPool(randomEngineers, initialList);

                }
                if (j > 0 && engineerSchedules.Count > 0)
                {
                    currentScheduleStart = Convert.ToDateTime(engineerSchedules[engineerSchedules.Count - 1].ScheduleDate);
                    if (currentScheduleStart.DayOfWeek == DayOfWeek.Friday)
                    {
                        int diffDay = ((int)DayOfWeek.Monday - (int)currentScheduleStart.DayOfWeek + 7) % 7;

                        currentScheduleStart = currentScheduleStart.AddDays(diffDay);
                    }
                }
                engineerSchedules = GenerateSschedulePerWeek(currentScheduleStart, scheduleStartdate, scheduleEnddate, randomEngineers, engineerSchedules, previousweek);
                previousweek = engineerSchedules;
            }

            return engineerSchedules;

        }

        private List<Engineer> UpdateAvailableEngineersPool(List<Engineer> randomEngineers, List<Engineer> initialList)
        {
            var toUpdateList = randomEngineers.GroupBy(s => s)
           .SelectMany(grp => grp.Skip(1)).ToList();
            //To suffice the rule"Each engineer should have completed one whole day of support in any 2 week period.", 
            //Enginners unavailabe in current week should be made available for next week
            foreach (Engineer e in initialList)
            {
                if (e.IsAvailable == false)
                    e.IsAvailable = true;
            }
            //Make Overtime employees in currentweek as unavailable in second week,to suffice afore said rule
            foreach (Engineer e in initialList)
            {
                bool isShiftcompleted = toUpdateList.Any(emp => emp.ID == e.ID);
                if (isShiftcompleted == true)
                    e.IsAvailable = false;

            }
            return initialList;

        }

        private List<EngineerSchedule> GenerateSschedulePerWeek(DateTime currentscheduleStart, DateTime scheduleStart, DateTime scheduleEnd, List<Engineer> randomEngineers, List<EngineerSchedule> engineerSchedules, List<EngineerSchedule> previousWeek)
        {
            DateTime scheduleDate = currentscheduleStart;
            List<Engineer> previousDayshift = new List<Engineer>();
            //An engineer can do at most one-half day shift in a day.
            for (int i = 0; i < randomEngineers.Count; i++)
            {
                if (i < randomEngineers.Count - 1)
                {

                    if (randomEngineers[i] == randomEngineers[i + 1])
                    {
                        if (i + 2 < randomEngineers.Count - 1)
                        {
                            //swap if consecutive engineers in same shift
                            Engineer temp = randomEngineers[i + 1];
                            randomEngineers[i + 1] = randomEngineers[i + 2];
                            randomEngineers[i + 2] = temp;
                        }

                    }
                }
            }
            for (int i = 0; i < randomEngineers.Count; i = i + 2)
            {
                if (scheduleDate < scheduleEnd)
                {
                    EngineerSchedule engineerSchedule = new EngineerSchedule();
                    List<Engineer> eng = new List<Engineer>();
                    if (previousDayshift.Count > 0)
                    {
                        //An engineer cannot have two afternoon shifts on consecutive days. 
                        if (randomEngineers[i + 1] == previousDayshift[1])
                        {
                            eng.Add(randomEngineers[i + 1]);
                            eng.Add(randomEngineers[i]);
                        }
                        else
                        {
                            eng.Add(randomEngineers[i]);
                            eng.Add(randomEngineers[i + 1]);
                        }
                    }
                    else
                    {
                        eng.Add(randomEngineers[i]);
                        eng.Add(randomEngineers[i + 1]);
                    }
                    //Store the previous day engineers in a list
                    previousDayshift = eng;

                    //Map to EngineerSchedule
                    engineerSchedule.engineer = eng;

                    if (i == 0)
                        engineerSchedule.ScheduleDate = scheduleDate.ToShortDateString();
                    else
                    {
                        scheduleDate = scheduleDate.AddDays(1);
                        engineerSchedule.ScheduleDate = scheduleDate.ToShortDateString();

                    }
                    engineerSchedule.StartDate = scheduleStart.ToShortDateString();
                    engineerSchedule.EndDate = scheduleEnd.ToShortDateString();
                    engineerSchedules.Add(engineerSchedule);

                }
            }
            return engineerSchedules;

        }
        public List<EngineerSchedule> GetSchedules(int engineerID)
        {
            throw new NotImplementedException();
        }
    }
}