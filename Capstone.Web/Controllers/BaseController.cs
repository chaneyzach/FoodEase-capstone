using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.Helpers;
using Capstone.Web.Models.ViewModels;
using Capstone.Web.Database;
using Capstone.Web.Models;

namespace Capstone.Web.Controllers
{
    public class BaseController : Controller
    {
        private IMealDBService _dal;
        public const string UserKey = "UserKey";
        public const string NameKey = "NameKey";
        protected string _nextView = null;

        /// <summary>
        /// Property checking if user is authenticated - used in GetAuthenticatedView method
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return Session[UserKey] != null;
            }
        }

        /// <summary>
        /// Getting access to DB
        /// </summary>
        /// <param name="db"></param>
        public BaseController(IMealDBService dal)
        {
            _dal = dal;
        }

        public ActionResult GetAuthenticatedView(string viewName, object model = null)
        {
            ActionResult result = null;
            if (IsAuthenticated)
            {
                //result = View(viewName, model);
                result = View(viewName, model);
            }
            else
            {
                result = RedirectToAction("Register", "User");
                //result = View("Register");
            }
            return result;
        }

        public ActionResult Logout()
        {
            Session[UserKey] = null;
            return RedirectToAction("Index", "Home");
        }

        public void LoginUser(string username, string password)
        {
            User user = null;

            try
            {
                user = _dal.GetUser(username);
            }
            catch (Exception)
            {
                throw new Exception("Either the username or the password is invalid.");
            }

            PasswordHelper passHelper = new PasswordHelper(password, user.Salt);
            if (!passHelper.Verify(user.Hash))
            {
                throw new Exception("Either the username or the password is invalid.");
            }

            Session[UserKey] = user.Id;
        }
    }
}