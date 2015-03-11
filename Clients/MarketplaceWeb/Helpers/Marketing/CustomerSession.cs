using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using VirtoCommerce.ApiClient.DataContracts;

namespace MarketplaceWeb.Helpers.Marketing
{
    public class CustomerSession
    {
        private string _categoryId;

        public CustomerSession()
        {
            //TODO store is now hardcoded
            Tags.Add(ContextFieldConstants.StoreId, "MarketPlace");
        }

        public readonly TagQuery Tags = new TagQuery();


        public string CategoryId
        {
            get
            {
                return _categoryId ?? (_categoryId = GetCookieValue("mp.CategoryId"));
            }
            set
            {
                _categoryId = value;
                SetCookie("mp.CategoryId", value);
            }
        }


        #region Customer Session
        public static CustomerSession Current
        {
            get
            {
                const string key = "mp-customersession";

                if (HttpContext.Current == null)
                {
                    var ctxThread = Thread.GetData(Thread.GetNamedDataSlot(key));
                    if (ctxThread != null)
                        return (CustomerSession)ctxThread;

                    var ctx = new CustomerSession();
                    Thread.SetData(Thread.GetNamedDataSlot(key), ctx);
                    return ctx;
                }

                // Persist in HttpContext
                if (HttpContext.Current.Items[key] == null)
                {
                    var ctx = new CustomerSession();
                    HttpContext.Current.Items.Add(key, ctx);
                }
                return (CustomerSession)HttpContext.Current.Items[key];
            }
        }
        #endregion

        #region Cookie management

        /// <summary>
        /// Sets the cookie.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="val">The value.</param>
        /// <param name="expires">The expires.</param>
        /// <param name="secure">if set to <c>true</c> [secure].</param>
        /// <param name="encrypt">if set to <c>true</c> [encrypt].</param>
        public static void SetCookie(string key, string val, DateTime? expires = null, bool secure = false, bool encrypt = false)
        {
            if (HttpContext.Current != null)
            {
                if (encrypt)
                {
                    val = EncryptCookie(val);
                }
                var responseCookie = HttpContext.Current.Response.Cookies[key];

                if (responseCookie == null)
                {
                    responseCookie = new HttpCookie(key);
                    HttpContext.Current.Response.Cookies.Add(responseCookie);
                }

                if (val != responseCookie.Value)
                {
                    if (expires.HasValue)
                    {
                        responseCookie.Expires = expires.Value;
                    }
                    responseCookie.Secure = secure;
                    responseCookie.Value = val;
                }
            }
        }


        /// <summary>
        /// Gets the cookie value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="decrypt">if set to <c>true</c> [decrypt].</param>
        /// <returns>
        /// System.String.
        /// </returns>
        public static string GetCookieValue(string key, bool decrypt = false)
        {
            string val = null;
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Request.Cookies[key] != null)
                {
                    val = HttpContext.Current.Request.Cookies[key].Value;

                    if (decrypt && !string.IsNullOrEmpty(val))
                    {
                        val = DecryptCookie(val);
                    }
                }
            }
            return val;
        }

        public static string EncryptCookie(string value)
        {
            var plainBytes = Encoding.UTF8.GetBytes(value);
            var encryptedBytes = MachineKey.Protect(plainBytes, "Cookie protection");
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string DecryptCookie(string value)
        {
            try
            {
                var encryptedBytes = Convert.FromBase64String(value);
                var decryptedBytes = MachineKey.Unprotect(encryptedBytes, "Cookie protection");
                return decryptedBytes != null ? Encoding.UTF8.GetString(decryptedBytes) : value;
            }
            catch
            {
                return value;
            }
        }

        #endregion
    }
}