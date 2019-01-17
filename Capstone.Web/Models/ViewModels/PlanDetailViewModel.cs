using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models.ViewModels
{
    public class PlanDetailViewModel
    {
        public Plan Plan { get; set; }
        public List<List<Meal>> MealsOnDays { get; set; } = new List<List<Meal>>();
        public List<Meal> AllMeals { get; set; }
        public List<string> _days = new List<string>
        {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday"
        };
    }
}