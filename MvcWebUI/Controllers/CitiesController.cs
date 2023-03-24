using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace MvcWebUI.Controllers
{
    [Route("[controller]")] // ~/Cities
    public class CitiesController : Controller
    {
        private readonly ICityService _cityService;

        public CitiesController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [Route("GetList/{countryId?}")]
        public IActionResult Get(int? countryId)
        {
            var cities = _cityService.GetList(countryId.Value);
            return Json(cities);
        }
    }
}
