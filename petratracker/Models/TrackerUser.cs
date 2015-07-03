using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using petratracker.Code;

namespace petratracker.Models
{
    public class TrackerUser
    {
        #region Private Members

        private static TrackerDataContext _trackerDB;
        private static readonly string[] _adminRoles = { "Super User", "Administrator" }; // TODO: pull from DB

        #endregion

        #region Constructors

        static TrackerUser()
        {
            _trackerDB = (App.Current as App).TrackerDBo;
        }
        
        public TrackerUser()
        {
        }


        #endregion

        #region Public CurrentUser Methods

        public static void SetCurrentUser(string username, string password) 
        {
            try
            {
                (App.Current as App).CurrentUser = _trackerDB.Users.Single(u => u.username == username);
                if (!CheckPassword(password))
                {
                    (App.Current as App).CurrentUser = null;
                    throw new Exception("Password Error");
                }

            }
            catch (Exception e)
            {
                (App.Current as App).CurrentUser = null;
                throw new Exception("Username not found", e.InnerException);
            }
        }

        public static User GetCurrentUser()
        {
            try
            {
                return (App.Current as App).CurrentUser;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public static string GetCurrentUserTitle()
        {
            try
            {
                return String.Format("{0} {1} ({2})", (App.Current as App).CurrentUser.first_name,
                                                      (App.Current as App).CurrentUser.last_name,
                                                      (App.Current as App).CurrentUser.Role.role1);
            }
            catch(Exception e)
            {
               throw e;
            }
        }

        public static bool IsCurrentUserAdmin()
        {
            try
            {
                return _adminRoles.Contains((App.Current as App).CurrentUser.Role.role1);
            } 
            catch(Exception e)
            {
                throw e;
            }
        }

        public static bool IsCurrentUserSuperAdmin()
        {
            try
            {
                return (App.Current as App).CurrentUser.Role.role1.Equals("Super User");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool FirstLogin()
        {
            try
            {
                return (App.Current as App).CurrentUser.first_login;     
            }
            catch(Exception e)
            {
                return false;
                throw e;
            }
        }

        public static bool CheckPassword(string password)
        {
            try
            {
               return BCrypt.CheckPassword(password, (App.Current as App).CurrentUser.password);
            }
            catch(Exception e)
            {
                return false;
                throw e;
            }
        }

        internal static void UpdateCurrentUserPassword(string p)
        {
            try
            {
                var newpassword = BCrypt.HashPassword(p, BCrypt.GenerateSalt());
                (App.Current as App).CurrentUser.password = newpassword;
                (App.Current as App).CurrentUser.first_login = false;
                (App.Current as App).CurrentUser.modified_by = (App.Current as App).CurrentUser.id;
                (App.Current as App).CurrentUser.updated_at = DateTime.Now;
                _trackerDB.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        internal static void SaveAppearance(string SelectedTheme, string SelectedAccent)
        {
            try
            {
                (App.Current as App).CurrentUser.theme = SelectedTheme;
                (App.Current as App).CurrentUser.accent = SelectedAccent;
                (App.Current as App).CurrentUser.modified_by = (App.Current as App).CurrentUser.id;
                (App.Current as App).CurrentUser.updated_at = DateTime.Now;
                _trackerDB.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion
       
        #region Public User Methods

        public static IEnumerable<User> GetUsers()
        {
            return (from u in _trackerDB.Users select u);
        }

        public static IEnumerable<Role> GetRoles()
        {
            return (from r in _trackerDB.Roles select r);
        }

        public static void AddUser(string username, string password, string first_name, string last_name, string middle_name, int role)
        {           
            try
            {
                User newUser = new User();
                newUser.username = username;
                newUser.password = BCrypt.HashPassword(password, BCrypt.GenerateSalt());
                newUser.middle_name = middle_name;
                newUser.first_name = first_name;
                newUser.last_name = last_name;
                newUser.role_id = role;
                newUser.first_login = true;
                newUser.theme = "BaseLight";
                newUser.accent = "Blue";
                newUser.modified_by = (App.Current as App).CurrentUser.id;
                newUser.created_at = DateTime.Now;
                newUser.updated_at = DateTime.Now;
                _trackerDB.Users.InsertOnSubmit(newUser);
                _trackerDB.SubmitChanges();

                SendEmail.sendNewUserMail(first_name + " " + last_name, username, password);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        internal static void ResetPasswordRequest(string username)
        {
            try
            {
                var user = _trackerDB.Users.Single(u => u.username == username);
                //SendEmail.sendResetPasswordMail(username);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal static void AddNewRole(string name, string description)
        {
            try
            {
                Role newRole = new Role();
                newRole.role1 = name;
                newRole.description = description;
                newRole.modified_by = (App.Current as App).CurrentUser.id;
                newRole.created_at = DateTime.Now;
                newRole.updated_at = DateTime.Now;
                _trackerDB.Roles.InsertOnSubmit(newRole);
                _trackerDB.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        #endregion
    }
}
