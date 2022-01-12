using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tangy.Extensions;

namespace FysioApp.Extensions
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> items, string selectedValue = null)
        {

            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("FirstName") + " " + item.GetPropertyValue("LastName"),
                       Value = item.GetPropertyValue("Id"),
                       Selected = item.GetPropertyValue("Id").Equals(selectedValue)
                   };


        }

        public static IEnumerable<SelectListItem> ToSelectListItemDiagnose<T>(this IEnumerable<T> items, string selectedValue = null)
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("Code") + " | " + item.GetPropertyValue("BodyLocalization") + " | " + item.GetPropertyValue("Pathology"),
                       Value = item.GetPropertyValue("Code"),
                       Selected = item.GetPropertyValue("Code").Equals(selectedValue)
                   };
        }

        public static IEnumerable<SelectListItem> ToSelectListItemOperation<T>(this IEnumerable<T> items, string selectedValue = null)
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("Code") + " | " + item.GetPropertyValue("Description"),
                       Value = item.GetPropertyValue("Code"),
                       Selected = item.GetPropertyValue("Code").Equals(selectedValue)
                   };
        }
    }
}
