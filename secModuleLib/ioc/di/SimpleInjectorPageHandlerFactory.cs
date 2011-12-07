using System;
using System.Web;
using System.Web.UI;
using lib.i18n;

namespace lib.ioc.di
{

    public class SimpleInjectorPageHandlerFactory : PageHandlerFactory
    {

        public override IHttpHandler GetHandler(HttpContext context, string requestType, string virtualPath, string path)
        {
            var handler = base.GetHandler(context, requestType, virtualPath, path);
            if (handler != null && handler is IPage)
            {
                IPage page = handler as IPage;
                //all the reources will be injected here.
                page.I18NHelper = new I18NHelper();
            }
            return handler;
        }

    }

}
