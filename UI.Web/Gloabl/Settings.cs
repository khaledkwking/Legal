using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
namespace UI.Web
{
    public class Settings
    {
        private static string strCookieeLanguage = "UI_Language";
        private static string strFirstLangauge = "en-US";

        internal static CultureInfo Language
        {
            get
            {
                System.Globalization.CultureInfo CI;
                HttpCookie Cookie = HttpContext.Current.Request.Cookies.Get(strCookieeLanguage);
                if (Cookie != null)
                {
                    CI = new CultureInfo(Cookie.Value);
                }
                else
                {
                    CI = new CultureInfo(strFirstLangauge);
                    Language = CI;
                }
                return CI;
            }
            set
            {
                HttpContext.Current.Request.Cookies.Remove(strCookieeLanguage);
                HttpCookie Cookie = new HttpCookie(strCookieeLanguage, value.Name);
                Cookie.Expires = DateTime.Now.AddYears(1);
                HttpContext.Current.Response.Cookies.Add(Cookie);
            }
        }
    }
}