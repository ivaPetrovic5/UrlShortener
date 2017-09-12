using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace UrlShortener
{
    public class UrlShortenerMethod
    {
        public String GetShort(int urlId)
        {
            string possible_chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            //string short_url;
            StringBuilder string_builder = new StringBuilder();
            while (urlId > 0)
            {
                string_builder.Append(possible_chars[urlId % possible_chars.Length]);
                urlId /= possible_chars.Length;
            }
            return string_builder.ToString();
        }
    }
}