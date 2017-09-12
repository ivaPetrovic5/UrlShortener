using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    public class RegisterController : ApiController
    {
        [Authorize]
        [HttpPost]
        public IHttpActionResult Index([FromBody] JObject jsonData)
        {
            UrlShortenerModelContainer db_context = new UrlShortenerModelContainer();
            dynamic json = jsonData;
            string urlLong = json.url;
            int redirectType;
            try
            {
                redirectType = json.redirectType;
            }
            catch (Exception)
            {
                redirectType = 302;
            }
            
            var header = Request.Headers.Authorization;

            if (urlLong == null)
            {
                return BadRequest();
            }

            UrlShortenerMethod urlShortener = new UrlShortenerMethod();
            int urlId = db_context.Urls.Select(a => a.UrlId).DefaultIfEmpty().Max() + 1;
            string short_url_part = "http://short.com/";
            string shortenedUrl = short_url_part + urlShortener.GetShort(urlId);
            Url url = new Url();
            url.UrlId = urlId;
            url.UrlShort = shortenedUrl;
            url.UrlLong = urlLong;
            url.RedirectType = redirectType;
            url.UserId = (from u in db_context.AspNetUsers where u.UserName == User.Identity.Name select u.Id).Distinct().FirstOrDefault();
            db_context.Urls.Add(url);
            db_context.SaveChanges();

            return Ok(new { shortUrl = shortenedUrl });
        }
    }
}
