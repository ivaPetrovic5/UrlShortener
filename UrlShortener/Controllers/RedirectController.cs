using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    public class RedirectController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Index(string urlShort)
        {
            if (urlShort == null)
            {
                return BadRequest();
            }
            else
            {
                UrlShortenerModelContainer db_context = new UrlShortenerModelContainer();
                var username = Request.GetRequestContext().Principal.Identity.Name;
                Visit visit = new Visit();
                var queryRes = (from user in db_context.AspNetUsers
                                join url in db_context.Urls
                                on user.Id equals url.UserId
                                where url.UrlShort == urlShort && user.UserName == username
                                select new
                                {
                                    user.Id,
                                    url.UrlId,
                                    url.UrlLong
                                }).FirstOrDefault();
                visit.UrlId = queryRes.UrlId;
                visit.UserId = queryRes.Id;
                visit.VisitTime = DateTime.Now;
                db_context.Visits.Add(visit);
                db_context.SaveChanges();
                //return Redirect(queryRes.UrlLong);
                return Ok(new { longUrl = queryRes.UrlLong });
            }
        }
    }
}
