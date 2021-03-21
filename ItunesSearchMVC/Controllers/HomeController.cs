using ItunesSearchMVC.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ItunesSearchMVC.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            string apiUrl = "https://itunes.apple.com/search?term=jack+johnson";

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
                }
            }
            return View();
        }
    }
}