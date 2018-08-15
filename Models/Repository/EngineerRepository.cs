using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Xml.Linq;
using System.Web;

namespace Employee_Scheduler.Models.Repository
{
    public class EngineerRepository : IEngineerRepository
    {
        List<Engineer> IEngineerRepository.GetAllEngineers()
        {
            List<Engineer> engineers = new List<Engineer>();
            //Read engineer data from xml file and add it to the list

            XDocument doc = XDocument.Load(HttpContext.Current.Server.MapPath("~/App_Data/Employee.xml"));

            foreach (XElement element in doc.Descendants("employee"))

            {

                Engineer engineer = new Engineer();

                engineer.ID =Convert.ToInt32(element.Element("Id").Value.ToString());

                engineer.Name = element.Element("Name").Value;

                engineer.IsAvailable = true;
                engineers.Add(engineer);

            }
            //testing

            //engineers[5].IsAvailable = false;

            //engineers[6].IsAvailable = false;

            //engineers[7].IsAvailable = false;

            engineers[8].IsAvailable = false;

            engineers[9].IsAvailable = false;

            return engineers;



        }

        List<Engineer> IEngineerRepository.GetEngineers(int ID)
        {
            throw new NotImplementedException();
        }
    }
}