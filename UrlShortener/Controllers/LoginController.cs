using Newtonsoft.Json.Linq;
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
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            if(Session["user"] != null)
            {
                return RedirectToAction("Index", "UrlList");
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string accountId, string password)
        {
            string apiUrl = "http://localhost:62773/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", accountId),
                    new KeyValuePair<string, string>("password", password)
                };

                var content = new FormUrlEncodedContent(pairs);
                HttpResponseMessage response = await client.PostAsync("Token", content);
                if (!response.IsSuccessStatusCode)
                {
                    return View("WrongCredentials");
                }
                var data = response.Content.ReadAsStringAsync().Result;
                dynamic json = JObject.Parse(data);
                Session["user"] = (string)json.access_token;
            }

            return RedirectToAction("Index", "UrlList");
        }
    }
}