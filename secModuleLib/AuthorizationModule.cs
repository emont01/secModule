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

namespace test
{
    /*
    public class AuthorizationModule : IHttpModule
    {

        public void Init(HttpApplication application)
        {
            application.AuthorizeRequest += new EventHandler(authorize);
        }

        public void authorize(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;

            if (SecurityUtil.existValidUser())
            {
                using (DbManager db = new DbManager(SettingsUtil.getWebSetting(SettingsUtil.ENVIRONMENT)))
                {
                    Menu menu = Menu.readByPath(db, WebAppUtil.removeVirtualPathAndConvertToLowerCase(application.Request.Path));
                    if (menu != null)
                    {
                        foreach (Role menuRole in menu.getMenuRoles(db))
                        {
                            if (!application.User.IsInRole(menuRole.Nombre))
                            {
                                throw new HttpException(401, "UnAuthorized access to " + application.Request.Path);
                            }
                        }
                    }
                }
            }
        }

        private Menu readByPath()
        {
            return db.SetCommand("SELECT M.* FROM SEGURIDAD_MENU M WHERE LOWER(URL) = LOWER(@path) and LOWER(APLICACION) = LOWER(@app)",
                db.Parameter("@path", path),
                db.Parameter("@app", SettingsUtil.getWebSetting(SettingsUtil.APPLICATION_NAME)))
                .ExecuteObject<Menu>();
        }

        public void Dispose()
        {
        }
    }
    */
}
