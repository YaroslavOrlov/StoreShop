using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestOrdersWebProject.Domain.Core;
using TestOrdersWebProject.Domain.Interfaces;
using TestOrdersWebProject.Infrastructure.Interfaces.Repositories;
using TestOrdersWebProject.Services.DTO;
using TestOrdersWebProject.Services.Interfaces;
using TestOrdersWebProject.UnitOfWorks;
using TestOrdersWebProject.ViewModels;

namespace TestOrdersWebProject.Controllers
{
    public class PurchasesController : Controller
    {
        PurchasesUnitOfWork _unitOfWork;
        public PurchasesController(IDbContext context, IPurchase makePurchase)
        {
            _unitOfWork = new PurchasesUnitOfWork(context, makePurchase);
        }

        // POST: Currencies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public JsonResult Create(PurchaseDTO purchase)
        {
            if (ModelState.IsValid)
            {
                var isSuccess = _unitOfWork.MakePurchase(purchase);
                if (isSuccess)
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, message = "Invalid purchase operation!" }, JsonRequestBehavior.AllowGet);
        }
    }
}