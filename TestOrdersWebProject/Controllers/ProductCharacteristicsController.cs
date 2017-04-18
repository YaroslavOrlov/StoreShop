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
using TestOrdersWebProject.UnitOfWorks;

namespace TestOrdersWebProject.Controllers
{
    public class ProductCharacteristicsController : Controller
    {
        ProductCharacteristicsUnitOfWork _unitOfWork;
        public ProductCharacteristicsController(IDbContext context)
        {
            _unitOfWork = new ProductCharacteristicsUnitOfWork(context);
        }

        [Authorize(Roles = "Admin")]
        // GET: ProductCharacteristics
        public ActionResult Index()
        {
            var productCharacteristics = _unitOfWork.Characteristics.GetWithInclude(x => x.CharacteristicsGroup);
            return View(productCharacteristics.ToList());
        }

        [Authorize(Roles = "Admin")]
        // GET: ProductCharacteristics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCharacteristic productCharacteristic = _unitOfWork.Characteristics.GetWithInclude(x => x.Id == id, y => y.CharacteristicsGroup).Single();
            if (productCharacteristic == null)
            {
                return HttpNotFound();
            }
            return View(productCharacteristic);
        }

        [Authorize(Roles = "Admin")]
        // GET: ProductCharacteristics/Create
        public ActionResult Create()
        {
            ViewBag.CharacteristicsGroupId = new SelectList(_unitOfWork.CharacteristicsGroup.GetAll(), "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: ProductCharacteristics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductCharacteristic productCharacteristic)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Characteristics.Create(productCharacteristic);
                _unitOfWork.Characteristics.Save();
                return RedirectToAction("Index");
            }

            ViewBag.CharacteristicsGroupId = new SelectList(_unitOfWork.CharacteristicsGroup.GetAll(), "Id", "Name", productCharacteristic.CharacteristicsGroupId);
            return View(productCharacteristic);
        }

        // GET: ProductCharacteristics/Create
        public ActionResult CreateGroup()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGroup(CharacteristicsGroup characteristicsGroup)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CharacteristicsGroup.Create(characteristicsGroup);
                _unitOfWork.CharacteristicsGroup.Save();
                return RedirectToAction("Index");
            }

            return View(characteristicsGroup);
        }

        [Authorize(Roles = "Admin")]
        // GET: ProductCharacteristics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCharacteristic productCharacteristic = _unitOfWork.Characteristics.FindById((int)id);
            if (productCharacteristic == null)
            {
                return HttpNotFound();
            }

            ViewBag.CharacteristicsGroupId = new SelectList(_unitOfWork.CharacteristicsGroup.GetAll(), "Id", "Name", productCharacteristic.CharacteristicsGroupId);
            return View(productCharacteristic);
        }

        [Authorize(Roles = "Admin")]
        // POST: ProductCharacteristics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCharacteristic productCharacteristic)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Characteristics.Update(productCharacteristic);
                _unitOfWork.Characteristics.Save();
                return RedirectToAction("Index");
            }
            ViewBag.CharacteristicsGroupId = new SelectList(_unitOfWork.CharacteristicsGroup.GetAll(), "Id", "Name", productCharacteristic.CharacteristicsGroupId);
            return View(productCharacteristic);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CharacteristicsGroup characteristicGroup = _unitOfWork.CharacteristicsGroup.FindById((int)id);
            if (characteristicGroup == null)
            {
                return HttpNotFound();
            }

            return View(characteristicGroup);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGroup(CharacteristicsGroup characteristicGroup)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CharacteristicsGroup.Update(characteristicGroup);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(characteristicGroup);
        }

        [Authorize(Roles = "Admin")]
        // GET: ProductCharacteristics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCharacteristic productCharacteristic = _unitOfWork.Characteristics.FindById((int)id);
            if (productCharacteristic == null)
            {
                return HttpNotFound();
            }

            return View(productCharacteristic);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CharacteristicsGroup characteristicsGroup = _unitOfWork.CharacteristicsGroup.FindById((int)id);
            if (characteristicsGroup == null)
            {
                return HttpNotFound();
            }

            return View(characteristicsGroup);
        }

        [Authorize(Roles = "Admin")]
        // POST: ProductCharacteristics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductCharacteristic productCharacteristic = _unitOfWork.Characteristics.FindById((int)id);
            _unitOfWork.Characteristics.Remove(productCharacteristic);
            _unitOfWork.Characteristics.Save();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("DeleteGroup")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGroupConfirmed(int id)
        {
            CharacteristicsGroup characteristicsGroup = _unitOfWork.CharacteristicsGroup.FindById(id);
            characteristicsGroup.ProductCharacteristics.Clear();
            _unitOfWork.CharacteristicsGroup.Update(characteristicsGroup);
            _unitOfWork.CharacteristicsGroup.Remove(characteristicsGroup);
            _unitOfWork.CharacteristicsGroup.Save();
            return RedirectToAction("Index");
        }

        public PartialViewResult GetCharacteristicsGroup(int? id)
        {
            if(id != null)
                ViewBag.CharacteristicsGroupId = new SelectList(_unitOfWork.CharacteristicsGroup.GetAll(), "Id", "Name", id);
            ViewBag.CharacteristicsGroupId = new SelectList(_unitOfWork.CharacteristicsGroup.GetAll(), "Id", "Name");
            return PartialView("ListGroupCharacteristicsPartialView");
        }

        public PartialViewResult GetCharacteristics(int? id)
        {
            if (id != null)
                ViewBag.Characteristics = new SelectList(_unitOfWork.Characteristics.GetAll(), "Id", "Name", id);
            ViewBag.Characteristics = new SelectList(_unitOfWork.Characteristics.GetAll(), "Id", "Name");
            return PartialView("ListCharacteristicsPartialView");
        }
    }
}
