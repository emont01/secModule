﻿/**
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
using System.Collections.Specialized;
using System.Configuration;
using System.Runtime.Remoting.Contexts;
using System.Web.Configuration;
using System.Web.Security;
using lib.dal;
using lib.model;

namespace lib.providers
{


    [Context("MembershipProvider")]
    public class AuthenticationProvider : MembershipProvider
    {
        private const string DESCRIPTION = "description", APP_NAME = "applicationName",
            MAX_INVALID_PWD_ATTEMPS = "maxInvalidPasswordAttempts";

        private bool pEnablePasswordReset;
        private bool pEnablePasswordRetrieval;
        private bool pRequiresQuestionAndAnswer;
        private bool pRequiresUniqueEmail;
        private int pMaxInvalidPasswordAttempts;
        private int pPasswordAttemptWindow;
        private MembershipPasswordFormat pPasswordFormat;
        private int pMinRequiredNonAlphanumericCharacters;
        private int pMinRequiredPasswordLength;
        private string pPasswordStrengthRegularExpression;

        private MachineKeySection machineKey;// encryption key values.

        #region System.Web.Security.MembershipProvider properties

        public override string ApplicationName { get; set; }

        public override bool EnablePasswordReset
        {
            get { return pEnablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return pEnablePasswordRetrieval; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return pRequiresQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return pRequiresUniqueEmail; }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return pMaxInvalidPasswordAttempts; }
        }

        public override int PasswordAttemptWindow
        {
            get { return pPasswordAttemptWindow; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return pPasswordFormat; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return pMinRequiredNonAlphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return pMinRequiredPasswordLength; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return pPasswordStrengthRegularExpression; }
        }
        #endregion

        #region configs

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            if (name == null || String.IsNullOrEmpty(name.Trim()))
            {
                name = "AuthenticationProvider";
            }

            ConfigHelper helper = new ConfigHelper(config);


            config[DESCRIPTION] = helper.contains(DESCRIPTION) ? helper.get(DESCRIPTION) : "Custom Membership Provider";

            ApplicationName = helper.getOrFail(APP_NAME);

            base.Initialize(name, config);

            pMaxInvalidPasswordAttempts = helper.contains(MAX_INVALID_PWD_ATTEMPS) ?
                Convert.ToInt32(helper.get(MAX_INVALID_PWD_ATTEMPS)) : 3;

            pPasswordFormat = MembershipPasswordFormat.Clear;
            pEnablePasswordReset = false;
            pEnablePasswordRetrieval = false;
            pRequiresQuestionAndAnswer = false;
            pRequiresUniqueEmail = false;
            pPasswordAttemptWindow = 0;

            machineKey = ConfigurationManager.GetSection("system.web/machineKey") as MachineKeySection;
            if (machineKey.ValidationKey.Contains("AutoGenerate"))
            {
                if (PasswordFormat != MembershipPasswordFormat.Clear)
                {
                    throw new ConfigurationException("Hashed or Encrypted passwords are not supportedwith auto-generated keys.");
                }
            }
        }

        #endregion

        #region System.Web.Security.MembershipProvider methods implementation

        /**
         * MembershipProvider.ChangePassword
         */
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new InvalidOperationException("Password Retrieval Not Enabled.");
        }

        /// <summary>
        /// Takes, as input, a user name and a Boolean value indicating whether to update the LastActivityDate value for the user to show that the user is currently online. The GetUser method returns a MembershipUser object populated with current values from the data source for the specified user. If the user name is not found in the data source, the GetUser method returns null (Nothing in Visual Basic).
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            using (SecurityDAO secDAO = new SecurityDAO())
            {
                User dbUser = secDAO.ReadUserByName(username);
                MembershipUser user = secDAO.ConvertUserToMembershipUser(dbUser, this.Name);
                if (userIsOnline)
                {
                    secDAO.RecordUserActivity(dbUser);
                }
                return user;
            }
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            using (SecurityDAO secDAO = new SecurityDAO())
            {
                Int32 id = Convert.ToInt32(providerUserKey);
                User dbUser = secDAO.ReadUserById(id);
                MembershipUser user = secDAO.ConvertUserToMembershipUser(dbUser, this.Name);
                if (userIsOnline)
                {
                    secDAO.RecordUserActivity(dbUser);
                }
                return user;
            }
        }

        public override bool UnlockUser(string username)
        {
            try
            {
                using (SecurityDAO secDAO = new SecurityDAO())
                {
                    User user = secDAO.ReadUserByName(username);
                    if (user == null)
                    {
                        return false;
                    }
                    user.Blocked = false;
                    secDAO.UpdateUser(user);
                    //TODO record somewhere into the db that the user got unblocked
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            using (SecurityDAO secDAO = new SecurityDAO())
            {
                User dbUser = secDAO.ReadUserById(Convert.ToInt32(user.ProviderUserKey));
                dbUser.Blocked = user.IsLockedOut;
                secDAO.UpdateUser(dbUser);
            }
        }

        public override bool ValidateUser(string username, string password)
        {
            using (SecurityDAO secDAO = new SecurityDAO())
            {
                User user = secDAO.ReadUserByName(username);
                if (user == null)
                    return false;

                string hashedPassword = secDAO.EncodePassword(password, user.Salt);

                bool isValid = (!user.Blocked && user.Password == hashedPassword);
                if (isValid)
                {
                    secDAO.RecordUserLoginSuccess(user);
                }
                else
                {
                    //TODO record user login attemp failure
                    secDAO.RecordUserLoginFailure(user);
                }
                return isValid;
            }
        }


        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            MembershipUserCollection collection = new MembershipUserCollection();
            using (SecurityDAO secDAO = new SecurityDAO())
            {
                IList<User> users = secDAO.ListUsersByName(usernameToMatch, pageIndex, pageSize);
                foreach (User user in users)
                {
                    collection.Add(secDAO.ConvertUserToMembershipUser(user, this.Name));
                }
                totalRecords = users.Count;
            }
            return collection;
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException("This is not implemented");
        }


        #endregion

    }

}
