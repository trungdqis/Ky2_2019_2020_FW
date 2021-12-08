using Ky2_2019_2020.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Ky2_2019_2020.Controllers
{
    public class CongNhanController : Controller {
        public IActionResult ListByTrieuChungForm() {
            return View();
        }

        [HttpPost]
        public IActionResult ListByTrieuChung(int soTc) {
            DataContext context = HttpContext.RequestServices.GetService(typeof(Ky2_2019_2020.Models.DataContext)) as DataContext;

            return View(context.sqlListByTrieuChungCongNhan(soTc));
        }

        public IActionResult ListByDiemCachLyForm() {
            DataContext context = HttpContext.RequestServices.GetService(typeof(Ky2_2019_2020.Models.DataContext)) as DataContext;

            return View(context.sqlListDiemCachLy());
        }

        [HttpPost]
        public IActionResult ListByDiemCachLy(string maDiemCachLy) {
            DataContext context = HttpContext.RequestServices.GetService(typeof(Ky2_2019_2020.Models.DataContext)) as DataContext;

            return View(context.sqlListByDiemCachLyCongNhan(maDiemCachLy));
        }

        public IActionResult Details(string maCongNhan) {
            DataContext context = HttpContext.RequestServices.GetService(typeof(Ky2_2019_2020.Models.DataContext)) as DataContext;

            return View(context.sqlFindCongNhanByMaCN(maCongNhan));
        }

        public string Delete(string maCongNhan) {
            int count;
            DataContext context = HttpContext.RequestServices.GetService(typeof(Ky2_2019_2020.Models.DataContext)) as DataContext;

            count = context.sqlDeleteCongNhanByMaCN(maCongNhan);

            return (1 == count) ? "Delete successfully!" : "Something wrong...";
        }
    }
}