using BulkyBook.Data.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace BulkyBook.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        public readonly IUnitOfWork _uow;

        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork uow, IWebHostEnvironment webHostEnvironment)
        {
            _uow = uow;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var products = _uow.Product.GetAll();
            return View(products);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM
            {
                CoverTypeList = _uow.CoverType.GetAll().Select(
               u => new SelectListItem
               {
                   Text = u.Name,
                   Value = u.Id.ToString()
               }),
                CategoryList = _uow.Category.GetAll().Select(
               u => new SelectListItem
               {
                   Text = u.Name,
                   Value = u.Id.ToString()
               }),
                Product= new Product()
            };
            if (id > 0)
            {
                productVM.Product = _uow.Product.FirstOrDefault(c => c.Id == id);
            }

            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string rootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(rootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads,fileName+extension),FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products" + fileName + extension;
                }
                if (obj.Product.Id != 0)
                    _uow.Product.Update(obj.Product);
                else
                    _uow.Product.Add(obj.Product);
                _uow.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index", "Product");
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var product = _uow.Product.FirstOrDefault(c => c.Id == id);
            if (product == null)
                return NotFound();

            return View(product);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _uow.Product.Update(product);
                _uow.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index", "Category");
            }
            return View(product);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var product = _uow.Product.FirstOrDefault(c => c.Id == id);
            if (product == null)
                return NotFound();

            return View(product);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var product = _uow.Product.FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound(product);
            }
            _uow.Product.Delete(product);
            _uow.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index", "Product");
        }

        #region API

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _uow.Product.GetAll();
            return Json(products);
        }

        #endregion
    }
}
