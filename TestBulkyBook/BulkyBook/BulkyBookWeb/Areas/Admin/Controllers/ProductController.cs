using BulkyBook.DataAccess;
using BulkyBook.Models;
using BulkyBook.DataAccess;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyBook.Models.ViewModels;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        //private readonly ApplicationDbContext _db;
        // private readonly ICategoryRepository _db;
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment _hostEnviornment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnviornment)
        {
            _unitofwork = unitOfWork;
            _hostEnviornment = hostEnviornment;
        }

        public IActionResult Index()
        {
            // var objCategoryList = _db.Categories.ToList();
         //   IEnumerable<Product> objProductList = _unitofwork.product.GetAll();
            return View();
        }


        [HttpGet]
        public IActionResult Upsert(int? id)
        {

            //Product product=new Product();
            //IEnumerable<SelectListItem> CategoryList = _unitofwork.Category.GetAll().Select(u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() });
            //IEnumerable<SelectListItem> CoverTypeList = _unitofwork.CoverType.GetAll().Select(u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() });

            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitofwork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),

                CoverTypeList = _unitofwork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };

            if (id == null || id == 0)
            {
              //  ViewBag.CategoryList = CategoryList;
               // ViewData["CoverTypeList"] = CoverTypeList;
                //Need to create new product
                return View(productVM);
            }
            else
            {

                //Update product
                productVM.Product = _unitofwork.product.GetFirstOrDefault(u => u.Id == id);
                return View(productVM);
            }
           

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj,IFormFile ? file)
        {

            if (ModelState.IsValid)
            {
               // _unitofwork.product.Update(obj);

                string wwwRootPath=_hostEnviornment.WebRootPath;
                if(file != null)
                {
                    string filename=Guid.NewGuid().ToString();
                    var uploads=Path.Combine(wwwRootPath,@"Images\products");
                    var extension=Path.GetExtension(file.FileName);

                    if (obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl=@"Images\Products\"+filename+extension;
                }

                if (obj.Product.Id == 0)
                { 
                _unitofwork.product.Add(obj.Product);
                }
                else
                {
                    _unitofwork.product.Update(obj.Product);
                }
                _unitofwork.Save();
                TempData["success"] = "Product Created Successfully !!";
                //return View();
                return RedirectToAction("Index");
            }
            return View(obj);

        }


        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    // var categoryFromDb = _db.Categories.Find(id);
        //    var ProductFromDbFirst = _unitofwork.product.GetFirstOrDefault(u => u.Id == id);
        //    //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

        //    if (ProductFromDbFirst == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(ProductFromDbFirst);
        //}

      
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeletePost(int? id)
        //{
        //    var obj = _unitofwork.product.GetFirstOrDefault(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }


        //    _unitofwork.product.Remove(obj);
        //    _unitofwork.Save();
        //    TempData["success"] = "Product Deleted successfully !!";
        //    //return View();
        //    return RedirectToAction("Index");

        //}
        #region API Call
        [HttpGet]
        public IActionResult GetAll()
        {
            var productlist = _unitofwork.product.GetAll(includeProperties:"Category,CoverType");
            return Json(new { data = productlist });
        }

        [HttpDelete]
       
        public IActionResult Delete(int? id)
        {
            var obj = _unitofwork.product.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new {success=false,Message="Error while deleting!!"});
            }

            var oldImagePath = Path.Combine(_hostEnviornment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitofwork.product.Remove(obj);
            _unitofwork.Save();
            //  TempData["success"] = "Product Deleted successfully !!";
            //return View();
            //return RedirectToAction("Index");
            return Json(new { success = true, Message = "Deleted successfully!!" });

        }

        #endregion


    }


}
