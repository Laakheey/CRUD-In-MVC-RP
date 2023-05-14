using MVC_Assignment.Generic_Repository;
using MVC_Assignment.Models;
using MVC_Assignment.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_Assignment.Controllers
{
    public class AuthenticationController : Controller
    {
        private UnitOfWork<ShoppingCartContext> unitOfWork = new UnitOfWork<ShoppingCartContext>();
        private GenericRepository<User> genericRepository;
        private IUserRepository userRepository;

        public AuthenticationController()
        {
            genericRepository = new GenericRepository<User>(unitOfWork);
            userRepository = new UserRepository(unitOfWork);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new Authentication());
        }
        [HttpPost]
        public ActionResult Login(Authentication model)
        {
            if (ModelState.IsValid)
            {
                User usercheck = userRepository.GetUserByName(model.Username);
                if (usercheck != null)
                {
                    if (usercheck.Password == model.Password)
                    {
                        if (usercheck.Role.ToLower() == "admin")
                        {
                            Session["username"] = usercheck.Name;
                            FormsAuthentication.SetAuthCookie(usercheck.Name, false);
                            return RedirectToAction("Index", "Users");
                        }
                        else if (usercheck.Role.ToLower() == "user")
                        {
                            Session["Id"] = usercheck.Id;
                            Session["username"] = usercheck.Name;
                            FormsAuthentication.SetAuthCookie(usercheck.Name, false);
                            return RedirectToAction("Index", "Product");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Could not login because user does not belong to any of the roles");
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Incorrect Password");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User with the current username does not exist");
                    return View(model);
                }
            }
            ModelState.AddModelError("", "Invalid Credentials");
            return View(model);
        }
        [HttpGet]
        public ActionResult Signup()
        {
            return View(new User());
        }
        [HttpPost]
        public ActionResult Signup(User model)
        {
            if (ModelState.IsValid)
            {
                User emailcheck = userRepository.GetUserByEmail(model.Email);
                User usernamecheck = userRepository.GetUserByName(model.Name);
                if (emailcheck == null)
                {
                    if (usernamecheck == null)
                    {
                        genericRepository.Insert(model);
                        unitOfWork.Save();
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Username already in use");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email already exists");
                    return View(model);
                }
            }
            ModelState.AddModelError("", "Invalid Details");
            return View(model);
        }
        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}