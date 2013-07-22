using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Select2Demo.Extensions
{
    public static class Lists
    {
        /// <summary>
        /// Convert a list of items to a select list
        /// </summary>
        public static List<SelectListItem> ToSelectList<T>(
          this IEnumerable<T> enumerable,
          Func<T, string> text,
          Func<T, string> value,
          String defaultOption)
        {
            var items = enumerable.Select(f => new SelectListItem()
            {
                Text = text(f),
                Value = value(f)
            }).ToList();

            if (!(defaultOption == null))
            {
                items.Insert(0, new SelectListItem()
                {
                    Text = defaultOption,
                    Value = ""
                });
            }

            return items.ToList();
        }


        //From http://stackoverflow.com/questions/5663655/like-operator-in-linq-to-objects
        //For sql "Like" comparison in linq to objects
        public static bool Like(this string s, string pattern)
        {
            //Find the pattern anywhere in the string
            pattern = ".*" + pattern + ".*";
           
            return Regex.IsMatch(s, pattern, RegexOptions.IgnoreCase);
        }
    }
}