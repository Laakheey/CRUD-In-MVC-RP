using MVC_Assignment.DropDown_Data;
using MVC_Assignment.Generic_Repository;
using MVC_Assignment.Models;
using MVC_Assignment.UnitOfWork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MVC_Assignment.Controllers
{
    public class ProductController : Controller
    {
        private UnitOfWork<ShoppingCartContext> unitOfWork = new UnitOfWork<ShoppingCartContext>();
        private GenericRepository<Product> genericRepository;
        private ProductRepository productRepository;

        public ProductController()
        {
            genericRepository = new GenericRepository<Product>(unitOfWork);
            productRepository = new ProductRepository(unitOfWork);
        }
        [HttpGet]
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
        public ActionResult Create(Product model, HttpPostedFileBase file)
        {
            try
            {
                unitOfWork.CreateTransaction();
                if (ModelState.IsValid)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string filename = Path.GetFileName(file.FileName);
                        string _filename = DateTime.Now.ToString("yymmssfff") + filename;
                        string extension = Path.GetExtension(file.FileName);
                        string path = Path.Combine(Server.MapPath("/Content/Images/"), _filename);
                        model.Image = "/Content/Images/" + _filename;
                        file.SaveAs(path);
                    }
                    genericRepository.Insert(model);
                    unitOfWork.Save();
                    ModelState.Clear();
                    unitOfWork.Commit();
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch
            {
                unitOfWork.Rollback();
            }
            return View();
        }
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product model = genericRepository.GetById(id);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            Session["ImgPath"] = model.Image;
            return View(model);
        }
        [HttpPost]
        public ActionResult EditProduct(Product model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string _filename = DateTime.Now.ToString("yymmssfff") + filename;
                    string extension = Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("/Content/Images/"), _filename);
                    model.Image = "/Content/Images/" + _filename;
                    file.SaveAs(path);
                }
                model.Image = Session["ImgPath"].ToString();
                genericRepository.Update(model);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet, ActionName("Delete")]
        public ActionResult DeleteProduct(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product model = genericRepository.GetById(id);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Product model = genericRepository.GetById(id);
            genericRepository.Delete(model);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product model = genericRepository.GetById(id);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            Session["ImgPath"] = model.Image;
            return View(model);
        }
        [HttpGet]
        public ActionResult GetProductByCategory(string category)
        {
            var product = productRepository.GetProductByCategory(category);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

    }
}