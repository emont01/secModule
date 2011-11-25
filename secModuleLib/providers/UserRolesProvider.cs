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
using System.Collections.Specialized;
using System.Linq;
using System.Web.Security;
using lib.dal;
using lib.model;

namespace lib.providers
{
    public class UserRolesProvider : RoleProvider
    {
        private const string DESCRIPTION = "description", APP_NAME = "applicationName",
            MAX_INVALID_PWD_ATTEMPS = "maxInvalidPasswordAttempts";

        public override string ApplicationName { get; set; }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            if (name == null || String.IsNullOrEmpty(name.Trim()))
            {
                name = "UserRolesProvider";
            }

            ConfigHelper util = new ConfigHelper(config);

            config[DESCRIPTION] = util.contains(DESCRIPTION) ? util.get(DESCRIPTION) : "Custom Role Provider";

            this.ApplicationName = util.getOrFail(APP_NAME);
            base.Initialize(name, config);

        }

        public override void AddUsersToRoles(string[] userNames, string[] roleNames)
        {
            validateRolesExists(roleNames);

            validateUsersDoNotHaveRolesAllready(userNames, roleNames);

            using (SecurityDAO secDAO = new SecurityDAO())
            {
                foreach (string username in userNames)
                {
                    foreach (string rolename in roleNames)
                    {
                        secDAO.assignRoleToUser(rolename, username);
                    }
                }
            }
        }

        private void validateUsersDoNotHaveRolesAllready(string[] userNames, string[] roleNames)
        {
            foreach (string userName in userNames)
            {
                if (userName.Contains(","))
                {
                    throw new ArgumentException("User names cannot contain commas.");
                }
                foreach (string roleName in roleNames)
                {
                    if (IsUserInRole(userName, roleName))
                    {
                        throw new InvalidOperationException("User is already in role");
                    }
                }
            }
        }

        private void validateRolesExists(string[] roleNames)
        {
            foreach (string rolename in roleNames)
            {
                if (!RoleExists(((rolename == null) ? "" : rolename).Trim()))
                {
                    throw new ArgumentException("Role [" + rolename + "]name not found");
                }
            }
        }

        public override void CreateRole(string roleName)
        {
            if (roleName.Contains(","))
            {
                throw new ArgumentException("Role names cannot contain commas.");
            }
            if (RoleExists(roleName))
            {
                throw new InvalidOperationException("Role name [" + roleName + "]already exists");
            }
            using (SecurityDAO secDAO = new SecurityDAO())
            {
                secDAO.createRole(new Role
                {
                    Name = roleName,
                    Description = ""
                });
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if (!RoleExists(roleName))
            {
                throw new InvalidOperationException("Role does not exists");
            }
            if (throwOnPopulatedRole && GetUsersInRole(roleName).Length > 0)
            {
                throw new InvalidOperationException("Cannot delete a role with related users");
            }
            using (SecurityDAO secDAO = new SecurityDAO())
            {
                secDAO.deleteRole(secDAO.readRoleByName(roleName));
            }
            return true;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            using (SecurityDAO secDAO = new SecurityDAO())
            {
                return secDAO.listUserNamesByRole(roleName).ToArray();
            }
        }

        public override bool RoleExists(string roleName)
        {
            using (SecurityDAO dao = new SecurityDAO())
            {
                return dao.readRoleByName(roleName) != null;
            }
        }

        public override bool IsUserInRole(string userName, string roleName)
        {
            using (SecurityDAO dao = new SecurityDAO())
            {
                return dao.isUserInRole(userName, roleName);
            }
        }

        public override void RemoveUsersFromRoles(string[] userNames, string[] roleNames)
        {
            foreach (string roleName in roleNames)
            {
                if (!RoleExists(roleName))
                {
                    throw new InvalidOperationException("Role name not found");
                }
            }

            foreach (string userName in userNames)
            {
                foreach (string roleName in roleNames)
                {
                    if (!IsUserInRole(userName, roleName))
                    {
                        throw new InvalidOperationException("User is not in role");
                    }
                }
            }

            using (SecurityDAO dao = new SecurityDAO())
            {
                foreach (string userName in userNames)
                {
                    foreach (string roleName in roleNames)
                    {
                        dao.removeUserFromRole(userName, roleName);
                    }
                }
            }
        }

        public override string[] GetRolesForUser(string userName)
        {
            using (SecurityDAO dao = new SecurityDAO())
            {
                string[] roles = dao.listRoleNamesByUser(userName).ToArray();
                return roles;
            }
        }

        public override string[] FindUsersInRole(string roleName, string userNameToMatch)
        {
            using (SecurityDAO dao = new SecurityDAO())
            {
                return dao.listUserNamesByRoleAndName(roleName, userNameToMatch).ToArray();
            }
        }

        public override string[] GetAllRoles()
        {
            using (SecurityDAO dao = new SecurityDAO())
            {
                return dao.listAllRolesNames().ToArray();
            }
        }
    }
}

