using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel;
using TTCMS.Domain;


namespace TTCMS.Web.Extensions
{
    public static class SelectListExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItems(
              this IEnumerable<Language> lang, string selectedId = "")
        {
            var listnew = new List<SelectListItem>();
            foreach (var item in lang)
            {
                var listItem = new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id,
                    Selected = selectedId == item.Id
                };
                listnew.Add(listItem);
            }
            return listnew;
        }
        public static ICollection<SelectListItem> ToSelectListItems(
              this IEnumerable<ApplicationRole> role, string selectedId = "")
        {
            var listnew = new List<SelectListItem>();
            foreach (var item in role)
            {
                var listItem = new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id,
                    Selected = selectedId == item.Id
                };
                listnew.Add(listItem);
            }
            return listnew;
        }
        public static ICollection<SelectListItem> ToSelectListItems(int maxvalue, string selectedId = "")
        {
            var listnew = new List<SelectListItem>();
            for (int i = 1; i <= maxvalue;i++)
            {
                var listItem = new SelectListItem()
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = selectedId == i.ToString()
                };
                listnew.Add(listItem);
            }
            return listnew;
        }
        //public static IEnumerable<SelectListItem> ToSelectListItems(
        //      this IEnumerable<GoalStatus> goalStatus, int selectedId)
        //{
        //    return

        //        goalStatus.OrderBy(gs => gs.GoalStatusType)
        //              .Select(gs =>
        //                  new SelectListItem
        //                  {
        //                      Selected = (gs.GoalStatusId == selectedId),
        //                      Text = gs.GoalStatusType,
        //                      Value = gs.GoalStatusId.ToString()
        //                  });
        //}
    }
}
