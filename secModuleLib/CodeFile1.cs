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
//namespace test
//{
    
//    public enum Configs
//    {
//        applicationName,
//        description,
//        maxInvalidPasswordAttempts,
//        writeExceptionsToEventLog,
//    }

//    [Tracing]
//    public sealed class ProviderUtil : ContextBoundObject
//    {
//        // If false, exceptions are thrown to the caller. If true,
//        // exceptions are written to the event log.
//        private bool pWriteExceptionsToEventLog;
//        private string name;

//        public ProviderUtil(string name)
//        {
//            this.Name = name;
//        }

//        public bool WriteExceptionsToEventLog
//        {
//            get { return pWriteExceptionsToEventLog; }
//            set { pWriteExceptionsToEventLog = value; }
//        }

//        public string Name
//        {
//            get { return this.name; }
//            set { this.name = value; }
//        }

//        public void raiseError(string action, Exception ex)
//        {
//            if (WriteExceptionsToEventLog)
//            {
//                writeToEventLog(action, ex);
//            }
//            throw new ProviderException(ex.Message, ex);
//        }

//        public string getCurrectConnectionStringName()
//        {
//            return SettingsUtil.getWebSetting(SettingsUtil.ENVIRONMENT);
//        }

//        //
//        // WriteToEventLog
//        // A helper function that writes exception detail to the event log. Exceptions
//        // are written to the event log as a security measure to avoid private database
//        // details from being returned to the browser. If a method does not return a status
//        // or boolean indicating the action succeeded or failed, a generic exception is also 
//        // thrown by the caller.
//        //
//        public void writeToEventLog(string action, Exception e)
//        {
//            EventLog log = new EventLog();
//            log.Source = this.Name;
//            log.Log = this.Name;

//            string message = "An exception occurred communicating with the data source.\n\n";
//            message += "Action: " + action + "\n\n";
//            message += "Exception: " + e.ToString();

//            log.WriteEntry(message);
//        }

//        /**
//         * helper function to retrieve config values of fail is they are missing.
//         */
//        public string readConfigOrFail(NameValueCollection config, Configs settingName)
//        {
//            string val = readConfigOrGetDefault(config, settingName, "MISSING");
//            if (val.Equals("MISSING"))
//            {
//                throw new System.Configuration.ConfigurationException("Missing required configuration " + settingName.ToString());
//            }
//            return val;
//        }

//        /**
//         * helper function to retrieve config values from the configuration file.
//         */
//        public string readConfigOrGetDefault(NameValueCollection config, Configs settingName, string defValue)
//        {
//            if (!String.IsNullOrEmpty(config[settingName.ToString()]))
//            {
//                return config[settingName.ToString()];
//            }
//            return defValue;
//        }

//        public void checkApplicationNameConfig(NameValueCollection config)
//        {
//            string appName = SettingsUtil.getWebSetting(SettingsUtil.APPLICATION_NAME);
//            if (String.IsNullOrEmpty(appName))
//            {
//                throw new ProviderException("Unable to find the application name. " +
//                    "Please create the " + SettingsUtil.APPLICATION_NAME + " key in appSettings");
//            }
//            else
//            {
//                config[Configs.applicationName.ToString()] = appName;
//            }
//        }
//    }

//    [System.Runtime.Remoting.Contexts.Context("MembershipProvider")]
//    public class TeinsaMembershipProvider : MembershipProvider
//    {
//        //
//        // Used when determining encryption key values.
//        //
//        private MachineKeySection machineKey;
//        private ProviderUtil util;

//        #region configs

//        public override void Initialize(string name, NameValueCollection config)
//        {
//            if (config == null)
//            {
//                throw new ArgumentNullException("config");
//            }
//            if (name == null || String.IsNullOrEmpty(name.Trim()))
//            {
//                name = "TeinsaMembershipProvider";
//            }

//            util = new ProviderUtil(name);

//            config[Configs.description.ToString()] = util.readConfigOrGetDefault(config, Configs.description,
//                "Custom Teinsa Membership Provider");

//            util.checkApplicationNameConfig(config);

//            base.Initialize(name, config);

//            try
//            {
//                ApplicationName = util.readConfigOrFail(config, Configs.applicationName);

//                pMaxInvalidPasswordAttempts = Convert.ToInt32(
//                    util.readConfigOrGetDefault(config, Configs.maxInvalidPasswordAttempts,
//                    SecurityUtil.ADAuthenticatorUtil.getMaxLoginFailures().ToString()));

//                util.WriteExceptionsToEventLog = Convert.ToBoolean(util.readConfigOrGetDefault(config,
//                    Configs.writeExceptionsToEventLog, "true"));

//                pPasswordFormat = MembershipPasswordFormat.Clear;
//                pEnablePasswordReset = false;
//                pEnablePasswordRetrieval = false;
//                pRequiresQuestionAndAnswer = false;
//                pRequiresUniqueEmail = false;
//                pPasswordAttemptWindow = 0;

//                //Configuration cfg = WebConfigurationManager.OpenWebConfiguration(HostingEnvironment.ApplicationVirtualPath);
//                //machineKey = (MachineKeySection)cfg.GetSectionGroup("system.web").Sections["machineKey"];
//                machineKey = ConfigurationManager.GetSection("system.web/machineKey") as MachineKeySection;
//                if (machineKey.ValidationKey.Contains("AutoGenerate"))
//                {
//                    if (PasswordFormat != MembershipPasswordFormat.Clear)
//                    {
//                        throw new ProviderException("Hashed or Encrypted passwords are not supportedwith auto-generated keys.");
//                    }
//                }
//            }
//            catch (Exception Ex)
//            {
//                throw new System.Configuration.ConfigurationErrorsException("There was an error reading the membership configuration settings", Ex);
//            }

//        }

//        #endregion

//        #region System.Web.Security.MembershipProvider methods implementation

//        /**
//         * MembershipProvider.ChangePassword
//         */
//        public override bool ChangePassword(string username, [SensitiveData] string oldPassword, [SensitiveData] string newPassword)
//        {
//            throw new NotImplementedException();
//        }

//        public override bool ChangePasswordQuestionAndAnswer(string username, [SensitiveData] string password, [SensitiveData] string newPasswordQuestion, [SensitiveData] string newPasswordAnswer)
//        {
//            throw new NotImplementedException();
//        }

//        public override MembershipUser CreateUser(string username, [SensitiveData] string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
//        {
//            throw new NotImplementedException();
//        }

//        public override bool DeleteUser(string username, bool deleteAllRelatedData)
//        {
//            throw new NotImplementedException();
//        }

//        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
//        {
//            throw new NotImplementedException();
//        }

//        public override int GetNumberOfUsersOnline()
//        {
//            throw new NotImplementedException();
//        }

//        public override string GetPassword(string username, [SensitiveData] string answer)
//        {
//            throw new ProviderException("Password Retrieval Not Enabled.");
//        }

//        public override MembershipUser GetUser(string username, bool userIsOnline)
//        {
//            using (DbManager db = new DbManager(util.getCurrectConnectionStringName()))
//            {
//                Usuario usuario = Usuario.readUserByName(db, username);
//                if (usuario == null)
//                {
//                    util.raiseError("GetUser(string username, bool userIsOnline)", new Exception("Ingreso fallido, usuario no registrado."));
//                }

//                MembershipUser mUser = fromUsuarioToMembershipUser(db, usuario);

//                if (userIsOnline)
//                {
//                    usuario.registrarIngreso(db, this.ApplicationName);
//                }
//                return mUser;
//            }
//        }

//        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
//        {
//            using (DbManager db = new DbManager(util.getCurrectConnectionStringName()))
//            {
//                Usuario usuario = Usuario.getById(db, Convert.ToInt32(providerUserKey), this.ApplicationName);
//                if (usuario == null)
//                {
//                    util.raiseError("GetUser(string username, bool userIsOnline)", new Exception("Ingreso fallido, usuario no registrado."));
//                }

//                MembershipUser mUser = fromUsuarioToMembershipUser(db, usuario);

//                if (userIsOnline)
//                {
//                    usuario.registrarIngreso(db, this.ApplicationName);
//                }
//                return mUser;
//            }
//        }
//        public override bool UnlockUser(string username)
//        {
//            try
//            {
//                using (DbManager db = new DbManager(util.getCurrectConnectionStringName()))
//                {
//                    Usuario usuario = Usuario.readUserByName(db, username);
//                    if (usuario == null)
//                    {
//                        return false;
//                    }
//                    usuario.desBloquear();
//                    usuario.actualizarUsuario(db);
//                    usuario.registrarActividad(db, TiposActividad.DesbloqueoCuenta,
//                        String.Format("{0} desloquea al usuario: {1}", SecurityUtil.getUserName(), username));
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public override string GetUserNameByEmail(string email)
//        {
//            throw new NotImplementedException();
//        }

//        public override string ResetPassword(string username, string answer)
//        {
//            throw new NotImplementedException();
//        }

//        public override void UpdateUser(MembershipUser user)
//        {
//            using (DbManager db = new DbManager(util.getCurrectConnectionStringName()))
//            {
//                Usuario usuario = Usuario.readUserByName(db, user.UserName);
//                usuario.Bloqueado = user.IsLockedOut;
//                usuario.actualizarUsuario(db);
//            }
//        }

//        public override bool ValidateUser(string username, [SensitiveData] string password)
//        {
//            using (DbManager db = new DbManager(util.getCurrectConnectionStringName()))
//            {
//                Usuario dbUser = Usuario.readUserByName(db, username);
//                if (dbUser == null)
//                    return false;
//                bool authenticated = authenticateWithActiveDirectory(username, password, this.getDefaultDomain());
//                bool isValid = (!dbUser.Bloqueado && authenticated);
//                if (isValid)
//                {
//                    dbUser.registrarIngreso(db, this.ApplicationName);
//                }
//                else
//                {
//                    dbUser.IntentosDeIngreso = dbUser.IntentosDeIngreso + 1;
//                    dbUser.actualizarUsuario(db);
//                }
//                return isValid;
//            }
//        }

//        private bool authenticateWithActiveDirectory(string userName, [SensitiveData] string password, string domain)
//        {
//            try
//            {
//                IntPtr token = IntPtr.Zero;
//                if (SecurityUtil.ADAuthenticatorUtil.LogonUser(userName, domain, password, ref token) == 0)//el token debe ser distinto de cero (0)
//                {
//                    return false;
//                }
//                return true;
//            }
//            catch (Exception ex)
//            {
//                util.raiseError("authenticateWithActiveDirectory(string userName, string password, string domain)",
//                    new ProviderException("Se ha presentado un problema inesperado. Por favor contacte al administrador", ex));
//            }
//            return false;
//        }

//        private string getDefaultDomain()
//        {
//            return SettingsUtil.getWebSetting(SettingsUtil.DOMAIN_NAME);
//        }

//        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
//            out int totalRecords)
//        {
//            MembershipUserCollection collection = new MembershipUserCollection();
//            using (DbManager db = new DbManager(util.getCurrectConnectionStringName()))
//            {
//                int offset = pageIndex * pageSize;
//                int limit = offset + pageSize;
//                List<Usuario> usuarios = db.SetCommand(@"SELECT * FROM
//                              ( SELECT row_number() OVER (ORDER BY nombre ASC) row_num, u.* FROM seguridad_usuario ) 
//                              usuarios
//                              WHERE row_num >= @offset AND row_num < @limit AND nombre LIKE @nombre",
//                              db.Parameter("@offset", offset), db.Parameter("@limit", limit),
//                              db.Parameter("@nombre", usernameToMatch)).ExecuteScalarList<Usuario>();
//                foreach (Usuario usuario in usuarios)
//                {
//                    collection.Add(fromUsuarioToMembershipUser(db, usuario));
//                }
//                totalRecords = usuarios.Count;
//            }
//            return collection;
//        }

//        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
//        {
//            throw new NotImplementedException();
//        }


//        #endregion

//        #region private utility methods

//        private MembershipUser fromUsuarioToMembershipUser(DbManager db, Usuario usuario)
//        {
//            return new MembershipUser(this.Name, usuario.Nombre, usuario.Id, "", "", "",
//                true, usuario.Bloqueado, usuario.FechaCreacion, usuario.getLastLogin(db).Fecha,
//                usuario.getLastActivity(db).Fecha, DateTime.Now, usuario.getLastLockedOut(db).Fecha);
//        }
//        #endregion

//        #region membershipProvider properties
//        //
//        // System.Web.Security.MembershipProvider properties.
//        //
//        private string pApplicationName;
//        private bool pEnablePasswordReset;
//        private bool pEnablePasswordRetrieval;
//        private bool pRequiresQuestionAndAnswer;
//        private bool pRequiresUniqueEmail;
//        private int pMaxInvalidPasswordAttempts;
//        private int pPasswordAttemptWindow;
//        private MembershipPasswordFormat pPasswordFormat;

//        public override string ApplicationName
//        {
//            get { return pApplicationName; }
//            set { pApplicationName = value; }
//        }

//        public override bool EnablePasswordReset
//        {
//            get { return pEnablePasswordReset; }
//        }


//        public override bool EnablePasswordRetrieval
//        {
//            get { return pEnablePasswordRetrieval; }
//        }


//        public override bool RequiresQuestionAndAnswer
//        {
//            get { return pRequiresQuestionAndAnswer; }
//        }


//        public override bool RequiresUniqueEmail
//        {
//            get { return pRequiresUniqueEmail; }
//        }


//        public override int MaxInvalidPasswordAttempts
//        {
//            get { return pMaxInvalidPasswordAttempts; }
//        }


//        public override int PasswordAttemptWindow
//        {
//            get { return pPasswordAttemptWindow; }
//        }


//        public override MembershipPasswordFormat PasswordFormat
//        {
//            get { return pPasswordFormat; }
//        }

//        private int pMinRequiredNonAlphanumericCharacters;

//        public override int MinRequiredNonAlphanumericCharacters
//        {
//            get { return pMinRequiredNonAlphanumericCharacters; }
//        }

//        private int pMinRequiredPasswordLength;

//        public override int MinRequiredPasswordLength
//        {
//            get { return pMinRequiredPasswordLength; }
//        }

//        private string pPasswordStrengthRegularExpression;

//        public override string PasswordStrengthRegularExpression
//        {
//            get { return pPasswordStrengthRegularExpression; }
//        }
//        #endregion
//    }

//    public class TeinsaRoleProvider : RoleProvider
//    {
//        private ProviderUtil util;

//        private string pApplicationName;

//        public override string ApplicationName
//        {
//            get { return pApplicationName; }
//            set { pApplicationName = value; }
//        }

//        public override void Initialize(string name, NameValueCollection config)
//        {
//            if (config == null)
//            {
//                throw new ArgumentNullException("config");
//            }
//            if (name == null || String.IsNullOrEmpty(name.Trim()))
//            {
//                name = "TeinsaRoleProvider";
//            }

//            util = new ProviderUtil(name);

//            util.checkApplicationNameConfig(config);

//            base.Initialize(name, config);

//            config[Configs.description.ToString()] = util.readConfigOrGetDefault(config, Configs.description,
//                "Custom Teinsa Role Provider");

//            this.ApplicationName = util.readConfigOrFail(config, Configs.applicationName);

//            util.WriteExceptionsToEventLog = Convert.ToBoolean(
//                util.readConfigOrGetDefault(config, Configs.writeExceptionsToEventLog, "true"));


//        }

//        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
//        {
//            foreach (string rolename in roleNames)
//            {
//                if (!RoleExists(((rolename == null) ? "" : rolename).Trim()))
//                {
//                    throw new ProviderException("Role name not found");
//                }
//            }

//            foreach (string username in usernames)
//            {
//                if (username.Contains(","))
//                {
//                    throw new ArgumentException("User names cannot contain commas.");
//                }
//                foreach (string rolename in roleNames)
//                {
//                    if (IsUserInRole(username, rolename))
//                    {
//                        throw new ProviderException("User is already in role");
//                    }
//                }
//            }
//            try
//            {
//                using (DbManager db = new DbManager(util.getCurrectConnectionStringName()))
//                {
//                    foreach (string username in usernames)
//                    {
//                        foreach (string rolename in roleNames)
//                        {
//                            Role role = Role.readRoleByName(db, rolename);
//                            Usuario user = Usuario.readUserByName(db, username);

//                            db.SetSpCommand("SP_Man_Role_Usuario",
//                              db.Parameter(ParameterDirection.Input, "@role_menu_id", 0, DbType.Int32),
//                              db.Parameter("@role_id", role.Id), db.Parameter("@usuario_id", user.Id),
//                              db.Parameter("@tipo", "I")).ExecuteNonQuery();
//                        }
//                    }
//                }
//            }
//            catch (Exception e)
//            {
//                util.raiseError("AddUsersToRoles(string[] usernames, string[] roleNames)", e);
//            }
//        }

//        public override void CreateRole(string roleName)
//        {
//            if (roleName.Contains(","))
//            {
//                throw new ArgumentException("Role names cannot contain commas.");
//            }
//            if (RoleExists(roleName))
//            {
//                throw new ProviderException("Role name already exists");
//            }
//            using (DbManager db = new DbManager(util.getCurrectConnectionStringName()))
//            {
//                db.SetCommand("SP_Man_Role",
//                    db.Parameter(ParameterDirection.Input, "@role_id", 0, DbType.Int32),
//                    db.Parameter("@nombre", roleName), db.Parameter("@descripcion", ""),
//                    db.Parameter("@activo", true), db.Parameter("@tipo", "I")).ExecuteNonQuery();
//            }
//        }

//        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
//        {
//            if (!RoleExists(roleName))
//            {
//                throw new ProviderException("Role does not exists");
//            }
//            if (throwOnPopulatedRole && GetUsersInRole(roleName).Length > 0)
//            {
//                throw new ProviderException("Cannot delete a role with related users");
//            }
//            using (DbManager db = new DbManager(util.getCurrectConnectionStringName()))
//            {
//                db.SetCommand("SP_Man_Role",
//                    db.Parameter(ParameterDirection.Input, "@role_id", 0, DbType.Int32),
//                    db.Parameter("@nombre", roleName), db.Parameter("@descripcion", ""),
//                    db.Parameter("@activo", true), db.Parameter("@tipo", "I")).ExecuteNonQuery();
//            }
//            return true;
//        }

//        public override string[] GetUsersInRole(string roleName)
//        {
//            using (DbManager db = new DbManager(util.getCurrectConnectionStringName()))
//            {
//                List<String> users = db.SetCommand(@"SELECT u.id, u.nombre, u.bloqueado, u.intentos_ingreso, u.eliminado, u.aplicacion, u.fecha_creacion
//                        FROM seguridad_role_usuario AS ru INNER JOIN
//                        seguridad_usuario AS u ON ru.usuario_id = u.id INNER JOIN
//                        seguridad_role AS r ON ru.role_id = r.id WHERE (r.nombre = @role_name)",
//                        db.Parameter("@role_name", roleName)).ExecuteScalarList<String>("Nombre");
//                return users.ToArray();
//            }
//        }

//        public override bool RoleExists(string rolename)
//        {
//            using (DbManager db = new DbManager(util.getCurrectConnectionStringName()))
//            {
//                return Role.count(db, rolename) > 0;
//            }
//        }

//        public override bool IsUserInRole(string username, string rolename)
//        {
//            using (DbManager db = new DbManager(util.getCurrectConnectionStringName()))
//            {
//                int count = db.SetCommand(@"SELECT count(*) FROM seguridad_role_usuario AS ru INNER JOIN
//                         seguridad_usuario AS u ON ru.usuario_id = u.id INNER JOIN
//                         seguridad_role AS r ON ru.role_id = r.id
//                         WHERE (u.nombre = @user_name) AND (r.nombre = @role_name)",
//                    db.Parameter("@usuario_id", username), db.Parameter("@role_id", rolename)).ExecuteScalar<Int32>();
//                return count > 0;
//            }
//        }

//        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
//        {
//            foreach (string rolename in roleNames)
//            {
//                if (!RoleExists(rolename))
//                {
//                    throw new ProviderException("Role name not found");
//                }
//            }

//            foreach (string username in usernames)
//            {
//                foreach (string rolename in roleNames)
//                {
//                    if (!IsUserInRole(username, rolename))
//                    {
//                        throw new ProviderException("User is not in role");
//                    }
//                }
//            }

//            try
//            {
//                using (DbManager db = new DbManager(util.getCurrectConnectionStringName()))
//                {
//                    foreach (string username in usernames)
//                    {
//                        foreach (string rolename in roleNames)
//                        {
//                            Int32 id = db.SetCommand(@"SELECT ru.id FROM seguridad_role_usuario AS ru INNER JOIN
//                                    seguridad_usuario AS u ON ru.usuario_id = u.id INNER JOIN
//                                    seguridad_role AS r ON ru.role_id = r.id
//                                    WHERE (r.nombre = @role_name) AND (u.nombre = @user_name)",
//                                    db.Parameter("@role_name", rolename), db.Parameter("@user_name", username))
//                                    .ExecuteScalar<Int32>();

//                            db.SetSpCommand("SP_Man_Role_Usuario",
//                              db.Parameter(ParameterDirection.Input, "@role_menu_id", id, DbType.Int32),
//                              db.Parameter("@role_id", 0), db.Parameter("@usuario_id", 0),
//                              db.Parameter("@tipo", "D")).ExecuteNonQuery();
//                        }
//                    }
//                }
//            }
//            catch (Exception e)
//            {
//                util.raiseError("RemoveUsersFromRoles(string[] usernames, string[] roleNames)", e);
//            }
//        }

//        public override string[] GetRolesForUser(string username)
//        {
//            using (DbManager db = new DbManager(util.getCurrectConnectionStringName()))
//            {
//                return db.SetCommand(@"SELECT R.id, R.nombre, R.descripcion, R.activo
//                              FROM seguridad_usuario AS U INNER JOIN
//                              seguridad_role_usuario AS RU ON U.id = RU.usuario_id INNER JOIN
//                              seguridad_role AS R ON R.id = RU.role_id
//                              WHERE (U.nombre LIKE @user_name)",
//                              db.Parameter("@user_name", username)).ExecuteScalarList<String>("Nombre").ToArray();
//            }
//        }

//        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
//        {
//            using (DbManager db = new DbManager(util.getCurrectConnectionStringName()))
//            {
//                return db.SetCommand(@"SELECT U.id, U.nombre, U.bloqueado, U.intentos_ingreso, U.eliminado, U.aplicacion, U.fecha_creacion
//                                     FROM seguridad_usuario AS U INNER JOIN
//                                     seguridad_role_usuario AS RU ON U.id = RU.usuario_id INNER JOIN
//                                     seguridad_role AS R ON R.id = RU.role_id
//                                     WHERE (R.nombre = @role_name) AND (U.nombre LIKE @user_to_match)",
//                                     db.Parameter("@role_name", roleName), db.Parameter("@user_to_match", usernameToMatch))
//                                     .ExecuteScalarList<String>("Nombre").ToArray();
//            }
//        }

//        public override string[] GetAllRoles()
//        {
//            using (DbManager db = new DbManager(util.getCurrectConnectionStringName()))
//            {
//                return db.SetCommand("SELECT * FROM SEGURIDAD_ROLE").ExecuteScalarList<string>("Nombre").ToArray();
//            }
//        }
//    }

//    public class TeinsaURLAuthorizationModule : ContextBoundObject, IHttpModule
//    {
//        public void Init(HttpApplication application)
//        {
//            application.AuthorizeRequest += new EventHandler(authorize);
//        }

//        public void authorize(object sender, EventArgs e)
//        {
//            HttpApplication application = (HttpApplication)sender;

//            if (SecurityUtil.existValidUser())
//            {
//                using (DbManager db = new DbManager(SettingsUtil.getWebSetting(SettingsUtil.ENVIRONMENT)))
//                {
//                    Menu menu = Menu.readByPath(db, WebAppUtil.removeVirtualPathAndConvertToLowerCase(application.Request.Path));
//                    if (menu != null)
//                    {
//                        foreach (Role menuRole in menu.getMenuRoles(db))
//                        {
//                            if (!application.User.IsInRole(menuRole.Nombre))
//                            {
//                                throw new HttpException(401, "UnAuthorized access to " + application.Request.Path);
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        public void Dispose()
//        {
//        }
//    }
//}