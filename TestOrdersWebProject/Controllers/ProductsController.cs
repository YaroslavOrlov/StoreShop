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
using TestOrdersWebProject.UnitOfWorks;
using TestOrdersWebProject.ViewModels;

namespace TestOrdersWebProject.Controllers
{
    public class ProductsController : Controller
    {
        ProductsUnitOfWork _unitOfWork;
        public ProductsController(IDbContext context)
        {
            _unitOfWork = new ProductsUnitOfWork(context);
        }

        // GET: Products
        public ActionResult Index()
        {
            return View();
        }

        // GET: Products
        public JsonResult GetAll()
        {
            return Json(_unitOfWork.Products.GetWithInclude(x => x.Categories)
            .Select(x => new ProductDTO()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                CategoriesId = x.Categories.Select(y => y.Id).ToArray(),
                Discount = 0,
                Count = 1} 
            ), JsonRequestBehavior.AllowGet);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _unitOfWork.Products.GetWithInclude(x => x.Id == id, y => y.Characteristics, y => y.Categories).Single();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product()
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    Price = viewModel.Price,
                    Characteristics = new List<ProductCharacteristic>(),
                    Categories = new List<ProductCategory>()
                };

                ApplyCollectionsToProduct(viewModel, product);

                _unitOfWork.Products.Create(product);
                _unitOfWork.Products.Save();
                return RedirectToAction("Index");
            }
            
            return View(viewModel);    
        }

        [Authorize(Roles = "Admin")]
        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = _unitOfWork.Products.FindById((int)id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ProductViewModel viewModel = new ProductViewModel()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Characteristics = product.Characteristics.Select(x => x.Id).ToArray(),
                Categories = product.Categories.Select(x => x.Id).ToArray()
            };
            
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Product product = _unitOfWork.Products.GetWithInclude(x => x.Id == viewModel.Id, y => y.Characteristics).Single();
                product.Name = viewModel.Name;
                product.Description = viewModel.Description;
                product.Price = viewModel.Price;

                if(viewModel.Characteristics != null || viewModel.CharacteristicsGroupId != null)
                    product.Characteristics.Clear();

                if (viewModel.Categories != null)
                    product.Categories.Clear();

                ApplyCollectionsToProduct(viewModel, product);

                _unitOfWork.Products.Update(x => x.Id == product.Id,
                    product.Characteristics, 
                    _unitOfWork.ProductsCharacteristics.GetAll(), 
                    nameof(product.Characteristics));
                _unitOfWork.Products.Update(x => x.Id == product.Id,
                   product.Categories,
                   _unitOfWork.ProductCategories.GetAll(),
                   nameof(product.Categories));
                _unitOfWork.Save();
            
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _unitOfWork.Products.FindById((int)id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = _unitOfWork.Products.FindById((int)id);
            _unitOfWork.Products.Remove(product);
            _unitOfWork.Products.Save();
            return RedirectToAction("Index");
        }

        private void ApplyCollectionsToProduct(ProductViewModel viewModel, Product product)
        {
            if (viewModel.Characteristics != null)
            {
                foreach (var characteristic in _unitOfWork.ProductsCharacteristics.Get(x => viewModel.Characteristics.Contains(x.Id)))
                {
                    product.Characteristics.Add(characteristic);
                }
            }

            if (viewModel.Categories != null)
            {
                foreach (var category in _unitOfWork.ProductCategories.Get(x => viewModel.Categories.Contains(x.Id)))
                {
                    product.Categories.Add(category);
                }
            }

            if (viewModel.CharacteristicsGroupId != null)
            {
                foreach (var characteristic in _unitOfWork.ProductsCharacteristics.Get(x => x.CharacteristicsGroupId == viewModel.CharacteristicsGroupId))
                {
                    product.Characteristics.Add(characteristic);
                }
            }

        }
    }
}
