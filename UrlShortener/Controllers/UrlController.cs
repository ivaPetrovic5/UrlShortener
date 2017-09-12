using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    public class UrlController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(Url url)
        {
            string apiUrl = "http://localhost:62773/";
            using (var client = new HttpClient())
            {
                string authenticationToken = Thread.CurrentPrincipal.Identity.Name;
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string token = (string)Session["user"];
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                var content = new { url = url.UrlLong, redirectType = url.RedirectType };
                HttpResponseMessage response = await client.PostAsJsonAsync("api/register", content);
                if (!response.IsSuccessStatusCode)
                {
                    //var data = response.Content.ReadAsStringAsync().Result;
                    return View();
                }
            }
            return RedirectToAction("Index", "UrlList");
        }
    }
}