using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestOrdersWebProject.Domain.Core;
using TestOrdersWebProject.Domain.Core.Context;
using TestOrdersWebProject.Infrastructure.Interfaces.Repositories;

namespace TestOrdersWebProject.Controllers
{
    public class ProductCategoriesController : Controller
    {
        IGenericRepository<ProductCategory> _productsCategories;

        public ProductCategoriesController(IGenericRepository<ProductCategory> productsCategories)
        {
            _productsCategories = productsCategories;
        }
        // GET: ProductCategories
        public ActionResult Index()
        {
            var productCategories = _productsCategories.GetWithInclude(p => p.ParentCategory, p => p.SubCategories);
            return View(productCategories.ToList());
        }

        // GET: ProductCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = _productsCategories.Get(c => c.Id == id).Single();
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        [Authorize(Roles = "Admin")]
        // GET: ProductCategories/Create
        public ActionResult Create()
        {
            ViewBag.ParentCategoryId = new SelectList(_productsCategories.GetAll(), "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: ProductCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                _productsCategories.Create(productCategory);
                _productsCategories.Save();
                return RedirectToAction("Index");
            }

            ViewBag.ParentCategoryId = new SelectList(_productsCategories.Get(x => !x.SubCategories.Contains(x)), "Id", "Name", productCategory.ParentCategoryId);
            return View(productCategory);
        }

        [Authorize(Roles = "Admin")]
        // GET: ProductCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = _productsCategories.GetWithInclude(x => x.Id == id, p => p.ParentCategory, p => p.SubCategories).Single();
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentCategoryId = new SelectList(_productsCategories.Get(x => x.Id != productCategory.Id), "Id", "Name", productCategory.ParentCategoryId);
            return View(productCategory);
        }

        [Authorize(Roles = "Admin")]
        // POST: ProductCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                _productsCategories.Update(productCategory);
                _productsCategories.Save();
                return RedirectToAction("Index");
            }
            ViewBag.ParentCategoryId = new SelectList(_productsCategories.GetAll(), "Id", "Name", productCategory.ParentCategoryId);
            return View(productCategory);
        }

        [Authorize(Roles = "Admin")]
        // GET: ProductCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = _productsCategories.GetWithInclude(x => x.Id == id, p => p.ParentCategory).Single();
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        [Authorize(Roles = "Admin")]
        // POST: ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductCategory productCategory = _productsCategories.GetWithInclude(x => x.Id == id, p => p.ParentCategory, p => p.SubCategories).Single();
            productCategory.ParentCategory = null;
            _productsCategories.Update(productCategory);
            _productsCategories.Remove(productCategory);
            _productsCategories.Save();
            return RedirectToAction("Index");
        }

        public PartialViewResult GetCategories(int[] categories)
        {
            if (categories != null)
                ViewBag.Categories = new SelectList(_productsCategories.GetAll(), "Id", "Name", categories);
            ViewBag.Categories = new SelectList(_productsCategories.GetAll(), "Id", "Name");
            return PartialView("ListProductCategoriesPartialView");
        }
    }
}
