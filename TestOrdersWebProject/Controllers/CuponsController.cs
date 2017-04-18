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
using TestOrdersWebProject.Domain.Interfaces;
using TestOrdersWebProject.Infrastructure.Interfaces.Repositories;
using TestOrdersWebProject.Services.DTO;
using TestOrdersWebProject.Services.Interfaces;
using TestOrdersWebProject.UnitOfWorks;

namespace TestOrdersWebProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CuponsController : Controller
    {
        CuponsUnitOfWork _unitOfWork;
        public CuponsController(IDbContext context, IDiscountProduct makeDiscount)
        {
            _unitOfWork = new CuponsUnitOfWork(context, makeDiscount);
        }
        // GET: Cupons
        public ActionResult Index()
        {
            var cupons = _unitOfWork.Cupons.GetWithInclude(c => c.Currency, c => c.ProductCategory);
            return View(cupons.ToList());
        }

        // GET: Cupons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cupon cupon = _unitOfWork.Cupons.GetWithInclude(x => x.Id == id, c => c.Currency, c => c.ProductCategory).Single();
            if (cupon == null)
            {
                return HttpNotFound();
            }
            return View(cupon);
        }

        // GET: Cupons/Create
        public ActionResult Create()
        {
            ViewBag.CurrencyId = new SelectList(_unitOfWork.Currencies.GetAll(), "Id", "Name");
            ViewBag.ProductCategoryId = new SelectList(_unitOfWork.ProductCategories.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Cupons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cupon cupon)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Cupons.Create(cupon);
                _unitOfWork.Cupons.Save();
                return RedirectToAction("Index");
            }

            ViewBag.CurrencyId = new SelectList(_unitOfWork.Currencies.GetAll(), "Id", "Name", cupon.CurrencyId);
            ViewBag.ProductCategoryId = new SelectList(_unitOfWork.ProductCategories.GetAll(), "Id", "Name", cupon.ProductCategoryId);
            return View(cupon);
        }

        // GET: Cupons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cupon cupon = _unitOfWork.Cupons.GetWithInclude(x => x.Id == id, c => c.Currency, c => c.ProductCategory).Single();
            if (cupon == null)
            {
                return HttpNotFound();
            }

            ViewBag.CurrencyId = new SelectList(_unitOfWork.Currencies.GetAll(), "Id", "Name", cupon.CurrencyId);
            ViewBag.ProductCategoryId = new SelectList(_unitOfWork.ProductCategories.GetAll(), "Id", "Name", cupon.ProductCategoryId);
            return View(cupon);
        }

        // POST: Cupons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cupon cupon)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Cupons.Update(cupon);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.CurrencyId = new SelectList(_unitOfWork.Currencies.GetAll(), "Id", "Name", cupon.CurrencyId);
            ViewBag.ProductCategoryId = new SelectList(_unitOfWork.ProductCategories.GetAll(), "Id", "Name", cupon.ProductCategoryId);
            return View(cupon);
        }

        // GET: Cupons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cupon cupon = _unitOfWork.Cupons.GetWithInclude(x => x.Id == id, c => c.Currency, c => c.ProductCategory).Single();
            if (cupon == null)
            {
                return HttpNotFound();
            }
            return View(cupon);
        }

        // POST: Cupons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cupon cupon = _unitOfWork.Cupons.GetWithInclude(x => x.Id == id, c => c.Currency, c => c.ProductCategory).Single();
            cupon.Currency = null;
            _unitOfWork.Cupons.Update(cupon);
            _unitOfWork.Cupons.Remove(cupon);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        // GET: Discount by cupon
        [HttpPost]
        public JsonResult ApplyCupon(List<ProductDTO> cart, int? discountCode)
        {
            cart = cart.Select(x => new ProductDTO()
            {
                Id = x.Id,
                Name = x.Name,
                CategoriesId = x.CategoriesId,
                Price = x.Price * x.Count,
                Count = x.Count,
                Discount = 0,
                CuponId = null
            }).ToList();

            if (discountCode == null)
                return Json(cart, JsonRequestBehavior.AllowGet);

            var discountedCart = _unitOfWork.MakeDiscount(cart, (int)discountCode);
            return Json(discountedCart, JsonRequestBehavior.AllowGet);    
        }
    }
}
