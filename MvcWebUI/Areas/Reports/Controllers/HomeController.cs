using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcWebUI.Areas.Reports.Models;

namespace MvcWebUI.Areas.Reports.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Reports")] // Reports
    public class HomeController : Controller
    {
        private readonly IReportService _reportService;
        private readonly ICategoryService _categoryService;
        private readonly IStoreService _storeService;

        public HomeController(IReportService reportService, ICategoryService categoryService, IStoreService storeService)
        {
            _reportService = reportService;
            _categoryService = categoryService;
            _storeService = storeService;
        }

        public IActionResult Index(HomeIndexViewModel viewModel)
        {
            viewModel.Report = _reportService.GetListLeftOuterJoin(viewModel.Filter);
            viewModel.Categories = new SelectList(_categoryService.Query().ToList(), "Id", "Name");
            viewModel.Stores = new MultiSelectList(_storeService.Query().ToList(), "Id", "Name");
            return View(viewModel);
        }
    }
}
