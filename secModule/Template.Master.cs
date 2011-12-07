using System;
using lib.ioc.di;

namespace web
{
    public partial class Template : System.Web.UI.MasterPage, IPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region IPage Members

        public lib.i18n.I18NSupport I18NHelper { get; set; }

        public string _(string text)
        {
            return I18NHelper._(text);
        }

        #endregion
    }
}
