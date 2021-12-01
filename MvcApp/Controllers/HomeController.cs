using MvcApp.Common;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index() => View();

        [HttpPost]
        public ActionResult Scan(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                // i was too lazy to add dependency injection... this will have to work for now
                var scanner = VirusScanner.Default(ConfigurationManager.AppSettings["VirusScanner:ApiKey"]);
                // scan and nothing happens
                scanner.Scan(file.InputStream);
                // notification
                TempData.Add("ScanResult", "File scanned successfully");
            }

            return View("Index");
        }

        [HttpPost]
        public async Task<ActionResult> ScanAsync(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                // i was too lazy to add dependency injection... this will have to work for now
                var scanner = VirusScanner.Default(ConfigurationManager.AppSettings["VirusScanner:ApiKey"]);
                // works
                await scanner.ScanAsync(file.InputStream);
                // notification
                TempData.Add("ScanResult", "File scanned successfully");
            }

            return View("Index");
        }
    }
}
