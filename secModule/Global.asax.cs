using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using lib.modules;

namespace web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected virtual void Application_Error(Object sender, EventArgs e)
        {
            string redirectionURL = "";
            HttpContext context = HttpContext.Current;
            try
            {
                Exception exception = context.Server.GetLastError().GetBaseException();

                if (exception.GetType() == typeof(System.Security.SecurityException))
                {
                    if (exception.Message == "Request for principal permission failed.")
                        redirectionURL = WebAppUtil.getAppVirtualPath() + "unauthorized.aspx";
                }
                else if (exception.Message.EndsWith("does not exist."))
                {
                    redirectionURL = WebAppUtil.getAppVirtualPath() + "page_not_found.aspx";
                }
                else
                {
                    redirectionURL = WebAppUtil.getAppVirtualPath() + "error_page.aspx";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                // --------------------------------------------------
                // To let the page finish running we clear the error
                // --------------------------------------------------
                context.Server.ClearError();
                context.Response.Redirect(redirectionURL, true);
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}