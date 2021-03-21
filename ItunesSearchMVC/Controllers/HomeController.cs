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
            var searchString = RemoveWhitespace(itunesSearch.searchString);
            var searchType = itunesSearch.searchType;

            string baseUri = "https://itunes.apple.com/search?";

            string searchParameters = $"term={searchString}&entity={searchType}";

            string apiUrl = baseUri + searchParameters;

            var lastreleaseDate = DateTime.Now.AddYears(-8);

            var model = new ItunesSearchModel();
            model.showResultsSection = true;
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

        public string RemoveWhitespace(string input)
        {
            int j = 0, inputlen = input.Length;
            char[] newarr = new char[inputlen];

            for (int i = 0; i < inputlen; ++i)
            {
                char tmp = input[i];

                if (!char.IsWhiteSpace(tmp))
                {
                    newarr[j] = tmp;
                    ++j;
                }
            }
            return new String(newarr, 0, j);
        }
    }
}