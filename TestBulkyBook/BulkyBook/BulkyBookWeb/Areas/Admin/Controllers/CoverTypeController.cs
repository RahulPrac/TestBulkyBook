using BulkyBook.DataAccess;
using BulkyBook.Models;
using BulkyBook.DataAccess;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.DataAccess.Repository.IRepository;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CovertypeController : Controller
    {
        //private readonly ApplicationDbContext _db;
        // private readonly ICategoryRepository _db;
        private readonly IUnitOfWork _unitofwork;

        public CovertypeController(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }

        public IActionResult Index()
        {
            // var objCategoryList = _db.Categories.ToList();
            IEnumerable<CoverType> objCoverTypeList = _unitofwork.CoverType.GetAll();
            return View(objCoverTypeList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
           
            if (ModelState.IsValid)
            {
                _unitofwork.CoverType.Add(obj);
                _unitofwork.Save();
                TempData["success"] = "CoverType created successfully !!";
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

            var CoverTypeFromDbFirst = _unitofwork.CoverType.GetFirstOrDefault(u => u.Id == id);
           

            if (CoverTypeFromDbFirst == null)
            {
                return NotFound();
            }
            return View(CoverTypeFromDbFirst);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {

            if (ModelState.IsValid)
            {
                _unitofwork.CoverType.Update(obj);
                _unitofwork.Save();
                TempData["success"] = "CoverType Updated successfully !!";
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
            var CoverTypeFromDbFirst = _unitofwork.CoverType.GetFirstOrDefault(u => u.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

            if (CoverTypeFromDbFirst == null)
            {
                return NotFound();
            }
            return View(CoverTypeFromDbFirst);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitofwork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }


            _unitofwork.CoverType.Remove(obj);
            _unitofwork.Save();
            TempData["success"] = "CoverType Deleted successfully !!";
            //return View();
            return RedirectToAction("Index");

        }


    }



}
