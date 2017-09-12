using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    public class StatisticController : ApiController
    {
        // POST api/values
        [Authorize]
        [HttpGet]
        public IHttpActionResult Index(string accountId)
        {
            UrlShortenerModelContainer db_context = new UrlShortenerModelContainer();

            var obj = (
                        from user in db_context.AspNetUsers
                        join url in db_context.Urls
                            on user.Id equals url.UserId
                        join visit in db_context.Visits
                            on url.UrlId equals visit.UrlId
                        where user.UserName == accountId
                        group new { user, url, visit } by new { url.UrlLong } into grp
                        select new
                        {
                            Count = grp.Count(),
                            grp.Key.UrlLong
                        }
                      );

            var resultDict = new Dictionary<string, int>();
            foreach (var result in obj)
            {
                resultDict[result.UrlLong] = result.Count;
            }

            return Ok(resultDict);
        }
    }
}
