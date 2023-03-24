using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace MvcWebUI.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoriesViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public ViewViewComponentResult Invoke()
        {
            List<CategoryModel> categories;

            //categories = _categoryService.Query().ToList(); // *1

            //Task<List<CategoryModel>> task = _categoryService.Query().ToListAsync(); // *2
            //categories = task.Result;

            var task = _categoryService.GetListAsync();
            categories = task.Result;
            return View(categories);
        }
    }
}
