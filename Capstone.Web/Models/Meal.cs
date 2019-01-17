using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    public class Meal : Base
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int CreatorId { get; set; }
        public List<string> _categories = new List<string>
        {
            "",
            "Breakfast",
            "Lunch",
            "Dinner",
            "Snack",
            "Dessert"
        };

        public Meal Clone()
        {
            Meal meal = new Meal();
            meal.Name = Name;
            meal.UserId = UserId;
            meal.CategoryId = CategoryId;
            return meal;
        }
    }
}