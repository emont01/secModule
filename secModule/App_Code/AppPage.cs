using System.Web.UI;
using lib.i18n;
using lib.ioc.di;

namespace web
{
    /// <summary>
    ///     Default app page
    /// </summary>
    public class AppPage : Page, IPage
    {
        #region IPage Members

        public I18NSupport I18NHelper { get; set; }

        public string _(string text)
        {
            return I18NHelper._(text);
        }

        #endregion
    }
}