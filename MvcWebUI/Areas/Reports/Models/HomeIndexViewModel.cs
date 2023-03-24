#nullable disable

using Business.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcWebUI.Areas.Reports.Models
{
    public class HomeIndexViewModel
    {
        public List<ReportModel> Report { get; set; }
        public ReportFilterModel Filter { get; set; }
        public SelectList Categories { get; set; }
        public MultiSelectList Stores { get; set; }
    }
}
