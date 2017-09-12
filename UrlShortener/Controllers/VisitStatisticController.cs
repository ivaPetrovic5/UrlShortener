using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace UrlShortener.Controllers
{
    public class VisitStatisticController : Controller
    {
        public ActionResult Index()
        {
            if (Session["user"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public async Task<ActionResult> Index(string username)
        {
            string apiUrl = "http://localhost:62773/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string token = (string)Session["user"];
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                HttpResponseMessage response = await client.GetAsync(apiUrl + "api/statistic?accountId=" + username);
                List<string> values = new List<string>();
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    Dictionary<string, int> urlDict = JsonConvert.DeserializeObject<Dictionary<string, int>>(data);
                    ViewBag.urlDict = urlDict;
                    return View("ShowStatistics");
                }
            }
            return View();
        }
    }
}