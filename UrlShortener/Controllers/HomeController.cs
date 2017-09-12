using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<ActionResult> Index(AspNetUser user)
        {
            string apiUrl = "http://localhost:62773/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new { accountId = user.UserName };
                HttpResponseMessage response = await client.PostAsJsonAsync("api/account/register", content);
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    dynamic json = JObject.Parse(result);
                    ViewBag.password = json.password;
                    if (json.success == "true")
                    {
                        ViewBag.password = json.password;
                        return View("ReturnPassword");
                    }
                    else
                    {
                        return View("UsernameTaken");
                    }
                } else
                {
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
        }
    }
}
