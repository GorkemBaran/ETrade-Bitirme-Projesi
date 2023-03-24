#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Contexts;
using DataAccess.Entities;
using Business.Services;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace MvcWebUI.Controllers
{
    public class ProductsController : Controller
    {
        // Add service injections here
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IStoreService _storeService;

		public ProductsController(IProductService productService, ICategoryService categoryService, IStoreService storeService)
		{
			_productService = productService;
			_categoryService = categoryService;
			_storeService = storeService;
		}

		// GET: Products
		public IActionResult Index()
        {
            List<ProductModel> productList = _productService.Query().ToList(); // TODO: Add get list service logic here
            return View(productList);
        }

        // GET: Products/Details/5
        //[Authorize(Roles = "Admin,User")]
        [Authorize]
        public IActionResult Details(int id)
        {
            ProductModel product = _productService.Query().SingleOrDefault(p => p.Id == id); // TODO: Add get item service logic here
            if (product == null)
            {
                //return NotFound();
                return View("_Error", "Product not found!");
            }
            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            ViewData["CategoryId"] = new SelectList(_categoryService.Query().ToList(), "Id", "Name");
            ViewBag.Stores = new MultiSelectList(_storeService.Query().ToList(), "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(ProductModel product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                bool? imageResult = UpdateImage(product, image);
                if (imageResult == false)
                {
                    ModelState.AddModelError("", $"Image upload failed! Accepted extensions {AppSettings.AcceptedImageExtensions} and maximum image size (MB) {AppSettings.AcceptedImageLength}");
                    ViewData["CategoryId"] = new SelectList(_categoryService.Query().ToList(), "Id", "Name", product.CategoryId);
                    ViewBag.Stores = new MultiSelectList(_storeService.Query().ToList(), "Id", "Name", product.StoreIds);
                    return View(product);
                }
                // TODO: Add insert service logic here
                var result = _productService.Add(product);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            ViewData["CategoryId"] = new SelectList(_categoryService.Query().ToList(), "Id", "Name", product.CategoryId);
			ViewBag.Stores = new MultiSelectList(_storeService.Query().ToList(), "Id", "Name", product.StoreIds);
			return View(product);
        }

        private bool? UpdateImage(ProductModel model, IFormFile image)
        {
            #region Validation
            bool? result = null;
            string uploadedFileName = null, uploadedFileExtension = null;
            if (image is not null && image.Length > 0)
            {
                result = false; // validasyondan geçemedi ilk değer ataması
                uploadedFileName = image.FileName; // asusrog.jpg
                uploadedFileExtension = Path.GetExtension(uploadedFileName); // .jpg
                string[] acceptedImageExtensions = AppSettings.AcceptedImageExtensions.Split(',');
                //foreach (var acceptedImageExtension in acceptedImageExtensions)
                //{
                //    if (acceptedImageExtension.ToLower().Trim() == uploadedFileExtension.ToLower())
                //    {
                //        result = true;
                //        break;
                //    }
                //}
                result = acceptedImageExtensions.Any(acceptedImageExtension => acceptedImageExtension.ToLower().Trim() == uploadedFileExtension.ToLower());
                if (result == true)
                {
                    // 1 byte = 8 bits
                    // 1 kilobyte = 1024 bytes
                    // 1 megabyte = 1024 kilobytes = 1024 * 1024 bytes = 1.048.576
                    double acceptedImageLength = AppSettings.AcceptedImageLength * Math.Pow(1024, 2); // bytes
                    if (image.Length > acceptedImageLength)
                        result = false;
                }
            }
            #endregion

            #region Kayıt
            if (result == true)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    image.CopyTo(memoryStream);
                    model.Image = memoryStream.ToArray();
                    model.ImgExtension = uploadedFileExtension;
                }
            }
            #endregion
            return result;
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            ProductModel product = _productService.Query().SingleOrDefault(p => p.Id == id); // TODO: Add get item service logic here
            if (product == null)
            {
                //return NotFound();
                return View("_Error", "Product not found!");
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            ViewData["CategoryId"] = new SelectList(_categoryService.Query().ToList(), "Id", "Name", product.CategoryId);
			ViewBag.Stores = new MultiSelectList(_storeService.Query().ToList(), "Id", "Name", product.StoreIds);
			return View(product);
        }

        // POST: Products/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(ProductModel product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
				bool? imageResult = UpdateImage(product, image);
				if (imageResult == false)
				{
					ModelState.AddModelError("", $"Image upload failed! Accepted extensions {AppSettings.AcceptedImageExtensions} and maximum image size (MB) {AppSettings.AcceptedImageLength}");
					ViewData["CategoryId"] = new SelectList(_categoryService.Query().ToList(), "Id", "Name", product.CategoryId);
					ViewBag.Stores = new MultiSelectList(_storeService.Query().ToList(), "Id", "Name", product.StoreIds);
					return View(product);
				}
				// TODO: Add update service logic here
				var result = _productService.Update(product);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            ViewData["CategoryId"] = new SelectList(_categoryService.Query().ToList(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public IActionResult Delete(int id)
        {
            if (!(User.Identity.IsAuthenticated && User.IsInRole("Admin")))
                return View("_Error", "You don't have permission to this opeartion!");

            ProductModel product = _productService.Query().SingleOrDefault(p => p.Id == id); // TODO: Add get item service logic here
            if (product == null)
            {
                //return NotFound();
                return View("_Error", "Product not found!");
            }
            return View(product);
        }

        // POST: Products/Delete
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Admin"))
                return View("_Error", "You don't have permission to this opeartion!");
            // TODO: Add delete service logic here
            _productService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteImage(int id)
        {
            _productService.DeleteImage(id);
            return RedirectToAction(nameof(Details), new { id = id });
        }
	}
}
