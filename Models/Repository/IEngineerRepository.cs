using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee_Scheduler.Models;

namespace Employee_Scheduler.Models.Repository
{
   public interface IEngineerRepository
    {
        //Get all Enginners from XML datasource(Employee.xml)
        List<Engineer> GetAllEngineers();
        //Get a single Engineer from XML datasource based on EmployeeID
        List<Engineer> GetEngineers(int ID);
    }
}
