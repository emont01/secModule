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
using System.Text;
using System.Web.Security;
using lib.dal;
using lib.model;
using System.Collections.Specialized;

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

            base.Initialize(name, config);
            this.ApplicationName = util.getOrFail(APP_NAME);

        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            validateRolesExists(roleNames);

            validateUsersDoNotHaveRolesAllready(usernames, roleNames);

            using (SecurityDAO secDAO = new SecurityDAO())
            {
                foreach (string username in usernames)
                {
                    foreach (string rolename in roleNames)
                    {
                        secDAO.assignRoleToUser(rolename, username);
                    }
                }
            }
        }

        private void validateUsersDoNotHaveRolesAllready(string[] usernames, string[] roleNames)
        {
            foreach (string username in usernames)
            {
                if (username.Contains(","))
                {
                    throw new ArgumentException("User names cannot contain commas.");
                }
                foreach (string rolename in roleNames)
                {
                    if (IsUserInRole(username, rolename))
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
                return secDAO.listUsersNamesByRole(roleName).ToArray();
            }
        }

        public override bool RoleExists(string roleName)
        {
            using (SecurityDAO dao = new SecurityDAO())
            {
                return dao.readRoleByName(roleName) != null;
            }
        }

        public override bool IsUserInRole(string username, string rolename)
        {
            using (SecurityDAO dao = new SecurityDAO())
            {
                return dao.isUserInRole(username, rolename);
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            foreach (string rolename in roleNames)
            {
                if (!RoleExists(rolename))
                {
                    throw new InvalidOperationException("Role name not found");
                }
            }

            foreach (string username in usernames)
            {
                foreach (string rolename in roleNames)
                {
                    if (!IsUserInRole(username, rolename))
                    {
                        throw new InvalidOperationException("User is not in role");
                    }
                }
            }

            using (SecurityDAO dao = new SecurityDAO())
            {
                foreach (string username in usernames)
                {
                    foreach (string rolename in roleNames)
                    {
                        dao.removeUserFromRole(username, rolename);
                    }
                }
            }
        }

        public override string[] GetRolesForUser(string username)
        {
            using (SecurityDAO dao = new SecurityDAO())
            {
                return dao.listRolesByUser(dao.readUserByName(username));
            }
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            using (SecurityDAO dao = new SecurityDAO())
            {
                return null;
            }
        }

        public override string[] GetAllRoles()
        {
            using (SecurityDAO dao = new SecurityDAO())
            {
                return dao.listAllRolesNames();
            }
        }
    }
}

