using BulkyBook.Data.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Controllers
{
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var coverTypes = _unitOfWork.CoverType.GetAll();
            return View(coverTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CoverType item)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(item);
                _unitOfWork.Save();
                TempData["success"] = "CoverType created successfully";
                return RedirectToAction("Index");
            }
            return View(item);

        }
        public IActionResult Update(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var coverType = _unitOfWork.CoverType.FirstOrDefault(c => c.Id == id);
            if (coverType == null)
                return NotFound();
            return View(coverType);
        }

        [HttpPost]
        public IActionResult Update(CoverType item)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(item);
                _unitOfWork.Save();
                TempData["success"] = "CoverType updated successfully";
                return RedirectToAction("Index");
            }
            return View(item);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var coverType = _unitOfWork.CoverType.FirstOrDefault(c => c.Id == id);
            if (coverType == null)
                return NotFound();
            return View(coverType);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {

            if (id == null || id == 0)
                return NotFound();
            var coverType = _unitOfWork.CoverType.FirstOrDefault(c => c.Id == id);
            if (coverType == null)
                return NotFound();
            _unitOfWork.CoverType.Delete(coverType);
            _unitOfWork.Save();
            TempData["success"] = "CoverType deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
