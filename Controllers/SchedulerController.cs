using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Employee_Scheduler.Service;

namespace Employee_Scheduler.Controllers
{
    [RoutePrefix("api/EmployeeScheduler")]
    public class SchedulerController : ApiController
    {
        private readonly ISchedulerService _schedulerservice;        
        
        public SchedulerController(ISchedulerService schedulerService)
        {
            _schedulerservice = schedulerService;
        }
        // GET: api/Scheduler
        [HttpGet]
        [Route("GetSchedules")]
        public HttpResponseMessage GetAllSchedules()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _schedulerservice.GetAllEngineerSchedules());
        }

        // GET: api/Scheduler/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Scheduler
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Scheduler/5
        public void Put(int id, [FromBody]string value)
        {
        }

       
    }
}
