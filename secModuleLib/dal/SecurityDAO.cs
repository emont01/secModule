using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.Data;
using lib.model;

namespace lib.dal
{
    public class SecurityDAO : IDisposable
    {
        DbManager dbManager = new DbManager();

        #region IDisposable Members

        public void Dispose()
        {
            dbManager.Dispose();
        }

        #endregion

        internal Menu getMenuByPath(string path)
        {
            return dbManager.SetCommand("SELECT [id],[path],[name] FROM [Menus] WHERE LOWER([path]) like LOWER(@path)",
                dbManager.Parameter("@path", path)).ExecuteObject<Menu>();
        }

        internal IList<Role> getRolesFor(Menu menu)
        {
            return dbManager.SetCommand(
                @"SELECT Roles.id, Roles.description, Roles.name
                FROM Menus 
                INNER JOIN Menus_Roles ON Menus.id = Menus_Roles.menu_id 
                INNER JOIN Roles ON Roles.id = Menus_Roles.role_id
                WHERE Menus.id =@menu_id", dbManager.Parameter("@menu_id", menu.Id)).ExecuteList<Role>();
        }
    }
}
