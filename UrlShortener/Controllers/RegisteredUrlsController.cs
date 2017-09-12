using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    public class RegisteredUrlsController : ApiController
    {
        [Authorize]
        [HttpGet]
        public IHttpActionResult Index()
        {
            var username = Request.GetRequestContext().Principal.Identity.Name;
            UrlShortenerModelContainer db_context = new UrlShortenerModelContainer();
            List<string> urls = new List<string>();
            var obj = (from user in db_context.AspNetUsers
                       join url in db_context.Urls
                       on user.Id equals url.UserId
                       where user.UserName == username
                       select new
                       {
                           url.UrlShort
                       });

            //string userId = (from u in db_context.AspNetUsers where u.UserName == username select u.Id).Distinct().FirstOrDefault();
            //urls = db_context.Urls.Where(u => u.UserId == userId).ToList();
            return Ok(obj.ToList());
        }
    }
}
