using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.Helpers;
using Capstone.Web.Models.ViewModels;
using Capstone.Web.Models;
using Capstone.Web.Database;

namespace Capstone.Web.Controllers
{
    public class UserController : BaseController
    {
        private IMealDBService _dal;

        public UserController(IMealDBService dal) : base(dal)
        {
            _dal = dal;
        }

        // GET: User
        [HttpGet]
        public ActionResult Login()
        {
            ActionResult result = null;
            _nextView = "Login";

            if (IsAuthenticated)
            {
                result = RedirectToAction("Index", "Home");
            }
            else
            {
                result = View(_nextView);
            }

            return result;
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            ActionResult result = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception();
                }

                User user = null;
                try
                {
                    user = _dal.GetUser(model.Username);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("invalid-user", "Either the username or the password is invalid.");
                    throw;
                }

                PasswordHelper passHelper = new PasswordHelper(model.Password, user.Salt);
                if (!passHelper.Verify(user.Hash))
                {
                    ModelState.AddModelError("invalid-user", "Either the username or the password is invalid.");
                    throw new Exception();
                }

                Session[UserKey] = user.Id;
                Session[NameKey] = user.FirstName;
                //go to home page if login works
                result = RedirectToAction("GetRecipes", "Recipe");
            }
            catch (Exception)
            {
                result = View("Login");
            }

            return result;
        }

        [HttpGet]
        public ActionResult Register()
        {
            ActionResult result = null;
            _nextView = "Register";

            if (IsAuthenticated)
            {

                result = RedirectToAction("Index", "Home");
            }
            else
            {
                result = View(_nextView);
            }
            return result;
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            ActionResult result = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception();
                }

                User user = null;
                try
                {
                    user = _dal.GetUser(model.Username);
                }
                catch (Exception)
                {
                }

                if (user != null)
                {
                    ModelState.AddModelError("invalid-user", "The username is already taken.");
                    throw new Exception();
                }

                PasswordHelper passHelper = new PasswordHelper(model.Password);
                User newUser = new User()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Username = model.Username,
                    Salt = passHelper.Salt,
                    Hash = passHelper.Hash
                };

                _dal.AddUser(newUser);
                //base.LogUserIn(newUser);
                LoginUser(newUser.Username, model.Password);
                Session[NameKey] = newUser.FirstName;
                //send to recipe list when user logs in

                result = RedirectToAction("GetRecipes", "Recipe");

            }
            catch (Exception)
            {
                result = View("Register");
            }

            return result;
        }
    }
}