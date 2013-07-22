using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Select2Demo.Extensions;

namespace Select2Demo.Models.Home
{
    public class IndexVm
    {     
        [Required(ErrorMessage = "Please select an attendee")]
        public string AttendeeId { get; set; }
     
    }
}