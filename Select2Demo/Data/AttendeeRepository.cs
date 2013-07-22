using Select2Demo.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Select2Demo.Extensions;
using System.Web.Caching;

namespace Select2Demo.Data
{
    public class AttendeeRepository
    {

        public IQueryable<Attendee> Attendees { get; set; }

        public AttendeeRepository()
        {
            Attendees = GenerateAttendees();
        }

        //Return only the results we want
        public List<Attendee> GetAttendees(string searchTerm, int pageSize, int pageNum)
        {
            return GetAttendeesQuery(searchTerm)              
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToList();
        }

        //And the total count of records
        public int GetAttendeesCount(string searchTerm, int pageSize, int pageNum)
        {
            return GetAttendeesQuery(searchTerm)
                .Count();
        }


        //Our search term
        private IQueryable<Attendee> GetAttendeesQuery(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return Attendees
                .Where(
                    a =>
                    a.FirstName.Like(searchTerm) ||
                    a.LastName.Like(searchTerm)
                );
        }

        //Generate test data
        private IQueryable<Attendee> GenerateAttendees()
        {
            //Check cache first before regenerating test data
            string cacheKey="attendees";
            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                return (IQueryable<Attendee>)HttpContext.Current.Cache[cacheKey];
            }

            var attendees = new List<Attendee>();
            for (int i = 0; i < 1000; i++)
            {
                attendees.Add(
                    new Attendee()
                    {
                        AttendeeId = i,
                        FirstName = "First " + i.ToString(),
                        LastName = "Last " + i.ToString()
                    }
                    );
            }

            var result = attendees.AsQueryable();

            //Cache results
            HttpContext.Current.Cache[cacheKey] = result;

            return result;
        }
    }
}