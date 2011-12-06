using System;
using System.Web;
using System.Web.UI;

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
                page.Resource = new Resource(
                    String.Format("The owner of this resource is an instance of the class {0}",
                                  page.GetType().Name));
            }
            return handler;
        }

    }

}
