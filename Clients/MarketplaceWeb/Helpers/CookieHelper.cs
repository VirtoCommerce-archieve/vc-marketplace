using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MarketplaceWeb.Helpers
{
    public class CookieHelper
    {
        #region Cookie Management

        /// <summary>
        /// Sets the cookie.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="val">The val.</param>
        /// <param name="expires">The expires.</param>
        /// <param name="prefix">if set to <c>true</c> [prefix].</param>
        public static void SetCookie(string key, string val, DateTime expires, bool prefix = true)
        {
            var cookieName = prefix ? MakeStoreCookieName(key) : key;
            var httpCookie = HttpContext.Current.Request.Cookies.Get(cookieName) ?? new HttpCookie(cookieName);

            if (httpCookie.Value != val)
            {
                // Set cookie value
                httpCookie.Value = val;
                httpCookie.Expires = expires;

                HttpContext.Current.Response.Cookies.Set(httpCookie);
            }
        }

        /// <summary>
        /// Sets the cookie.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <param name="expires">The expires.</param>
        /// <param name="prefix">if set to <c>true</c> [prefix].</param>
        public static void SetCookie(string key, NameValueCollection values, DateTime expires, bool prefix = true)
        {
            var cookieName = prefix ? MakeStoreCookieName(key) : key;
            var httpCookie = HttpContext.Current.Request.Cookies.Get(cookieName) ?? new HttpCookie(cookieName);

            // Set cookie value
            httpCookie.Values.Clear();
            httpCookie.Values.Add(values);

            httpCookie.Expires = expires;
            HttpContext.Current.Response.Cookies.Set(httpCookie);
        }


        /// <summary>
        /// Gets the cookie.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="prefix">if set to <c>true</c> [prefix].</param>
        /// <returns>NameValueCollection.</returns>
        public static NameValueCollection GetCookie(string key, bool prefix = true)
        {
            var cookieName = prefix ? MakeStoreCookieName(key) : key;
            HttpCookie cookie = null;

            foreach (string cookieKey in HttpContext.Current.Response.Cookies.Keys)
            {
                if (cookieName.Equals(cookieKey, StringComparison.OrdinalIgnoreCase))
                    cookie = HttpContext.Current.Response.Cookies.Get(cookieName);
            }

            if (cookie != null)
                return cookie.Values;

            cookie = HttpContext.Current.Request.Cookies.Get(cookieName);
            return cookie != null ? cookie.Values : null;
        }

        /// <summary>
        /// Gets the cookie.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="prefix">if set to <c>true</c> [prefix].</param>
        /// <returns>System.String.</returns>
        public static string GetCookieValue(string key, bool prefix = true)
        {
            var cookieName = prefix ? MakeStoreCookieName(key) : key;
            string val = null;

            if (HttpContext.Current.Request.Cookies[cookieName] != null)
                val = HttpContext.Current.Request.Cookies[cookieName].Value;

            return val;
        }

        /// <summary>
        /// Clears the cookie.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="prefix">if set to <c>true</c> [prefix].</param>
        public static void ClearCookie(string key, string value, bool prefix = true)
        {
            var cookieName = prefix ? MakeStoreCookieName(key) : key;
            var cookie = HttpContext.Current.Request.Cookies.Get(cookieName);
            if (cookie != null)
            {
                if (String.IsNullOrEmpty(value))
                {
                    cookie.Values.Clear();
                    cookie.Expires = DateTime.Now.AddYears(-1);
                    HttpContext.Current.Response.Cookies.Set(cookie);
                }
                else
                {
                    var values = cookie.Values.GetValues(value);
                    if (values != null && values.Length > 0)
                    {
                        cookie.Values.Remove(value);
                        cookie.Expires = DateTime.Now.AddYears(-1);
                        HttpContext.Current.Response.Cookies.Set(cookie);
                    }
                }
            }
        }

        /// <summary>
        /// Makes the name of the store cookie.
        /// </summary>
        /// <param name="baseName">Name of the base.</param>
        /// <returns>System.String.</returns>
        private static string MakeStoreCookieName(string baseName)
        {
            return baseName + ConfigurationManager.AppSettings["DefaultCatalog"];
        }
        #endregion
    }
}