using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_Assignment.Generic_Repository;
using MVC_Assignment.Models;
using MVC_Assignment.UnitOfWork;

namespace MVC_Assignment.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UsersController : Controller
    {
        private UnitOfWork<ShoppingCartContext> unitOfWork = new UnitOfWork<ShoppingCartContext>();
        private GenericRepository<User> genericRepository;

        public UsersController()
        {
            genericRepository = new GenericRepository<User>(unitOfWork);
        }

        public ActionResult Index()
        {
            var model = genericRepository.GetAll();
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,Password,ConfirmPassword,Role")] User user)
        {
            unitOfWork.CreateTransaction();
            if (ModelState.IsValid)
            {
                genericRepository.Insert(user);
                unitOfWork.Save();
                ModelState.Clear();
                unitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(user);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = genericRepository.GetById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,Password,ConfirmPassword,Role")] User user)
        {
            unitOfWork.CreateTransaction();
            if (ModelState.IsValid)
            {
                genericRepository.Update(user);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(user);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = genericRepository.GetById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = genericRepository.GetById(id);
            genericRepository.Delete(user);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                genericRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
