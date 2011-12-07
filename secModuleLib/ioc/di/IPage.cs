
using lib.i18n;
namespace lib.ioc.di
{
    /// <summary>
    /// Interface for all pages
    /// </summary>
    public interface IPage
    {

        I18NSupport I18NHelper { get; set; }

        // I18N function
        string _(string text);
    }
}
