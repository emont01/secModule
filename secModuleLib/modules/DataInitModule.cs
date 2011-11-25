using System.Web;
using System.Web.Configuration;
using System;
using lib.dal;
using BLToolkit.Data;
using lib.model;

namespace lib.modules
{
    public class DataInitModule : IHttpModule
    {
        
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            if (InsertDefaultDataOnInit)
            {
                using (var secDAO = new SecurityDAO())
                {
                    secDAO.createRole("admins", "");

                    secDAO.createUser("admin@example.net", "admin", "admin");
                    secDAO.assignRoleToUser("admins", "admin");

                    secDAO.createMenu("home", "_private/home.aspx");
                    secDAO.assignRoleToMenu("admins", "_private/home.aspx");
                }
            }
            context.Disposed += new EventHandler(onAppDispose);
        }

        private bool InsertDefaultDataOnInit
        {
            get
            {
                string aux = WebConfigurationManager.AppSettings["InsertDefaultDataOnInit"];
                return string.IsNullOrEmpty(aux) ? false : Convert.ToBoolean(aux);
            }
        }

        void onAppDispose(object sender, EventArgs e)
        {
            if (DeleteDataOnDispose)
            {
                using (var db = new DbManager())
                {
                    db.SetCommand(
                        @"TRUNCATE TABLE dbo.Users_Roles
                        TRUNCATE TABLE dbo.Menus_Roles
                        TRUNCATE TABLE dbo.Menus
                        TRUNCATE TABLE dbo.Users
                        TRUNCATE TABLE dbo.Roles").ExecuteNonQuery();
                }
            }
        }

        private bool DeleteDataOnDispose
        {
            get
            {
                string aux = WebConfigurationManager.AppSettings["DeleteDataOnDispose"];
                return string.IsNullOrEmpty(aux)? false : Convert.ToBoolean(aux);
            }
        }

        #endregion
    }
}
