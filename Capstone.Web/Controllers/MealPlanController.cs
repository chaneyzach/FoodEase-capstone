using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.Models;
using Capstone.Web.Models.ViewModels;
using Capstone.Web.Database;

namespace Capstone.Web.Controllers
{
    public class MealPlanController : BaseController
    {
        private IMealDBService _dal;

        /// <summary>
        /// Getting access to DB
        /// </summary>
        /// <param name="db"></param>
        public MealPlanController(IMealDBService dal) : base(dal)
        {
            _dal = dal;
        }

        [HttpGet]
        public ActionResult GetMealPlan(int planId)
        {
            PlanDetailViewModel planDetail = new PlanDetailViewModel();

            if (IsAuthenticated)
            {
                _nextView = "MealPlanDetail";
                planDetail.Plan = _dal.GetPlanByPlanId(planId);

                for (int i = 0; i <= 6; i++)
                {
                    planDetail.MealsOnDays.Add(_dal.GetMealsOnDay(planId, i));
                }
            }

            return GetAuthenticatedView(_nextView, planDetail);
        }

        // GET: MealPlan
        [HttpGet]
        public ActionResult GetMealPlans()
        {
            List<Plan> plans = new List<Plan>();

            if (IsAuthenticated)
            {
                _nextView = "MealPlanList";
                plans.AddRange(_dal.GetAllPlansByUserId((int)Session[UserKey]));
            }

            return GetAuthenticatedView(_nextView, plans);
        }
        
        [HttpGet]
        public ActionResult AddPlan(string name, int? id)   //list of meal IDs, foreach
        {
            AddPlanViewModel addPlan = new AddPlanViewModel();
            List<Meal> mealsOnDay = new List<Meal>();

            if (id > 0)
            {
                addPlan.Plan = _dal.GetPlanByPlanId((int)id);
                addPlan.Meals = _dal.GetMealsByPlanId((int)id);
                for (int i = 0; i <= 6; i++)
                {
                    mealsOnDay = _dal.GetMealsOnDay((int)id, i);
                    addPlan.MealsOnDay.Add(mealsOnDay);
                }
                
            }

            if (Session[UserKey] != null)
            {
                addPlan.AllMeals = _dal.GetAllMealsByUserId((int)Session[UserKey]);                
            }

            bool doesExist = _dal.PlanExists(name);

            if (IsAuthenticated)
            {
                if (!doesExist)
                {
                    Plan plan = new Plan();

                    plan.PlanName = name;
                    plan.UserId = (int)Session[UserKey];
                    plan.Id = _dal.AddPlan(plan);

                    addPlan.Plan = plan;
                }
                
                _nextView = "AddMealPlan";
            }

            return GetAuthenticatedView(_nextView, addPlan);
        }

        [HttpPost]
        public ActionResult AddPlan(int planId, List<int> mealIds)
        {
            Plan plan = _dal.GetPlanByPlanId(planId);

            for (int i = 0; i <= 6; i++)
            {
                if (mealIds[i] != 0)
                {
                    _dal.AssignMealToPlan(planId, mealIds[i], i);
                }
            }

            return RedirectToAction("AddPlan", new
            {
                name = plan.PlanName,
                id = (int?)planId
            });
        }

        [HttpGet]
        public ActionResult ModifyPlan(int planId)
        {
            PlanDetailViewModel planDetail = new PlanDetailViewModel();

            if (IsAuthenticated)
            {
                _nextView = "ModifyMealPlan";
                planDetail.Plan = _dal.GetPlanByPlanId(planId);
                planDetail.AllMeals = _dal.GetAllMealsByUserId((int)Session[UserKey]);

                for (int i = 0; i <= 6; i++)
                {
                    planDetail.MealsOnDays.Add(_dal.GetMealsOnDay(planId, i));
                }
            }

            return GetAuthenticatedView(_nextView, planDetail);
        }

        [HttpPost]
        public ActionResult ModifyPlan(PlanDetailViewModel planDetail)
        {
            

            return RedirectToAction("GetMealPlan", new { planId = planDetail.Plan.PlanId });
        }
    }
}