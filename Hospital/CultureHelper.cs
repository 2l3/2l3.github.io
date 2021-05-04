using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace Hospital
{
    public static class CultureHelper
    {
        private static int UserId;
        public static int ReportLanguageId;
        private static string UserName;
        public static int getUserId()
        {
            UserId = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserId());
            return UserId;

        }

        public static string GetUserName()
        {
            UserName = HttpContext.Current.User.Identity.GetUserName();
            return UserName;
        }

        private enum LanguageLCID
        {
            Arabic = 1,
            English = 2,
        }


        private static readonly List<string> _supportedCultures = new List<string>
        {
            "en-US", // first culture is the DEFAULT
            "ar", // Arabic neutral culture
        };

        /// <summary>
        /// Gets a value indicating whether returns true if the language is a right-to-left language. Otherwise, false.
        /// </summary>
        public static bool IsRighToLeft
        {
            get { return System.Threading.Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft; }
        }

        public static string CurrentCultureHtmlDirection
        {
            get { return IsRighToLeft ? "rtl" : "ltr"; }
        }

        public enum KnownLanguages
        {
            Arabic = 1,
            English = 2,
        }

        public static int CurrentLanguageCode
        {
            get
            {
                if (System.Threading.Thread.CurrentThread.CurrentUICulture.LCID == 1)
                    return (int)KnownLanguages.Arabic;
                else
                    return (int)KnownLanguages.English;
            }
        }

        public static List<string> SupportedCultures
        {
            get { return _supportedCultures; }
        }

        /// <summary>
        /// Returns a valid culture name based on "name" parameter. If "name" is not valid, it returns the default culture "en-US"
        /// </summary>
        /// <param name="name" />Culture's name (e.g. en-US)
        /// <returns></returns></param>
        public static string GetImplementedCulture(string name)
        {
            // make sure it's not null
            if (string.IsNullOrEmpty(name))
            {
                return DefaultCulture; // return Default culture
            }

            // if it is implemented, accept it
            if (name == "en-US" || name == "bg")
            {
                return name; // accept it
            }

            // Find a close match. For example, if you have "en-US" defined and the user requests "en-GB",
            // the function will return closes match that is "en-US" because at least the language is the same (ie English)
            var n = GetNeutralCulture(name);
            foreach (var c in _supportedCultures)
                if (c.StartsWith(n))
                {
                    return c;
                }

            return DefaultCulture;
        }

        public static bool IsLanguageSupported(int id)
        {
            var culture = new CultureInfo(MapLangaugeToLCID(id));
            if (_supportedCultures.Any(c => c.Equals(culture.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                return true;
            }

            // Find a close match. For example, if you have "en-US" defined and the user requests "en-GB",
            // the function will return closes match that is "en-US" because at least the language is the same (ie English)
            var n = culture.TwoLetterISOLanguageName;
            return _supportedCultures.Any(c => c.StartsWith(n));
        }

        /// <summary>
        /// Gets returns default culture name which is the first name decalared (e.g. en-US)
        /// </summary>
        /// <returns></returns>
        public static string DefaultCulture
        {
            get { return _supportedCultures[0]; }
        }

        public static string CurrentCulture
        {
            get { return Thread.CurrentThread.CurrentCulture.Name; }
        }

        public static string CurrentNeutralCulture
        {
            get { return GetNeutralCulture(Thread.CurrentThread.CurrentCulture.Name); }
        }

        public static string GetNeutralCulture(string name)
        {
            if (!name.Contains("-"))
            {
                return name;
            }

            return name.Split('-')[0]; // Read first part only. E.g. "en", "es"
        }


        public static int MapLangaugeToLCID(int languageId)
        {
            switch (languageId)
            {
                case (int)KnownLanguages.English:
                    return (int)LanguageLCID.English;
                case (int)KnownLanguages.Arabic:
                    return (int)LanguageLCID.Arabic;
                default:
                    return (int)LanguageLCID.English;
            }
        }

        public static int MapLangaugeFromLCID(int languageId)
        {
            switch (languageId)
            {
                case (int)LanguageLCID.English:
                    return (int)KnownLanguages.English;
                case (int)LanguageLCID.Arabic:
                    return (int)KnownLanguages.Arabic;
                default:
                    return (int)KnownLanguages.English;
            }
        }
    }
}