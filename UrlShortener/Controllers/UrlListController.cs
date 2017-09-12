using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace UrlShortener.Controllers
{
    public class UrlListController : Controller
    {
        public async Task<ActionResult> Index()
        {
            if(Session["user"] != null)
            {
                string apiUrl = "http://localhost:62773/";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string token = (string)Session["user"];
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                    var test = HttpContext.User.Identity.Name;
                    HttpResponseMessage response = await client.GetAsync(apiUrl + "api/registeredurls");
                    List<string> values = new List<string>();
                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        dynamic json = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(data);
                        foreach (Dictionary<string, string> urlDict in json)
                        {
                            foreach (KeyValuePair<string, string> kvp in urlDict)
                            {
                                values.Add(kvp.Value);
                            }
                        }
                        ViewBag.urlList = values;
                    }
                }
                return View();
            }
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public async Task<ActionResult> RedirectUrl(string urlShort)
        {
            string apiUrl = "http://localhost:62773/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string token = (string)Session["user"];
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                HttpResponseMessage response = await client.GetAsync("api/redirect?urlShort=" + urlShort);
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    dynamic json = JObject.Parse(result);
                    string longUrl = (string)json.longUrl;
                    return new RedirectResult(longUrl);
                }
            }
            return View();
        }
    }
}