using Ky2_2019_2020.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ky2_2019_2020.Controllers
{
    public class DiemCachLyController : Controller {
        public IActionResult ThemDiemCachLy() {
            return View();
        }

        [HttpPost]
        public string Create(DiemCachLyModel newDiemCachLy) {
            int count;
            DataContext context = HttpContext.RequestServices.GetService(typeof(Ky2_2019_2020.Models.DataContext)) as DataContext;

            count = context.sqlInsertDiemCachLy(newDiemCachLy);

            return (1 == count) ? "Insert successfully!" : "Something wrong...";
        }
    }
}