using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.Data;
using BLToolkit.Data.Linq;
using lib.model;
using System.Web.Security;
using BLToolkit.DataAccess;
using System.Security.Cryptography;

namespace lib.dal
{
    public class SecurityDAO : IDisposable
    {
        private readonly DbManager dbManager;
        private SqlQuery<User> userAccessor;
        private SqlQuery<Role> roleAccessor;
        private SqlQuery<Menu> menuAccessor;
        private SqlQuery<UserRole> userRoleAccessor;
        private SqlQuery<MenuRole> menuRoleAccessor;

        public SecurityDAO()
        {
            dbManager = new DbManager();
            userAccessor = new SqlQuery<User>(dbManager);
            roleAccessor = new SqlQuery<Role>(dbManager);
            menuAccessor = new SqlQuery<Menu>(dbManager);
            userRoleAccessor = new SqlQuery<UserRole>(dbManager);
            menuRoleAccessor = new SqlQuery<MenuRole>(dbManager);
        }

        #region Tables properties
        /**
         * To simplify the linq usage
         */
        private Table<Menu> Menus
        {
            get { return dbManager.GetTable<Menu>(); }
        }

        private Table<Role> Roles
        {
            get { return dbManager.GetTable<Role>(); }
        }

        private Table<User> Users
        {
            get { return dbManager.GetTable<User>(); }
        }

        private Table<MenuRole> MenusRoles
        {
            get { return dbManager.GetTable<MenuRole>(); }
        }

        private Table<UserRole> UsersRoles
        {
            get { return dbManager.GetTable<UserRole>(); }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {

            userAccessor = null;
            roleAccessor = null;
            menuAccessor = null;
            userRoleAccessor = null;
            menuRoleAccessor = null;
            dbManager.Dispose();
        }

        #endregion

        internal Menu GetMenuByPath(string path)
        {
            path = String.IsNullOrEmpty(path) ? "" : path.ToLower();

            var query = from m in Menus
                        where m.Path.Contains(path)
                        select m;
            return query.FirstOrDefault();
        }

        internal IList<Role> GetRolesFor(Menu paramMenu)
        {
            var query = from menu in Menus
                        join menuRole in MenusRoles on menu.Id equals menuRole.MenuId
                        join role in Roles on menuRole.RoleId equals role.Id
                        where menu.Id == paramMenu.Id
                        select role;
            return query.ToList();
        }

        internal User ReadUserByName(string userName)
        {
            var query = from u in Users
                        where u.Name.Contains(userName)
                        select u;
            return query.FirstOrDefault();
        }

        internal User ReadUserById(int userId)
        {
            return userAccessor.SelectByKey(userId);
        }

        internal MembershipUser ConvertUserToMembershipUser(User user, string providerName)
        {
            return user == null ? null : new MembershipUser(providerName, user.Name, user.Id, "", "", "",
                true, user.Blocked, user.CreatedAt, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
        }

        internal void UpdateUser(User user)
        {
            userAccessor.Update(user);
        }

        internal IList<User> ListUsersByName(string userNameToMatch, int pageIndex, int pageSize)
        {
            var offset = pageIndex * pageSize;
            //var limit = offset + pageSize;
            var query = from u in Users
                        where u.Name.Contains(userNameToMatch)
                        select u;
            var users = query.Skip(offset).Take(pageSize).ToList();
//                dbManager.SetCommand(@"SELECT * FROM
//                              ( SELECT row_number() OVER (ORDER BY name ASC) row_num, u.* FROM Users u ) 
//                              Users
//                              WHERE row_num >= @offset AND row_num < @limit AND name LIKE @user_name",
//                          dbManager.Parameter("@offset", offset),
//                          dbManager.Parameter("@limit", limit),
//                          dbManager.Parameter("@user_name", userNameToMatch)).ExecuteScalarList<User>();
            return users;
        }

        internal Role ReadRoleByName(string roleName)
        {
            var query = from r in Roles
                        where r.Name.Contains(roleName)
                        select r;
            return query.FirstOrDefault();
        }

        internal void AssignRoleToUser(string roleName, string userName)
        {
            var user = ReadUserByName(userName);
            var role = ReadRoleByName(roleName);
            var userRole = new UserRole
                               {
                                   RoleId = role.Id, 
                                   UserId = user.Id
                               };
            userRoleAccessor.Insert(userRole);
        }


        internal void AssignRoleToMenu(string roleName, string menuPath)
        {
            var role = ReadRoleByName(roleName);
            var menu = ReadMenuByPath(menuPath);
            var menuRole = new MenuRole
                               {
                                   MenuId = menu.Id, 
                                   RoleId = role.Id
                               };
            menuRoleAccessor.Insert(menuRole);
        }

        private Menu ReadMenuByPath(string menuPath)
        {
            var query = from m in Menus
                        where m.Path.Contains(menuPath)
                        select m;
            return query.FirstOrDefault();
        }

        internal void CreateRole(string name, string description)
        {
            CreateRole(new Role { Name = name, Description = description });
        }

        internal void CreateRole(Role role)
        {
            roleAccessor.Insert(role);
        }

        internal void CreateUser(string email, string name, string plainTextPassword)
        {
            var salt = GenerateRandomBase64Salt();
            var user = new User
            {
                Blocked = false,
                CreatedAt = DateTime.Now,
                Email = email,
                Name = name,
                Password = EncodePassword(plainTextPassword, salt),
                Salt = salt
            };
            CreateUser(user);
        }

        internal void CreateUser(User user)
        {
            userAccessor.Insert(user);
        }

        internal string EncodePassword(string password, string salt)
        {
            var bytes = Encoding.Unicode.GetBytes(password);
            var src = Encoding.Unicode.GetBytes(salt);
            var dst = new byte[src.Length + bytes.Length];

            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);

            var algorithm = HashAlgorithm.Create("SHA1");

            var inArray = algorithm.ComputeHash(dst);

            return Convert.ToBase64String(inArray);
        }

        private static string GenerateRandomBase64Salt()
        {
            var saltBytes = new byte[50];

            var rng = RNGCryptoServiceProvider.Create();
            rng.GetNonZeroBytes(saltBytes);
            rng.GetBytes(saltBytes);
            var b64Salt = Convert.ToBase64String(saltBytes);
            return b64Salt;
        }

        internal void DeleteRole(Role role)
        {
            roleAccessor.Delete(role);
        }

        internal IList<User> ListUsersByRole(string roleName)
        {
            var query = from user in Users
                        join userRole in UsersRoles on user.Id equals userRole.UserId
                        join role in Roles on userRole.RoleId equals role.Id
                        where role.Name.Contains(roleName)
                        select user;
            return query.ToList();
        }

        internal IList<string> ListUserNamesByRole(string roleName)
        {
            var query = from user in Users
                        join userRole in UsersRoles on user.Id equals userRole.UserId
                        join role in Roles on userRole.RoleId equals role.Id
                        where role.Name.Contains(roleName)
                        select user.Name;
            return query.ToList();
        }

        internal bool IsUserInRole(string userName, string roleName)
        {
            var query = from user in Users
                        join userRole in UsersRoles on user.Id equals userRole.UserId
                        join role in Roles on userRole.RoleId equals role.Id
                        where role.Name.Contains(roleName) && user.Name.Contains(userName)
                        select user;
            return query.Any();
        }

        internal void RemoveUserFromRole(string userName, string roleName)
        {
            var user = ReadUserByName(userName);
            var role = ReadRoleByName(roleName);

            var userRole = (from ur in UsersRoles
                               where ur.UserId == user.Id && ur.RoleId == role.Id
                               select ur).FirstOrDefault();
            userRoleAccessor.Delete(userRole);
        }

        internal IList<string> ListRoleNamesByUser(string userName)
        {
            var query = from role in Roles
                        join userRole in UsersRoles on role.Id equals userRole.RoleId
                        join user in Users on userRole.UserId equals user.Id 
                        where user.Name.Contains(userName)
                        select role.Name;
            return query.ToList();
        }

        internal IList<string> ListUserNamesByRoleAndName(string roleName, string userNameToMatch)
        {
            var query = from user in Users
                        join userRole in UsersRoles on user.Id equals userRole.UserId
                        join role in Roles on userRole.RoleId equals role.Id
                        where role.Name.Contains(roleName) && user.Name.Contains(userNameToMatch)
                        select user.Name;
            return query.ToList();
        }

        internal IList<string> ListAllRoleNames()
        {
            //return dbManager.SetCommand("SELECT name FROM Roles").ExecuteScalarList<string>();
            return Roles.Select(r => r.Name).ToList();
        }

        internal void RecordUserLoginSuccess(User user)
        {
            //throw new NotImplementedException();
        }

        internal void RecordUserLoginFailure(User user)
        {
            //throw new NotImplementedException();
        }

        internal void CreateMenu(string name, string path)
        {
            CreateMenu(new Menu { Name = name, Path = path });
        }

        internal void CreateMenu(Menu menu)
        {
            menuAccessor.Insert(menu);
        }

        internal void RecordUserActivity(User dbUser)
        {
            //throw new NotImplementedException();
        }

        internal IList<Role> ListAllRoles()
        {
            return roleAccessor.SelectAll();
        }
    }
}
