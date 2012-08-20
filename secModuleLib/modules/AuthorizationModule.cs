/**
Copyright 2011 Eivar Montenegro <e.mont01@gmail.com>

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
   limitations under the License.
**/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using lib.dal;
using lib.model;
using BLToolkit.Data;

namespace lib.modules
{

    public class AuthorizationModule : IHttpModule
    {
        public void Init(HttpApplication application)
        {
            application.AuthorizeRequest += new EventHandler(authorize);
        }

        public void authorize(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;

            if (existValidUser())
            {
                using (SecurityDAO secDAO = new SecurityDAO())
                {
                    Menu menu = secDAO.GetMenuByPath(getVirtualPathAsLowerCase(application));
                    if (menu != null)
                    {
                        foreach (Role menuRole in secDAO.GetRolesFor(menu))
                        {
                            if (!userIsInRole(application, menuRole))
                            {
                                throw new HttpException(401, "UnAuthorized access to " + application.Request.Path);
                            }
                        }
                    }
                }
            }
        }

        private bool userIsInRole(HttpApplication application, Role menuRole)
        {
            return application.User.IsInRole(menuRole.Name);
        }

        private string getVirtualPathAsLowerCase(HttpApplication application)
        {
            return WebAppUtil.removeVirtualPathAndConvertToLowerCase(application.Request.Path);
        }

        private bool existValidUser()
        {
            return HttpContext.Current.User != null &&
                HttpContext.Current.User.Identity.IsAuthenticated &&
                HttpContext.Current.User.Identity.GetType() == typeof(FormsIdentity);
        }
        
        public void Dispose()
        {
        }
    }

    public sealed class WebAppUtil
    {

        public static string removeVirtualPathAndConvertToLowerCase(string requestPath)
        {
            string virtualPath = getAppVirtualPathAsLowerCase();
            //requestPath in lower case
            requestPath = requestPath.ToLower();
            if (requestPath.StartsWith(virtualPath))
            {
                requestPath = requestPath.Remove(0, virtualPath.Length);
            }
            return requestPath;
        }

        public static string getAppVirtualPath()
        {
            string virtualPath = HttpRuntime.AppDomainAppVirtualPath;
            //if there is no vitual path return /(root) or append / to path (${virtualPath}/)
            return virtualPath.Equals("/") ? virtualPath : virtualPath + "/";
        }

        public static string getAppVirtualPathAsLowerCase()
        {
            return getAppVirtualPath().ToLower();
        }
    }
}
