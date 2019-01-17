using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    public class Plan : Base
    {
        public int PlanId { get; set; }
        public string PlanName { get; set; }
        public int UserId { get; set; }

        public Plan Clone()
        {
            Plan plan = new Plan();
            plan.PlanId = PlanId;
            plan.PlanName = PlanName;
            plan.UserId = UserId;
            return plan;
        }
    }
}