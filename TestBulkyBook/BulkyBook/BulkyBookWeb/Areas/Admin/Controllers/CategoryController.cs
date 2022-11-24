using BulkyBook.DataAccess;
using BulkyBook.Models;
using BulkyBook.DataAccess;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.DataAccess.Repository.IRepository;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _db;
        // private readonly ICategoryRepository _db;
        private readonly IUnitOfWork _unitofwork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }

        public IActionResult Index()
        {
            // var objCategoryList = _db.Categories.ToList();
            IEnumerable<Category> objCategoryList = _unitofwork.Category.GetAll();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Customerror", "The display order cannot exactly match the name.");
            }

            if (ModelState.IsValid)
            {
                _unitofwork.Category.Add(obj);
                _unitofwork.Save();
                TempData["success"] = "Data created successfully !!";
                //return View();
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //var categoryFromDb = _db.Categories.Find(id);
            // var categoryFromDbFirst= _db.Categories.FirstOrDefault(u=>u.Id==id);
            var categoryFromDbFirst = _unitofwork.Category.GetFirstOrDefault(u => u.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

            if (categoryFromDbFirst == null)
            {
                return NotFound();
            }
            return View(categoryFromDbFirst);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Customerror", "The display order cannot exactly match the name.");
            }

            if (ModelState.IsValid)
            {
                _unitofwork.Category.Update(obj);
                _unitofwork.Save();
                TempData["success"] = "Data Updated successfully !!";
                //return View();
                return RedirectToAction("Index");
            }
            return View(obj);

        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // var categoryFromDb = _db.Categories.Find(id);
            var categoryFromDbFirst = _unitofwork.Category.GetFirstOrDefault(u => u.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

            if (categoryFromDbFirst == null)
            {
                return NotFound();
            }
            return View(categoryFromDbFirst);
        }

        // [HttpPost]
        // [ValidateAntiForgeryToken]
        //public IActionResult Delete(Category obj)
        //{
        //    if (obj.Name == obj.DisplayOrder.ToString())
        //    {
        //        ModelState.AddModelError("Customerror", "The display order cannot exactly match the name.");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        _db.Categories.Remove(obj);
        //        _db.SaveChanges();

        //        //return View();
        //        return RedirectToAction("Index");
        //    }
        //    return View(obj);

        //}

        //[HttpPost,ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitofwork.Category.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }


            _unitofwork.Category.Remove(obj);
            _unitofwork.Save();
            TempData["success"] = "Data Deleted successfully !!";
            //return View();
            return RedirectToAction("Index");

        }


    }



}
