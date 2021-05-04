using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital
{
    public class LanguageSwitcherViewModel
    {
        /// <summary>
        /// The entries
        /// </summary>
        private readonly List<LanguageEntry> _entries = new List<LanguageEntry>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageSwitcherViewModel"/> class.
        /// </summary>
        public LanguageSwitcherViewModel()
        {
            _entries.Add(new LanguageEntry { ID = 1, Name = "AR", LCID = 1, Image = "AR.png" });
            _entries.Add(new LanguageEntry { ID = 2, Name = "EN", LCID = 2, Image = "EN.png" });
        }

        /// <summary>
        /// Gets the languages.
        /// </summary>
        /// <value>The languages.</value>
        public IEnumerable<LanguageEntry> Languages
        {
            get { return _entries; }
        }

        /// <summary>
        /// Struct LanguageEntry
        /// </summary>
        public struct LanguageEntry
        {
            /// <summary>
            /// The identifier
            /// </summary>
            public int ID;

            /// <summary>
            /// The lcid
            /// </summary>
            public int LCID;

            /// <summary>
            /// The name
            /// </summary>
            public string Name;

            public string Image;
        }
    }
}