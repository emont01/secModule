using System.Web;
using FairlyLocal;

namespace lib.i18n
{

    public interface I18NSupport
    {
        string _(string text);
        string LanguageCode { get; set; }
    }

    public class I18NHelper : lib.i18n.I18NSupport
    {

        #region internationalization

        string _languageCode;

        /// <summary>
        /// Get the current user's language preference.
        /// First looks for stored preference in the Session.
        /// Falls back on the best match from the browser's language collection.
        /// </summary>
        public string LanguageCode
        {
            get
            {
                if (_languageCode == null)
                {
                    if (HttpContext.Current.Session[Internationalization.Settings.LanguageCodeSessionKey] != null)
                    {
                        _languageCode = HttpContext.Current.Session[Internationalization.Settings.LanguageCodeSessionKey].ToString();
                    }
                    else
                    {
                        _languageCode = Internationalization.GetBestLanguage(HttpContext.Current.Request, Internationalization.Settings.WorkingLanguage);
                        HttpContext.Current.Session[Internationalization.Settings.LanguageCodeSessionKey] = _languageCode;
                    }
                }
                return _languageCode;
            }
            set
            {
                _languageCode = value;
                HttpContext.Current.Session[Internationalization.Settings.LanguageCodeSessionKey] = _languageCode;
            }
        }

        /// <summary>
        /// Returns the local translation of this piece of text.
        /// Shorthand for Internationalization.GetText.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string _(string text)
        {
            return Internationalization.GetText(text, LanguageCode);
        }

        #endregion
    }
}
