using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.Data;
using lib.model;
using System.Web.Security;
using BLToolkit.DataAccess;

namespace lib.dal
{
    public class SecurityDAO : IDisposable
    {
        private DbManager dbManager;
        private SqlQuery<User> userAccessor;
        private SqlQuery<Role> roleAccessor;
        private SqlQuery<Menu> menuAccessor;
        private SqlQuery<UserRole> userRoleAccessor;
        private SqlQuery<MenuRole> menuRoleAccessor;

        public SecurityDAO()
        {
            dbManager = new DbManager();
            userAccessor = new SqlQuery<User>(dbManager);
        }

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

        internal User readUserByName(string userName)
        {
            return dbManager.SetCommand(
                @"SELECT [id],[name],[email],[salt],[password],[created_at] 
                FROM [Users] WHERE name like @user_name",
                dbManager.Parameter("@user_name", userName)).ExecuteObject<User>();
        }

        internal User readUserById(int userId)
        {
            //TODO we may use userAccessor.SelectByKey(params object[] keys) and not a custom sql
            return dbManager.SetCommand(
                @"SELECT [id],[name],[email],[salt],[password],[created_at] 
                FROM [Users] WHERE name = @user_id",
                dbManager.Parameter("@user_id", userId)).ExecuteObject<User>();
        }

        internal MembershipUser convertUserToMembershipUser(User user, string providerName)
        {
            return user == null ? null : new MembershipUser(providerName, user.Name, user.Id, "", "", "",
                true, user.Blocked, user.CreatedAt, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
        }

        internal void saveUserLogin(Int32 userId)
        {
            //
        }

        internal void updateUser(User user)
        {
            userAccessor.Update(user);
        }

        internal IList<User> listUsersByName(string userNameToMatch, int pageIndex, int pageSize)
        {
            int offset = pageIndex * pageSize;
            int limit = offset + pageSize;
            List<User> users = dbManager.SetCommand(@"SELECT * FROM
                              ( SELECT row_number() OVER (ORDER BY name ASC) row_num, u.* FROM Users u ) 
                              Users
                              WHERE row_num >= @offset AND row_num < @limit AND name LIKE @user_name",
                          dbManager.Parameter("@offset", offset), 
                          dbManager.Parameter("@limit", limit),
                          dbManager.Parameter("@user_name", userNameToMatch)).ExecuteScalarList<User>();
            return users;
        }

        internal Role readRoleByName(string roleName)
        {
            Role role = dbManager.SetCommand(@"SELECT [id],[name],[description] 
                FROM dbo.[Roles] WHERE [name] like @role_name",
                dbManager.Parameter("@role_name", roleName)).ExecuteObject<Role>();
            return role;
        }

        internal void assignRoleToUser(string roleName, string userName)
        {
            User user = readUserByName(userName);
            Role role = readRoleByName(roleName);
            UserRole userRole = new UserRole();
            userRole.RoleId = role.Id;
            userRole.UserId = user.Id;
            userRoleAccessor.Insert(userRole);
        }

        internal void createRole(Role role)
        {
            roleAccessor.Insert(role);
        }

        internal void deleteRole(Role role)
        {
            roleAccessor.Delete(role);
        }

        internal IList<User> listUsersByRole(string roleName)
        {
            return dbManager.SetCommand(@"SELECT Users.*
                FROM Users 
                INNER JOIN Users_Roles ON Users.id = Users_Roles.user_id
                INNER JOIN Roles ON Roles.id = Users_Roles.role_id 
                WHERE Roles.name like @role_name",
                dbManager.Parameter("@role_name", roleName)).ExecuteList<User>();
        }

        internal IList<string> listUserNamesByRole(string roleName)
        {
            return dbManager.SetCommand(@"SELECT Users.name
                FROM Users 
                INNER JOIN Users_Roles ON Users.id = Users_Roles.user_id
                INNER JOIN Roles ON Roles.id = Users_Roles.role_id 
                WHERE Roles.name like @role_name",
                dbManager.Parameter("@role_name", roleName)).ExecuteScalarList<string>();
        }

        internal bool isUserInRole(string userName, string roleName)
        {

            Int32 count = dbManager.SetCommand(@"SELECT count(Users.id)
                FROM Users 
                INNER JOIN Users_Roles ON Users.id = Users_Roles.user_id
                INNER JOIN Roles ON Roles.id = Users_Roles.role_id 
                WHERE Roles.name like @role_name AND Users.name like @user_name",
                dbManager.Parameter("@role_name", roleName),
                dbManager.Parameter("@user_name", userName)).ExecuteScalar<Int32>();
            return count > 0;
        }

        internal void removeUserFromRole(string userName, string roleName)
        {
            User user = readUserByName(userName);
            Role role = readRoleByName(roleName);
            dbManager.SetCommand("DELETE FROM Users_Roles WHERE user_id = @user_id and role_id = @role_id",
                dbManager.Parameter("@user_id", user.Id), dbManager.Parameter("@role_id", role.Id))
                .ExecuteNonQuery();
        }


        internal IList<string> listRoleNamesByUser(string userName)
        {
            return dbManager.SetCommand(@"SELECT Roles.*
                FROM Roles 
                INNER JOIN Users_Roles ON Roles.id = Users_Roles.role_id
                INNER JOIN Users ON Users.id = Users_Roles.user_id
                WHERE Users.name like @user_name",
                dbManager.Parameter("@user_name", userName)).ExecuteScalarList<string>();
        }

        internal IList<string> listUserNamesByRoleAndName(string roleName, string userNameToMatch)
        {
            return dbManager.SetCommand(@"SELECT Users.name
                FROM Users 
                INNER JOIN Users_Roles ON Users.id = Users_Roles.user_id
                INNER JOIN Roles ON Roles.id = Users_Roles.role_id 
                WHERE Roles.name like @role_name AND Users.name like @user_name",
                dbManager.Parameter("@role_name", roleName),
                dbManager.Parameter("@user_name", userNameToMatch)).ExecuteScalarList<string>();
        }

        internal IList<string> listAllRolesNames()
        {
            return dbManager.SetCommand("SELECT name FROM Roles").ExecuteScalarList<string>();
        }

    }
}
