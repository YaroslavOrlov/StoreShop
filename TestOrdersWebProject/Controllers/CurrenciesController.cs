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
using TestOrdersWebProject.ViewModels;

namespace TestOrdersWebProject.Controllers
{
    public class CurrenciesController : Controller
    {
        IGenericRepository<Currency> _currencies;

        public CurrenciesController(IGenericRepository<Currency> currencies)
        {
            _currencies = currencies;
        }

        [HttpGet]
        // GET: Currency rate
        public JsonResult GetRate(int? id)
        {
            if (id == null)
                return Json(new { success = false, message = "Not found currency" }, JsonRequestBehavior.AllowGet);

            var rate = _currencies.Get(x => x.Id == id).Single().Rate;
            return Json(new { success = true, Rate = rate }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        // GET: Currencies
        public ActionResult Index()
        {
            return View(_currencies.GetAll());
        }

        [Authorize(Roles = "Admin")]
        // GET: Currencies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Currency currency = _currencies.FindById((int)id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        [Authorize(Roles = "Admin")]
        // GET: Currencies/Create
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: Currencies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Currency currency)
        {
            if (ModelState.IsValid)
            {
                _currencies.Create(currency);
                _currencies.Save();
                return RedirectToAction("Index");
            }

            return View(currency);
        }

        [Authorize(Roles = "Admin")]
        // GET: Currencies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Currency currency = _currencies.FindById((int)id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        [Authorize(Roles = "Admin")]
        // POST: Currencies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Currency currency)
        {
            if (ModelState.IsValid)
            {
                _currencies.Update(currency);
                _currencies.Save();
                return RedirectToAction("Index");
            }
            return View(currency);
        }

        [Authorize(Roles = "Admin")]
        // GET: Currencies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Currency currency = _currencies.FindById((int)id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        [Authorize(Roles = "Admin")]
        // POST: Currencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Currency currency = _currencies.FindById(id);
            _currencies.Remove(currency);
            _currencies.Save();
            return RedirectToAction("Index");
        }

        public PartialViewResult GetListRateCurrencies()
        {
            ViewBag.Currencies = new SelectList(_currencies.GetAll(), "Rate", "Name");
            return PartialView("ListCurrenciesRatePartialView");
        }

        public PartialViewResult GetListCurrencies(int? Id)
        {
            if (Id != null)
                ViewBag.CurrencyId = new SelectList(_currencies.GetAll(), "Id", "Name", Id);
            ViewBag.CurrencyId = new SelectList(_currencies.GetAll(), "Id", "Name");
            return PartialView("ListCurrenciesPartialView");
        }
    }
}
