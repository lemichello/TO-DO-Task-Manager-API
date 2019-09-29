using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace API.Helpers
{
    public static class CookieHelper
    {
        public static string GetCookieValue(HttpRequest request)
        {
            var cookie = request.Cookies.FirstOrDefault(i => i.Key == "taskManagerUserId");

            if (cookie.Equals(default(KeyValuePair<string, string>)))
                throw new UnauthorizedAccessException();

            return cookie.Value;
        }
    }
}