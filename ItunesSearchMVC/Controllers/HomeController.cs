using ItunesSearchMVC.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ItunesSearchMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new ItunesSearchModel();

            return View("Index", model);
        }

        [HttpPost]
        [ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ItunesSearchModel itunesSearch)
        {
            string apiUrl = "https://itunes.apple.com/search?term=jack+johnson";

            var lastreleaseDate = DateTime.Now.AddYears(-8);

            var model = new ItunesSearchModel();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var searchContent = await response.Content.ReadAsStringAsync();

                    var acutalData = Newtonsoft.Json.JsonConvert.DeserializeObject<ItunesSearchModel>(searchContent);
                    model.results = acutalData.results.Where(l => l.releaseDate >= lastreleaseDate).ToList();
                }                
            }

            return View("Index", model);
        }

        public ActionResult Create()
        {
            var model = new ItunesSearchModel();

            return View("Create", model);
        }
    }
}