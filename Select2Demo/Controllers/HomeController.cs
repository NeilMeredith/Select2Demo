using Select2Demo.Data;
using Select2Demo.Helpers;
using Select2Demo.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Select2Demo.Controllers
{
    public class HomeController : Controller
    {
     
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Redirect to the confirmation page with the selected attendee id
        /// </summary>
        [HttpPost]        
        public ActionResult Index(IndexVm vm)
        {
            return RedirectToAction("Confirmation", new { @id = vm.AttendeeId });
        }

        /// <summary>
        /// Display the selected attendee id
        /// </summary>
        [HttpGet]
        public ActionResult Confirmation(int id)
        {
            ConfirmationVm vm = new ConfirmationVm();
            vm.AttendeeId = id;
            return View(vm);
        }

        /// <summary>
        /// The method the ajax select2 query hits to get the attendees to display in the dropdownlist
        /// </summary>
        [HttpGet]
        public ActionResult GetAttendees(string searchTerm, int pageSize, int pageNum)
        {
            //Get the paged results and the total count of the results for this query. 
            AttendeeRepository ar = new AttendeeRepository();
            List<Attendee> attendees = ar.GetAttendees(searchTerm, pageSize, pageNum);
            int attendeeCount = ar.GetAttendeesCount(searchTerm, pageSize, pageNum);

            //Translate the attendees into a format the select2 dropdown expects
            Select2PagedResult pagedAttendees = AttendeesToSelect2Format(attendees, attendeeCount);       

            //Return the data as a jsonp result
            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private Select2PagedResult AttendeesToSelect2Format(List<Attendee> attendees, int totalAttendees)
        {
            Select2PagedResult jsonAttendees = new Select2PagedResult();
            jsonAttendees.Results = new List<Select2Result>();

            //Loop through our attendees and translate it into a text value and an id for the select list
            foreach (Attendee a in attendees)
            {
                jsonAttendees.Results.Add(new Select2Result { id = a.AttendeeId.ToString(), text = a.FirstName + " " + a.LastName });
            }
            //Set the total count of the results from the query.
            jsonAttendees.Total = totalAttendees;

            return jsonAttendees;
        }
    }



    //Extra classes to format the results the way the select2 dropdown wants them
    public class Select2PagedResult
    {
        public int Total { get; set; }
        public List<Select2Result> Results { get; set; }
    }

    public class Select2Result
    {
        public string id { get; set; }
        public string text { get; set; }
    }
}
