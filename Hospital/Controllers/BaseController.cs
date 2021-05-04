using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Hospital.Controllers
{
    public class BaseController : Controller
    {

        /// <summary>
        /// Gets the language identifier.
        /// </summary>
        /// <value>The language identifier.</value>
        /// 
        public int LanguageID
        {
            get { return CultureHelper.CurrentLanguageCode; }
        }

        /// <summary>
        /// Get The Language from Url
        /// </summary>
        protected string LanguageCode
        {
            get { return (string)ControllerContext.RouteData.Values["LanguageCode"]; }
        }

        public string this[int index]
        {
            get { return (string)ControllerContext.RouteData.Values["LanguageCode"]; }
        }


        /// <summary>
        /// Sets the culture.
        /// </summary>
        /// <param name="lcid">The lcid.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public ActionResult SetCulture(int lcid, string returnUrl)
        {
            // Validate input
            var culture = new CultureInfo(lcid);
            var cultureName = CultureHelper.GetImplementedCulture(culture.Name);

            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            cookie = new HttpCookie("_culture");
            cookie.Value = cultureName;
            cookie.Expires = DateTime.Now.AddHours(10).AddYears(1);

            //cookie.HttpOnly = true;

            Response.Cookies.Add(cookie);
            return RedirectToLocal(returnUrl);
        }

        /// <summary>
        /// Begins to invoke the action in the current controller context.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="state">The state.</param>
        /// <returns>Returns an IAsyncController instance.</returns>
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;
            int ReportcultureName = 0;
            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            HttpCookie ReportcultureCookie = Request.Cookies["ReportLanguage"];

            if (Request.QueryString["LanguageCode"] != null)
            {
                int langID;
                if (int.TryParse(Request.QueryString["LanguageCode"].ToString(), out langID))
                {
                    cultureName = new CultureInfo(CultureHelper.MapLangaugeToLCID(langID)).Name;
                }
                else
                {
                    langID = 2;
                    cultureName = new CultureInfo(CultureHelper.MapLangaugeToLCID(langID)).Name;
                }
            }
            else if (cultureCookie != null)
            {
                cultureName = cultureCookie.Value;
            }
            else
            {
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0
                    ? Request.UserLanguages[0]
                    : // obtain it from HTTP header AcceptLanguages
                    null;
            }
            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName);

            // Modify current thread's cultures
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);// Thread.CurrentThread.CurrentCulture;

            // Set Gregorian Calendar for DateTimeFormat per current culture
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.Calendar = new GregorianCalendar(GregorianCalendarTypes.Localized);

            //Report Lange
            if (ReportcultureCookie != null)
            {
                ReportcultureName = Convert.ToInt32(ReportcultureCookie.Value);
            }
            else
            {
                ReportcultureName = Convert.ToInt32(CultureHelper.CurrentLanguageCode);
            }
            CultureHelper.ReportLanguageId = ReportcultureName;

            return base.BeginExecuteCore(callback, state);
        }


        /// <summary>
        /// Redirects to local URLs only, to avoid URL redirection attacks. If the given URL is not local,
        /// redirects to the homepage.
        /// </summary>
        /// <param name="url">The URL to redirect to if local.</param>
        /// <returns>ActionResult.</returns>
        protected ActionResult RedirectToLocal(string url)
        {
            if (Url.IsLocalUrl(url))
            {
                return Redirect(url);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}